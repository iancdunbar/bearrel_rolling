using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    private Rigidbody2D rbody;
    private bool jumped;

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

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

    void Awake( )
    {
        rbody = GetComponent<Rigidbody2D>( );
        jumped = true;
    }

    // Use this for initialization
	void Start () 
    {
		return_value = max_speed;
        MessageDispatch.RegisterListener( "OnSwipeUp", OnSwipeUp );
	}

    void OnCollisionEnter2D( Collision2D other )
    {

        if( other.gameObject.tag == "Ground" )
        {
            jumped = false;
        }
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
		rbody.velocity = Vector3.ClampMagnitude( rbody.velocity, max_speed );
		Debug.Log(gameObject.rigidbody2D.velocity);
	}

	//Slow the bear down if it collides with a tree
	void OnTriggerEnter2D( Collider2D other )
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
			Application.LoadLevel("Arttest");
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
