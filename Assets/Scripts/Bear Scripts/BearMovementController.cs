using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Rigidbody ) )]
public class BearMovementController : MonoBehaviour {

    //////////////////////////////////////
    // Private Variables
    //////////////////////////////////////

    private Transform transRef;
    private Rigidbody rigidbodyRef;

    //////////////////////////////////////


    //////////////////////////////////////
    // Inspector Variables
    //////////////////////////////////////

    [SerializeField]
    private float impulseForce;

    //////////////////////////////////////

	// Use this for initialization
	void Awake () 
    {
        transRef = this.transform;
        rigidbodyRef = this.rigidbody;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if( Input.GetKey( KeyCode.LeftArrow ) )
        {
            rigidbodyRef.AddForce( Vector3.forward * impulseForce, ForceMode.Impulse );
        }

        if( Input.GetKey( KeyCode.RightArrow ) )
        {
            rigidbodyRef.AddForce( Vector3.back * impulseForce, ForceMode.Impulse );
        }

        if( Input.GetKey( KeyCode.UpArrow ) )
        {
            rigidbodyRef.AddForce( Vector3.right * impulseForce, ForceMode.Impulse );
        }

        if( Input.GetKey( KeyCode.DownArrow ) )
        {
            rigidbodyRef.AddForce( Vector3.left * impulseForce, ForceMode.Impulse );
        }
	}
}
