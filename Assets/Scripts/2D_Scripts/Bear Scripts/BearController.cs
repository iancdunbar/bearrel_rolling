using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BearController : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    private Rigidbody2D rbody;
    private bool jumped;
    private bool slammed;
	public bool dashed = false;
    private BearStateController bsc;
	private bool deathBool = false; 
	public bool isSloped = false;
	public float currentSpeed;
	public Vector2 currentVelocity;

	//HUD
	private int currentSmashDashNumber;
	private Image SmashDashHUDPrefab;
	private GameObject SmashDashMeterContainer; 
	private Image[] SmashDashContainers;
	private Color HUDColor = new Color(0.957f, 1.000f, 0.221f, 1.000f);
	private const int maxRegenSliderAmount = 100;
	private Slider smashDashRegenSlider;
	private static string[] deathMessages = {"FUCK TUCKER","U DED ;_;", "LIFE IS FUTILE", "SO METAL"};
	private static int random = Random.Range(0, deathMessages.Length);
	private static string deathMessage = deathMessages[random];


    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

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
	private int max_smashes_and_dashes;
	[SerializeField]
	private float smash_bar_regen_rate;
	[SerializeField]
	public float smash_bar_tree_bonus;
	[SerializeField]
	private float slow_duration;
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


    /////////////////////////////////////////////


    /////////////////////////////////////////////
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
        if( !jumped && Grounded == true)
        {
            GetComponent<Rigidbody2D>().AddForce( Vector2.up * jump_strength, ForceMode2D.Impulse );
			GetComponent<Rigidbody2D>().AddForce( Vector2.right * jump_distance, ForceMode2D.Impulse );
            bsc.ChangeState( BearState.JUMPING );
            jumped = true;
        }
    }

	//DASH//
	public void OnTap( object unused )
	{
		if( !dashed && dashed == false && currentSmashDashNumber > 0) 
		{
			//Spin & increase velocity in both directions
			GetComponent<Rigidbody2D>().AddTorque (-200, ForceMode2D.Impulse);
			GetComponent<Rigidbody2D>().AddForce( new Vector2(GetComponent<Rigidbody2D>().velocity.x * dash_strength, GetComponent<Rigidbody2D>().velocity.x * -dash_strength), ForceMode2D.Impulse);
			dashed = true;
			bsc.ChangeState( BearState.DASHING );
			emptySmashDashBox();
			StartCoroutine (DashCoolDown());
		}
	}

	//SLAM//
	public void OnSwipeDown( object unused )
    {

		if( !slammed && Grounded == false && currentSmashDashNumber > 0)
        {
			//Jared's temp adjustment
			//AddForce down instantly
			GetComponent<Rigidbody2D>().AddForce (Vector2.up * -slam_speed, ForceMode2D.Impulse);
			//AddTorque to spin the bear by ammount (Z-axis)
			GetComponent<Rigidbody2D>().AddTorque (-200, ForceMode2D.Impulse);
			//Original slamming
            //rigidbody2D.velocity = new Vector2( rigidbody2D.velocity.x, -10 );
            bsc.ChangeState( BearState.SLAMMING );
			emptySmashDashBox();
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
		SmashDashContainers = new Image[max_smashes_and_dashes];
		
		currentSmashDashNumber = max_smashes_and_dashes;

		SmashDashHUDPrefab = Resources.LoadAssetAtPath<Image>("Assets/Resources/HUD/DashSmash_Container.prefab");

		GameObject smashDashRegenSliderObj = GameObject.Find("Slider");

		if (smashDashRegenSliderObj != null) {
			smashDashRegenSlider = smashDashRegenSliderObj.GetComponent<Slider>();	

			smashDashRegenSlider.maxValue = maxRegenSliderAmount;
			smashDashRegenSlider.value = maxRegenSliderAmount;
		}

		SmashDashMeterContainer = GameObject.Find("DashSmash_Meter");

		
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
            bsc.ChangeState( BearState.IDLE );
			Grounded = true;
            jumped = false;
            slammed = false;

			max_speed = return_value;
        }
	
    }

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.tag == "Ground")
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
		//Detecting the normal direction of the terrain below
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 1, transform.position.y + 2), -Vector2.up * 1);
		Debug.DrawRay (new Vector2(transform.position.x - 1, transform.position.y + 2), -Vector2.up * 1, Color.green);

		//What is the Vector2 normal of the object being hit by the raycast
		//Debug.Log (hit.normal);

		//Whats the tag of the collider hit by the raycast
		//Debug.Log (hit.collider.tag);
	

		//If the x value of the normal being detected is greater than or = to 0.1 and the collider is named Ground then the terrain is sloped
		if( hit.collider == null ){ 
			Debug.Log( "The hit is null, this is what is causing the program to crash"); 
		}
		if (hit.normal.y <= 0.83f && hit.collider.tag == "Ground")
		{
			isSloped = true;
		}
		else
		{
			isSloped = false;
		}

		if (current_accelleration < min_accelleration) {
			current_accelleration = min_accelleration;
		}

		if (rbody.velocity.magnitude < min_speed) {
			rbody.velocity = rbody.velocity + (rbody.velocity.normalized * min_speed);
		} else{
			rbody.velocity = rbody.velocity + (rbody.velocity.normalized * current_accelleration);
		}

		//Clamp the maximum velocity of the bear to max_speed
		rbody.velocity = Vector3.ClampMagnitude( rbody.velocity, max_speed );


	}

	//HUD GUI stuff

	void OnGUI(){
		if(deathBool){

			GUI.Box (new Rect (0,0,Screen.width,Screen.height), "<color=red><size=80>" + deathMessage + "</size></color>");
		}
	}

	private void initialize_HUD()
	{
		float smashHudElementWidth = SmashDashHUDPrefab.rectTransform.sizeDelta.x;

		for (int i = 1; i <= max_smashes_and_dashes; i++) {
			//instantiate yellow HUD box for each smash/dash in the max number
			//x pos = i*length + (i-1)*3

			Image SmashDashHudImage = Instantiate(SmashDashHUDPrefab) as Image;
			SmashDashHudImage.rectTransform.position = new Vector3 (smashHudElementWidth*(i-1) + (i-1)*3,0,0);

			SmashDashHudImage.rectTransform.parent = SmashDashMeterContainer.transform;

			SmashDashContainers[i-1] = SmashDashHudImage;

		}

		for (int i = 0; i < max_smashes_and_dashes; i++) {
			SmashDashContainers[i].transform.localScale = new Vector3(1,1,1); //HACK
		}

		InvokeRepeating ("regenSmashDashBarConstant", 0f, 0.2f);

	}

	private void regenSmashDashBarConstant(){

		if (smashDashRegenSlider.value + smash_bar_regen_rate > maxRegenSliderAmount) {
			smashDashRegenSlider.value = smashDashRegenSlider.value + smash_bar_regen_rate - maxRegenSliderAmount;
			fillSmashDashBox ();
		} else {
			smashDashRegenSlider.value += smash_bar_regen_rate; 
		}


	}

	public void regenSmashDashBar(float refillAmount){

		if (smashDashRegenSlider.value + refillAmount > maxRegenSliderAmount) {
			smashDashRegenSlider.value = smashDashRegenSlider.value + refillAmount - maxRegenSliderAmount;
			fillSmashDashBox ();
		} else {
			smashDashRegenSlider.value += refillAmount; 
		}
		
		
	}

	private void fillSmashDashBox()
	{
		if (currentSmashDashNumber < max_smashes_and_dashes) {
			Image SmashDashContainerToFill = SmashDashContainers [currentSmashDashNumber];

			SmashDashContainerToFill.color = HUDColor;

			currentSmashDashNumber += 1;
		}
	}

	private void emptySmashDashBox()
	{

		if (currentSmashDashNumber > 0) {
			Image SmashDashContainerToEmpty = SmashDashContainers[currentSmashDashNumber-1];

			SmashDashContainerToEmpty.color = Color.clear;

			currentSmashDashNumber -= 1;
		}
	}


	//Slow the bear down if it collides with a tree
	IEnumerator OnTriggerEnter2D( Collider2D other )
	{
	
		if ( other.tag == "tree" && (bsc.current_state == BearState.DASHING || bsc.current_state == BearState.SLAMMING))
		{
			regenSmashDashBar(smash_bar_tree_bonus);
		}
		else
		{
			if(other.tag=="tree")
			{
				//max_speed = collision_speed;
				other.gameObject.transform.Rotate (0,0,-4);
				//StartCoroutine(SpeedLimitCooldown());
				current_accelleration += tree_decell_amount;
				if(rbody.velocity.magnitude + tree_decell_amount > min_speed){
					rbody.velocity = rbody.velocity + (rbody.velocity.normalized * tree_decell_amount);
				} else {
					rbody.velocity = rbody.velocity + (rbody.velocity.normalized * min_speed);
				}
			}

		}

		if (other.tag =="Boost")
		{
			gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * boostSpeed);
			StartCoroutine(BoostCoolDown());
		}

		if ( other.tag =="Death")
		{
			
			Debug.Log ("Avalanche impact");
			mCamera.GetComponent<GameCamera>().follow_target = GameObject.Find ("Avalance").GetComponent(typeof(Transform)) as Transform;;
			mCamera.GetComponent<GameCamera>().offset = mCamera.GetComponent<GameCamera>().AvalancheOffset;
			deathBool = true;

			yield return new WaitForSeconds(5.0f);
			Application.LoadLevel(Application.loadedLevel);

		
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
