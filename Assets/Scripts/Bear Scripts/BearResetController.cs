using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Rigidbody ) )]
public class BearResetController : MonoBehaviour {

    //////////////////////////////////////
    // Private Variables
    //////////////////////////////////////

    private Transform transRef;
    private Rigidbody rigidRef;
    private Vector3 defaultPos;
    private Quaternion defaultAngles;

    //////////////////////////////////////

    //////////////////////////////////////
    // GUI Messages
    //////////////////////////////////////

    private void OnReset( )
    {
        transRef.position = defaultPos;
        transRef.rotation = defaultAngles;

        rigidRef.velocity = Vector3.zero;
    }



    //////////////////////////////////////
    // Unity Messages
    //////////////////////////////////////

	// Use this for initialization
	void Awake () 
    {
        transRef = this.transform;
        rigidRef = this.rigidbody;
	}

    void Start( )
    {
        defaultPos = transRef.position;
        defaultAngles = transRef.rotation;
    }

    //////////////////////////////////////
}
