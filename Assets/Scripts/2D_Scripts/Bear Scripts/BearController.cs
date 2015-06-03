using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BearController : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variables
    /////////////////////////////////////////////

    private Rigidbody2D rbody;
    private bool jumped;
    private bool slammed;
    private BearStateController bsc;
	private bool deathBool = false;

    private float saved_forward_vel;

	//HUD
	private int currentSmashDashNumber;
	private Image SmashDashHUDPrefab;
	private Color HUDColor = new Color(0.957f, 1.000f, 0.221f, 1.000f);
	private const int maxInvulnSliderAmount = 100;
	private Slider invulnSlider;
	private static string[] deathMessages = {"U DED ;_;"};
	private static int random = Random.Range(0, deathMessages.Length);
	private static string deathMessage = deathMessages[random];

    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    public bool dashed = false;
    public bool isSloped = false;
    public float currentSpeed;
    public Vector2 currentVelocity;

    [SerializeField]
    private float jump_strength;
	[SerializeField]
	private float dash_strength;
	[SerializeField]
	private float jump_distance;
    [SerializeField]
	private float max_speed; //The max speed regardless of the accelleration
	[SerializeField]
	private float min_speed; //The minimum speed regardless of the accelleration - cant let the bear roll backwards!
	[SerializeField]
	private float min_accelleration; //minimum accell at all times
	[SerializeField]
	private float collision_speed;
	[SerializeField]
	private float tree_decell_amount;
	[SerializeField]
	private float invuln_bar_regen_rate;
	[SerializeField]
	public float invuln_bar_tree_penalty;
	[SerializeField]
	private float slow_duration;
	[SerializeField]
	private float invulnDuration;
	private float return_value;
	[SerializeField]
	private float boostSpeed;
	[SerializeField]
	private float slam_speed;
	[SerializeField]
	private Vector2 normal;
	private bool Grounded;
	public float dash_cooldown;
	public GameObject mCamera;
	private float current_accelleration;
	public float knockback;
	public bool jumpElligible = true;
	public bool bearInvuln = false;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
	/// 
    // State Messages
    /////////////////////////////////////////////

    public void OnEnterState( object arg )
    {
        BearState state = (BearState)arg;
		
        Debug.Log( "Entering " + state + " from BearController" );
    }

    /////////////////////////////////////////////

    /////////////////////////////////////////////
    // Input Messages
    /////////////////////////////////////////////

	//JUMP//
    public void OnSwipeUp( object unused )
    {
        if(jumpElligible)
        {
            rbody.AddForce( Vector2.up * jump_strength, ForceMode2D.Impulse );
			rbody.AddForce( Vector2.right * jump_distance, ForceMode2D.Impulse );
            bsc.ChangeState( BearState.JUMPING );
            jumped = true;
			jumpElligible = false;
        }
    }

	//DASH//
	public void OnTap( object unused )
	{
		if( !dashed && dashed == false) 
		{
			//Spin & increase velocity in both directions
			rbody.AddTorque (-200, ForceMode2D.Impulse);
			rbody.AddForce( new Vector2(rbody.velocity.x * dash_strength, rbody.velocity.x * -dash_strength), ForceMode2D.Impulse);
			dashed = true;
			bsc.ChangeState( BearState.DASHING );
			StartCoroutine (DashCoolDown());
		}
	}

	//SLAM//
	public void OnSwipeDown( object unused )
    {

		if( !slammed && jumpElligible == false)
        {
            saved_forward_vel = rbody.velocity.x;
			//Jared's temp adjustment
			//AddForce down instantly
			rbody.AddForce (Vector2.up * -slam_speed, ForceMode2D.Impulse);
			//AddTorque to spin the bear by ammount (Z-axis)
			rbody.AddTorque (-200, ForceMode2D.Impulse);
			//Original slamming
            //rigidbody2D.velocity = new Vector2( rigidbody2D.velocity.x, -10 );
            bsc.ChangeState( BearState.SLAMMING );
            slammed = true;
            jumped = true;
        }

    }


    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

	void Awake( )
	{
		rbody = GetComponent<Rigidbody2D>( );
		bsc = new BearStateController( );

		SmashDashHUDPrefab = Resources.LoadAssetAtPath<Image>("Assets/Resources/HUD/DashSmash_Container.prefab");

		GameObject invulnSliderObj = GameObject.Find("Slider");

		if (invulnSliderObj != null) {
			invulnSlider = invulnSliderObj.GetComponent<Slider>();	

			invulnSlider.maxValue = maxInvulnSliderAmount;
			invulnSlider.value = 0;
		}
		
		initialize_HUD ();
		
		jumped = true;
	}

    // Use this for initialization
	void Start () 
    {
		mCamera = GameObject.FindGameObjectWithTag("MainCamera");
		return_value = max_speed;
        MessageDispatch.RegisterListener( "OnSwipeUp", OnSwipeUp );
        MessageDispatch.RegisterListener( "OnSwipeDown", OnSwipeDown );
        MessageDispatch.RegisterListener( "OnEnterState", OnEnterState );
		MessageDispatch.RegisterListener( "OnTap", OnTap );

        bsc.ChangeState( BearState.IDLE );
	}

    void OnCollisionEnter2D( Collision2D other )
    {

        if( other.gameObject.tag == "Ground" )
        {
			jumpElligible = true; //if you jump, you may jump again after contacting the ground

            bsc.ChangeState( BearState.IDLE );

            rbody.velocity = rbody.velocity + new Vector2( saved_forward_vel, 0 );

			Grounded = true;
            jumped = false;
            slammed = false;

			max_speed = return_value;
        }
	
    }

	void OnCollisionExit2D (Collision2D other)
	{
		/*if (other.gameObject.tag == "Ground")
		{
			Grounded = false;
		}*/
		if (other.gameObject.tag == "Rock")
		{
			Grounded = false;
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
    {

		currentSpeed = rbody.velocity.magnitude;
		currentVelocity = rbody.velocity;

		// If the bear is on the ground they cannot SLAM-A-JAM.
		if (Grounded == true)
		{
			slammed = false;
		}

//		//Detecting the normal direction of the terrain below
//		RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 1, transform.position.y + 2), -Vector2.up * 1);
//		Debug.DrawRay (new Vector2(transform.position.x - 1, transform.position.y + 2), -Vector2.up * 1, Color.green);
//
//		//What is the Vector2 normal of the object being hit by the raycast
//		//Debug.Log (hit.normal);
//
//		//Whats the tag of the collider hit by the raycast
//		//Debug.Log (hit.collider.tag);
//	
//
//		//If the x value of the normal being detected is greater than or = to 0.1 and the collider is named Ground then the terrain is sloped
//		if( hit.collider == null ){ 
//			Debug.Log( "The hit is null, this is what is causing the program to crash"); 
//		}
//		if (hit.normal.y <= 0.83f && hit.collider.tag == "Ground")
//		{
//			isSloped = true;
//		}
//		else
//		{
//			isSloped = false;
//		}


		if (current_accelleration < min_accelleration) {
			current_accelleration = min_accelleration;
		}

		if (currentSpeed < min_speed) {
			rbody.velocity = rbody.velocity + (rbody.velocity.normalized * min_speed);
		} else{
			rbody.velocity = rbody.velocity + (rbody.velocity.normalized * current_accelleration);
		}

		//Clamp the maximum velocity of the bear to max_speed
		//invulnBear means youre camping to max
		if ((rbody.velocity.magnitude > max_speed) || bearInvuln) {
			rbody.velocity = Vector3.ClampMagnitude (rbody.velocity, max_speed);
		}


	}

	//HUD GUI stuff

	void OnGUI(){
		if(deathBool){

			GUI.Box (new Rect (0,0,Screen.width,Screen.height), "<color=red><size=80>" + deathMessage + "</size></color>");
		}

		if (bearInvuln) {
			GUI.Box (new Rect (0,0,150,30), "<color=yellow><size=12>INVULNERABEAR</size></color>");
		}
	}

	private void initialize_HUD()
	{
		float smashHudElementWidth = SmashDashHUDPrefab.rectTransform.sizeDelta.x;

		InvokeRepeating ("regenInvulnSliderConstant", 0f, 0.2f);

	}

	private void toggleInvulnerabear()
	{
		if (bearInvuln) {
			bearInvuln = false;
			max_speed = max_speed -= 10;
			min_speed = min_speed -= 10;
		} else {
			bearInvuln = true;
			max_speed = max_speed += 10;
			min_speed = min_speed += 10;
		}
	}

	private void regenInvulnSliderConstant(){

		if (bearInvuln) {
			invulnSlider.value -= invuln_bar_regen_rate/2;

			if(invulnSlider.value <= 0){
				toggleInvulnerabear();
			}

		} else {
			if (invulnSlider.value + invuln_bar_regen_rate > maxInvulnSliderAmount) {
				//smashDashRegenSlider.value = smashDashRegenSlider.value + smash_bar_regen_rate - maxRegenSliderAmount;
				//fillSmashDashBox ();
				toggleInvulnerabear();
			} else {
				invulnSlider.value += invuln_bar_regen_rate; 
			}
		}

	}

	public void decreaseInvulnSlider(float decreaseAmount){

		if (invulnSlider.value - decreaseAmount < 0) {
			invulnSlider.value = 0;
		} else {
			invulnSlider.value -= decreaseAmount; 
		}
		
		
	}

	private void HandleEndGame(GameCamera gameCam)
	{
		GameObject town = GameObject.Find ("Town");
		Transform townTransform = town.GetComponent(typeof(Transform)) as Transform;

		GameObject avalanche = GameObject.Find ("Avalance");


		Vector3 avalancheSpawnPos = townTransform.position + new Vector3(-300,0,0);

		avalanche.transform.position = avalancheSpawnPos;


		Vector3 avalancheTargetPos = townTransform.position + new Vector3 (1000, -100, 0);

		gameCam.follow_target = townTransform;
		gameCam.keepStatic = true;
		
		AvalanceController thisInstance = AvalanceController.Instance;
		thisInstance.PathOverride = true;
		thisInstance.PathOverrideTarget = avalancheTargetPos;

		//AvalanceController.AddPoint (avalancheTargetPos);

		gameCam.transform.position = new Vector3 (-445, -320, 10);

		//gameCam.keepStatic = true;
		//gameCam.offset = new Vector3 (0, 0, 0);
		//gameCam.MovingOffset = gameCam.offset;
		//gameCam.FastOffset = gameCam.offset;

	    //yield WaitForSeconds(3.0f);
		//Application.LoadLevel(Application.loadedLevel);

	}


	//Slow the bear down if it collides with a tree
	IEnumerator OnTriggerEnter2D( Collider2D other )
	{
	
		if ( other.tag == "tree" && (bsc.current_state == BearState.DASHING || bsc.current_state == BearState.SLAMMING))
		{

		}
		else
		{
			if(other.tag=="tree")
			{
				//max_speed = collision_speed;
				other.gameObject.transform.Rotate (0,0,-4);
				//StartCoroutine(SpeedLimitCooldown());
				decreaseInvulnSlider(invuln_bar_tree_penalty);
				current_accelleration += tree_decell_amount;
				if(rbody.velocity.magnitude + tree_decell_amount > 0){
					rbody.velocity = rbody.velocity + (rbody.velocity.normalized * tree_decell_amount);
				} else {
					rbody.velocity = rbody.velocity + (rbody.velocity.normalized * min_speed);
				}
			}

		}
		if(other.tag=="Rock")
		{
			Grounded = true;
			//GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

			//KNOCK BACK //////

			//GetComponent<Rigidbody2D>().AddForce (Vector2.right * -knockback, ForceMode2D.Impulse );
			//GetComponent<Rigidbody2D>().AddForce (Vector2.up * knockback, ForceMode2D.Impulse );
		}
		if(other.tag == "tree_gib")
		{
			Grounded = true;
		}

		if (other.tag =="Boost")
		{
			rbody.AddForce(Vector2.right * boostSpeed);
			StartCoroutine(BoostCoolDown());
		}

		if ( other.tag =="Death" && deathBool == false)
		{
			
			Debug.Log ("Avalanche impact");
			GameCamera camera = mCamera.GetComponent<GameCamera>();
			//camera.follow_target = GameObject.Find ("Avalance").GetComponent(typeof(Transform)) as Transform;
			//camera.GetComponent<GameCamera>().offset = mCamera.GetComponent<GameCamera>().AvalancheOffset;
			this.rbody.isKinematic = true; //stop the bear, otherwise camera will be affected by impacts
			deathBool = true;

			yield return new WaitForSeconds(3.0f);
			deathBool = false;
			//Application.LoadLevel(Application.loadedLevel);

			HandleEndGame(camera);
		}
		if (other.tag == "Avalanche")
		{
		}

	}
	IEnumerator SpeedLimitCooldown (){
		yield return new WaitForSeconds(slow_duration);
		max_speed = return_value;
	}
	IEnumerator DashCoolDown (){
		yield return new WaitForSeconds(dash_cooldown);
		dashed = false;
	}
	IEnumerator BoostCoolDown(){
		yield return new WaitForSeconds(2);
		//gameObject.rigidbody2D.velocity = max_speed;
	}


    /////////////////////////////////////////////
}
