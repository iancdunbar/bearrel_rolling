using UnityEngine;
using System.Collections;

public class Turntable : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variables
    /////////////////////////////////////////////

    private Transform trans_ref;

    private float initial_angle = 0;
    private float saved_current_angle = 0;

    private bool previous_mouse = false;
    private bool dragging = false;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private float generation_angle;
    [SerializeField]
    private float generation_radius;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Public Access
    /////////////////////////////////////////////

    public float Generation_Angle { get { return generation_angle; } set { generation_angle = value; } }

    public Vector3 GetGenerationPoint( )
    {
       return new Vector3( Mathf.Cos( generation_angle ), Mathf.Sin( generation_angle ), 0 ) * generation_radius;
    }

    /////////////////////////////////////////////



    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

    void Awake( )
    {
        trans_ref = transform;
    }

	// Use this for initialization
	void Start () 
    {
        previous_mouse = Input.GetMouseButton( 0 );
        saved_current_angle = generation_angle;
	}
	
	// Update is called once per frame
	void Update () 
    {
        bool current_mouse = Input.GetMouseButton( 0 );
        
        if( previous_mouse != current_mouse )
        {
            // If the previous state was Down
            if( previous_mouse )
            {
                // Then the mouse has been released
                dragging = false;
            }
            else
            {
                // Otherwise the mouse has been pressed
                // Calculate the initial angle
                Vector3 target_pos = Camera.main.ScreenToWorldPoint( Input.mousePosition );

                Vector3 tgt = target_pos - trans_ref.position;

                //float angle = Vector3.Angle( new Vector3( 1, 0, 0 ), tgt );

                initial_angle = Mathf.Atan2( tgt.y, tgt.x );

                saved_current_angle = generation_angle;

                // Set the dragging value to true
                dragging = true;
            }
        }

        if( dragging )
        {
            Vector3 target_pos = Camera.main.ScreenToWorldPoint( Input.mousePosition );

            Vector3 tgt = target_pos - trans_ref.position;

            generation_angle = saved_current_angle - (initial_angle - Mathf.Atan2( tgt.y, tgt.x ));
                
        }

        previous_mouse = current_mouse;
	}

    void OnDrawGizmos( )
    {
        if( trans_ref == null )
            trans_ref = transform;

        Vector3 target_pos = GetGenerationPoint( );

        Gizmos.DrawLine( transform.position, transform.position + target_pos );
    }

    /////////////////////////////////////////////
}
