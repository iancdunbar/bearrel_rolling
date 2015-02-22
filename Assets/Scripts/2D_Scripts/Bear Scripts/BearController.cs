using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    private Rigidbody2D rbody;

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
        rigidbody2D.AddForce( Vector2.up * jump_strength, ForceMode2D.Impulse );
        Debug.Log( "Swipe Up" );
    }

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

    void Awake( )
    {
        rbody = GetComponent<Rigidbody2D>( );
    }

    // Use this for initialization
	void Start () 
    {
        MessageDispatch.RegisterListener( "OnSwipeUp", OnSwipeUp );
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        rbody.velocity = Vector3.ClampMagnitude( rbody.velocity, max_speed );
	}

    /////////////////////////////////////////////
}
