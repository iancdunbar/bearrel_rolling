using UnityEngine;
using System.Collections;

public class TrackController : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variables
    /////////////////////////////////////////////
    
    private Transform trans_ref;
    private Vector3 previous_position;
    private Vector3 target_position;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private float speed;
    [SerializeField]
    private Turntable turntable_generator;
    [SerializeField]
    private SimpleColliderGenerator simple_collider;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

	// Use this for initialization
	void Awake () 
    {
        trans_ref = transform;
	}

    void Start( )
    {

        previous_position = trans_ref.position;
        target_position = turntable_generator.GetGenerationPoint( ) + trans_ref.position;
        simple_collider.AddPoint( target_position );
        
    }

	// Update is called once per frame
	void Update ()
    {

        if( Vector3.Distance( trans_ref.position, previous_position ) > 2 )
        {

            target_position = turntable_generator.GetGenerationPoint( ) + trans_ref.position;
            simple_collider.AddPoint( target_position );
            previous_position = trans_ref.position;
        }

        //trans_ref.position = Vector3.MoveTowards( trans_ref.position, target_position, speed * Time.deltaTime );
	}

    /////////////////////////////////////////////
}
