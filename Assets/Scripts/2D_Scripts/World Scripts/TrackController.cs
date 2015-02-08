using UnityEngine;
using System.Collections;

public class TrackController : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variables
    /////////////////////////////////////////////
    
    private Transform trans_ref;
    private Vector3 previous_postion;
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
        target_position = turntable_generator.GetGenerationPoint( );
        simple_collider.AddPoint( target_position );
        previous_postion = trans_ref.position;
    }

	// Update is called once per frame
	void Update ()
    {
        if( Vector3.Magnitude( trans_ref.position - previous_postion ) > 2 )
        {
            target_position = turntable_generator.GetGenerationPoint( );
            simple_collider.AddPoint( target_position );
            previous_postion = trans_ref.position;
        }

        Vector3.MoveTowards( trans_ref.position, target_position, speed * Time.deltaTime );
	}

    /////////////////////////////////////////////
}
