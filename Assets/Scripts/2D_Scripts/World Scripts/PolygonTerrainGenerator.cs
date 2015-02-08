﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonTerrainGenerator : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    private Queue<GameObject> active_terrain_segments;
    private Vector3 next_point;
    private Vector3 save_point;
    private float delta_x;
    private Mesh simple_quad;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////
    [SerializeField]
    private float quad_width;
    [SerializeField]
    private float quad_height;
    [SerializeField]
    private Material terrain_material;
    [SerializeField]
    private SimpleColliderGenerator collider_gen;
    [SerializeField]
    private Transform bear;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Private Fuctions
    /////////////////////////////////////////////

    private Mesh create_simple_quad( float w, float h )
    {
        Mesh mesh = new Mesh( );

        Vector3[] vertices = new Vector3[ ]
        {
            new Vector3( 0, 0, 0),
            new Vector3( w, 0, 0),
            new Vector3( w, -h, 0),
            new Vector3( 0, -h, 0)
            //new Vector3( w, 0,  h),
            //new Vector3( w, 0, -h),
            //new Vector3(-w, 0,  h),
            //new Vector3(-w, 0, -h),
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

    private void generate_next_quad( Vector3 start_position )
    {
        GameObject go = new GameObject( );
        go.name = "Terrain_Piece";

        Transform trans_ref = go.transform;
        MeshFilter mf = go.AddComponent<MeshFilter>( );
        MeshRenderer mr = go.AddComponent<MeshRenderer>( );

        trans_ref.eulerAngles = new Vector3( 0, 0, Random.RandomRange( -1f, 0 ) * 75 );  

        mf.mesh = simple_quad;
        mr.material = terrain_material;

        trans_ref.position = start_position;

        next_point = trans_ref.TransformPoint( simple_quad.vertices[ 1 ] );

        delta_x = next_point.x - start_position.x;
        save_point = bear.position;

        active_terrain_segments.Enqueue( go );

    }

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

    void Awake( )
    {
        active_terrain_segments = new Queue<GameObject>( );
        simple_quad = create_simple_quad( quad_width, quad_height );

    }

	// Use this for initialization
	void Start () 
    {

        next_point = new Vector3( 0, 0, -1 );
        for( int i = 0; i < 10; i++ )
        {
            generate_next_quad( next_point );
            collider_gen.AddPoint( next_point );
        }


	}

    // Update is called once per frame
    void Update( ) 
    {
        if( bear.position.x - save_point.x > delta_x )
        {
            generate_next_quad( next_point );
            collider_gen.AddPoint( next_point );

            if( active_terrain_segments.Count > 20 )
            {
                Destroy( active_terrain_segments.Dequeue( ) );
            }
        }
	}

    /////////////////////////////////////////////
}