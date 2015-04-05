using UnityEngine;
using System.Collections;

public class TerrainSegment : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variables
    /////////////////////////////////////////////

    private EdgeCollider2D collider_ref;
    [SerializeField, HideInInspector]
    private Vector3 start_point;
    [SerializeField, HideInInspector]
    private Vector3 end_point;
    [SerializeField, HideInInspector]
    private bool points_init;

    /////////////////////////////////////////////

    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private MeshFilter collisionMesh;
    [SerializeField]
    private bool getStartEndFromColors;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Private Functions
    /////////////////////////////////////////////



    /////////////////////////////////////////////



    /////////////////////////////////////////////
    // Public Functions
    /////////////////////////////////////////////

    public MeshFilter CollisionMesh { get { return collisionMesh; } }

    public Vector3 StartPoint { get { if( getStartEndFromColors && points_init ) return transform.position + start_point; else return transform.FindChild( "Start" ).position; } }
    public Vector3 EndPoint { get { if( getStartEndFromColors && points_init ) return transform.position + end_point; else return transform.FindChild( "End" ).position; } }

    public bool PointsFromColor { get { return getStartEndFromColors; } }

    public void InitStartEndPoints( Vector3 start, Vector3 end )
    {
        start_point = start;
        end_point = end;

        points_init = true;
        getStartEndFromColors = true;
    }

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

    void Awake( )
    {
        collider_ref = GetComponent<EdgeCollider2D>( );
    }

    /////////////////////////////////////////////
}
