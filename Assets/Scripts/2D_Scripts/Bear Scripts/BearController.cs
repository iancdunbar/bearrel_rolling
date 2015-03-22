using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    private Rigidbody2D rbody;
    private bool jumped;
    private bool slammed;
    private BearStateController bsc;
	private bool deathBool = false;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private float jump_strength;
	[SerializeField]
	private float jump_distance;
    [SerializeField]
    private float max_speed;
	[SerializeField]
	private float collision_speed;
	[SerializeField]
	private float slow_duration;
	private float return_value;
	[SerializeField]
	private float boostSpeed;


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

    public void OnSwipeUp( object unused )
    {
        if( !jumped )
        {
            rigidbody2D.AddForce( Vector2.up * jump_strength, ForceMode2D.Impulse );
			rigidbody2D.AddForce( Vector2.right * jump_distance, ForceMode2D.Impulse );
            jumped = true;
        }
    }

    public void OnSwipeDown( object unused )
    {

        if( !slammed )
        {
            rigidbody2D.velocity = new Vector2( rigidbody2D.velocity.x, -10 );
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

        jumped = true;
    }

    // Use this for initialization
	void Start () 
    {
		return_value = max_speed;
        MessageDispatch.RegisterListener( "OnSwipeUp", OnSwipeUp );
        MessageDispatch.RegisterListener( "OnSwipeDown", OnSwipeDown );
        MessageDispatch.RegisterListener( "OnEnterState", OnEnterState );

        bsc.ChangeState( BearState.IDLE );
	}

    void OnCollisionEnter2D( Collision2D other )
    {

        if( other.gameObject.tag == "Ground" )
        {
            jumped = false;
            slammed = false;
        }
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
		rbody.velocity = Vector3.ClampMagnitude( rbody.velocity, max_speed );
		Debug.Log(gameObject.rigidbody2D.velocity);
	}

	void OnGUI(){
		if(deathBool){
			GUI.Box (new Rect (0,0,Screen.width,Screen.height), "<color=red><size=80>BEAR DEATH</size></color>");
		}
	}

	//Slow the bear down if it collides with a tree
	IEnumerator OnTriggerEnter2D( Collider2D other )
	{
	
		if ( other.tag == "tree" )
		{
			max_speed = collision_speed;
			other.gameObject.transform.Rotate (0,0,-4);
			StartCoroutine(SpeedLimitCooldown());
		}

		if (other.tag =="Boost")
		{
			gameObject.rigidbody2D.AddForce(Vector2.right * boostSpeed);
			StartCoroutine(BoostCoolDown());
		}

		if ( other.tag =="Death")
		{
			deathBool = true;
			yield return new WaitForSeconds(5.0f);
			Application.LoadLevel(Application.loadedLevel);
		
		}
	}
	IEnumerator SpeedLimitCooldown (){
		yield return new WaitForSeconds(slow_duration);
		max_speed = return_value;
	}
	IEnumerator BoostCoolDown(){
		yield return new WaitForSeconds(2);
	//	gameObject.rigidbody2D.velocity = max_speed;
	}

    /////////////////////////////////////////////
}
