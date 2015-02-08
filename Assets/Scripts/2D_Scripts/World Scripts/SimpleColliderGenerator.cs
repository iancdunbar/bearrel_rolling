using UnityEngine;
using System.Collections;

[RequireComponent( typeof( EdgeCollider2D ) )]
public class SimpleColliderGenerator : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variables
    /////////////////////////////////////////////

    private EdgeCollider2D collider_ref;

    private float spawn_x;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private Transform bear;

    [SerializeField]
    private float target_angle;
    [SerializeField]
    private float angle_deviation;
    [SerializeField]
    private float target_length;
    [SerializeField]
    private float length_deviation;


    /////////////////////////////////////////////

    /////////////////////////////////////////////
    // Public Methods
    /////////////////////////////////////////////

    void GenerateNewPoints( int num_points )
    {

        

        Vector3 target_pos = Camera.main.ScreenToWorldPoint( Input.mousePosition );

        Vector2 point = new Vector2( collider_ref.transform.position.x, collider_ref.transform.position.y ) + collider_ref.points[ collider_ref.pointCount - 1 ];

        Vector3 tgt = target_pos - new Vector3( point.x, point.y, target_pos.z );

        float angle = Vector3.Angle( new Vector3( 1, 0, 0 ), tgt );

        angle = Mathf.Atan2( tgt.y, tgt.x );

        //Debug.Log( angle );

        for( int i = 0; i < num_points; i++ )
        {
            //float angle = Random.Range( -1f, 1f );
            //angle *= angle_deviation;

            //angle += target_angle;

            float length = Random.Range( -1f, 1f );
            length *= length_deviation;

            length += target_length;

            GenerateNewPoint( angle, target_length );

        }

    }

    void GenerateNewPoint( float angle, float length )
    {
        spawn_x = collider_ref.transform.position.x + collider_ref.points[ collider_ref.pointCount - 1 ].x;

        Vector2[] points = collider_ref.points;

        Vector2 point = points[ points.Length - 1 ] + new Vector2( Mathf.Cos( angle ) * length, Mathf.Sin(  angle ) * length );
        
        System.Array.Resize<Vector2>( ref points, points.Length + 1 );

        points[ points.Length - 1 ] = point;

        collider_ref.points = points;
    }

    public void AddPoint( Vector3 target_pos )
    {
        //Vector2 point = new Vector2( collider_ref.transform.position.x, collider_ref.transform.position.y ) + collider_ref.points[ collider_ref.pointCount - 1 ];

        Vector3 offset = target_pos - transform.position;

        Vector2[] points = collider_ref.points;

        Vector2 point = new Vector2( offset.x, offset.y );

        System.Array.Resize<Vector2>( ref points, points.Length + 1 );

        points[ points.Length - 1 ] = point;

        collider_ref.points = points;

    }

    /////////////////////////////////////////////




    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

    // Use this for initialization
    void Awake( ) 
    {
        collider_ref = this.GetComponent<EdgeCollider2D>( );

	}

    void Start( )
    {
        
    }
	
	// Update is called once per frame
	void Update () 
    {

	}

    //void OnDrawGizmos( )
    //{
    //    Vector3 target_pos = Camera.main.ScreenToWorldPoint( Input.mousePosition );

    //    if( collider_ref != null )
    //    {
    //        Vector2 point = new Vector2( collider_ref.transform.position.x, collider_ref.transform.position.y ) + collider_ref.points[ collider_ref.pointCount - 1 ];

    //        Gizmos.DrawLine( new Vector3( point.x, point.y, 0 ), target_pos );
    //    }
    //}

    /////////////////////////////////////////////
}
