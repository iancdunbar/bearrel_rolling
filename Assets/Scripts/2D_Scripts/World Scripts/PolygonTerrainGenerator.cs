using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonTerrainGenerator : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    private Queue<GameObject> active_terrain_segments;
    private Queue<GameObject> active_masks;
    private Vector3 next_point;
    private Vector3 save_point;
    private float delta_x;
    private Mesh simple_quad;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private GameObject terrain_piece;
    [SerializeField]
    private int end_vert;
    [SerializeField]
    private GameObject mask_prefab;

    [SerializeField]
    private float gen_distance;
    
    [SerializeField]
    private SimpleColliderGenerator collider_gen;
    [SerializeField]
    private Transform bear;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Private Fuctions
    /////////////////////////////////////////////

    private Mesh create_mask( Vector3 start, Vector3 end )
    {
        Mesh mesh = new Mesh( );


        float w = end.x - start.x;
        float h = 20;
        float end_off_y = end.y - start.y;
        
        Vector3[] verts = new Vector3[ ]
        {

            new Vector3( 0, 0, 0 ),
            new Vector3( 0, h, 0 ),
            new Vector3( w, h, 0 ),
            new Vector3( w, end_off_y, 0 )

        };

        Vector2[] uv = new Vector2[ ]
        {
            new Vector2(0, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
        };

        int[] triangles = new int[ ]
        {
            0, 1, 2,
            2, 3, 0
        };

        Vector3[] normals = new Vector3[ ]
        {
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
        };

        mesh.vertices = verts;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }

    private Mesh create_simple_quad( float w, float h )
    {
        Mesh mesh = new Mesh( );

        Vector3[] vertices = new Vector3[ ]
        {
            new Vector3( 0, 0, 0),
            new Vector3( w, 0, 0),
            new Vector3( w, -h, 0),
            new Vector3( 0, -h, 0)
        };

        Vector2[] uv = new Vector2[ ]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
        };

        int[] triangles = new int[ ]
        {
            0, 1, 2,
            2, 3, 0
        };

        Vector3[] normals = new Vector3[ ]
        {
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
        };

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }

    private void generate_next_piece( Vector3 start_position )
    {

        GameObject piece = (GameObject)Instantiate( terrain_piece );
        Transform trans_ref = piece.transform;

        trans_ref.eulerAngles = new Vector3( trans_ref.eulerAngles.x, trans_ref.eulerAngles.y, Random.RandomRange( -1f, 0f ) * 90 );  

        trans_ref.position = start_position - trans_ref.FindChild("Start").position;

        next_point = trans_ref.FindChild( "End" ).position;// TransformPoint( piece_mesh.vertices[ end_vert ] );

        GameObject mask_obj = (GameObject)Instantiate( mask_prefab );
        mask_obj.GetComponent<MeshFilter>( ).mesh = create_mask( start_position, next_point );

        mask_obj.transform.position = start_position;

        delta_x = next_point.x - start_position.x;

        save_point = bear.position;

        active_terrain_segments.Enqueue( piece );
        active_masks.Enqueue( mask_obj );

    }

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

    void Awake( )
    {
        active_terrain_segments = new Queue<GameObject>( );
        active_masks = new Queue<GameObject>( );

        simple_quad = create_simple_quad( 3.3f, 1 );

    }

	// Use this for initialization
	void Start () 
    {

        next_point = new Vector3( 0, 0, -1 );
        for( int i = 0; i < 10; i++ )
        {
            generate_next_piece( next_point );
            collider_gen.AddPoint( next_point );
        }


	}

    // Update is called once per frame
    void Update( ) 
    {
        if( next_point.x - bear.position.x < gen_distance )
        //if( Vector3.Distance(bear.position, next_point) < gen_distance )
        {
            generate_next_piece( next_point );
            collider_gen.AddPoint( next_point );

            if( active_terrain_segments.Count > 20 )
            {
                Destroy( active_terrain_segments.Dequeue( ) );
                Destroy( active_masks.Dequeue( ) );
            }
        }
	}

    /////////////////////////////////////////////
}
