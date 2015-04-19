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

    public Vector3 StartPoint { get { if( getStartEndFromColors && points_init ) return transform.position + Vector3.Scale(start_point, transform.localScale); else return transform.FindChild( "Start" ).position; } }
    public Vector3 EndPoint { get { if( getStartEndFromColors && points_init ) return transform.position + Vector3.Scale( end_point, transform.localScale ); else return transform.FindChild( "End" ).position; } }
	public Vector2[] EdgeColliderPoints { get {return scaledCollider(transform.localScale.x, transform.localScale.y);} }
    public bool PointsFromColor { get { return getStartEndFromColors; } }

    public void InitStartEndPoints( Vector3 start, Vector3 end )
    {
        start_point = start;
        end_point = end;

        points_init = true;
        getStartEndFromColors = true;

    }

	public Vector2[] scaledCollider(float localScaleX, float localScaleY)
	{
		Vector2[] scalledCollider = new Vector2[collider_ref.points.Length];
		Vector2 segmentPosXY = new Vector2 (transform.position.x, transform.position.y);
		Vector2 Vector2LocalScale = new Vector2 (localScaleX, localScaleY); 
		for (int i = 0; i < collider_ref.points.Length; i++) {
			scalledCollider[i] = segmentPosXY + Vector2.Scale(collider_ref.points[i], Vector2LocalScale);
		}

		return scalledCollider;
	}

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////
	void OnEnable()
	{
		gameObject.tag = "Ground";
	}
    void Awake( )
    {
        collider_ref = GetComponent<EdgeCollider2D>( );
		gameObject.tag = "Ground";
    }

    /////////////////////////////////////////////
}
