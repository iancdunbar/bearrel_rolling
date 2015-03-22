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

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private float jump_strength;
    [SerializeField]
    private float max_speed;

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
	}
	//Slow the bear down if it collides with a tree
	void OnTriggerExit2D( Collider2D other )
	{
		if ( other.tag == "tree" )
		{
			max_speed = 10;
			other.gameObject.transform.Rotate (0,0,-2);
			StartCoroutine(SpeedLimitCooldown());
		}
	}
	IEnumerator SpeedLimitCooldown (){
		yield return new WaitForSeconds(1);
		max_speed = 200;
	}

    /////////////////////////////////////////////
}
