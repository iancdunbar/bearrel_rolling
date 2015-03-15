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
    private float max_speed;

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
	}

    /////////////////////////////////////////////
}
