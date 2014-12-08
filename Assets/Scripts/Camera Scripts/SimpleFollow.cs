using UnityEngine;
using System.Collections;

public class SimpleFollow : MonoBehaviour {

    //////////////////////////////////////
    // Private Variables
    //////////////////////////////////////

    private Transform transRef;

    //////////////////////////////////////


    //////////////////////////////////////
    // Inspector Variables
    //////////////////////////////////////
    
    [SerializeField]    // The target for the object to follow
    private Transform   followTarget;
    [SerializeField]    // The target offset for the following object2
    private Vector3     targetOffset;
    [SerializeField]    // The maximum speed that the camera can move
    private float       maxSpeed;
    [SerializeField]    // Whether or not the update should happen in the Fixed update step
    private bool        updateOnFixed;

    //////////////////////////////////////


    //////////////////////////////////////
    // Unity Message Handling
    //////////////////////////////////////

	// Use this for initialization
	void Awake ( ) 
    {
        // Get the reference to this transform for future use
        transRef = this.transform;
	}
	
	// Update is called once per frame
	void Update ( ) 
    {
        if( !updateOnFixed )
        {
            transRef.position = Vector3.MoveTowards( transRef.position, followTarget.position + targetOffset, maxSpeed * Time.deltaTime);
        }
	}

    void FixedUpdate( )
    {
        if( updateOnFixed )
        {
            transRef.position = Vector3.MoveTowards( transRef.position, followTarget.position + targetOffset, maxSpeed * Time.fixedDeltaTime );
        }

    }

    //////////////////////////////////////
}
