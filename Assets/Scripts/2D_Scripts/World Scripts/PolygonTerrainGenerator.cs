﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using System.Linq;

public class PolygonTerrainGenerator : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

	private Queue<GameObject> active_terrain_segment_containers;
    private Queue<GameObject> active_terrain_segments;
	private Queue<GameObject> active_rock_embellishments;
	private Queue<GameObject> active_trees;
    private Queue<GameObject> active_masks;
    private Vector3 next_point;
    private Vector3 save_point;
    private float delta_x;
    private Mesh simple_quad;

	private float treeDrawingOffset = 0.2f;

	//Asset Paths from Resource Folder

	//**Rocks
	private const string rock_embellishment_prefab_path = "Assets/Resources/Terrain Assets/rock_embellishments/rock_embelishment.prefab";
	private const string rock_embellishment_sprites_path = "Terrain Assets/rock_embellishments/sprites";
	//**Trees
	private const string tree_prefab_path = "Assets/Resources/Terrain Assets/trees/tree_prefab.prefab";
	private const string tree_sprites_path = "Terrain Assets/trees/sprites";

    /////////////////////////////////////////////
	/// 
	/// 

	//Private Assets 
	private GameObject rock_embellishment_prefab;// = Resources.LoadAssetAtPath<GameObject>(rock_embellishment_prefab_path);

	private GameObject tree_prefab; // = Resources.LoadAssetAtPath<GameObject>(tree_prefab_path);
	private Texture2D treeTexture1;// = Resources.LoadAssetAtPath<Texture2D>(tree_sprites_1_path);
	//private Texture2D treeTexture2 = Resources.LoadAssetAtPath<Texture2D>(tree_sprites_2_path);
	//private Texture2D treeTexture3 = Resources.LoadAssetAtPath<Texture2D>(tree_sprites_3_path);

    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private GameObject[] terrain_piece;
	private GameObject getTerrain() {
		return terrain_piece [Random.Range(0, terrain_piece.Length )];
	}
    [SerializeField]
    private int end_vert;
    [SerializeField]
    private GameObject mask_prefab;
	[SerializeField]
	private float gen_distance;
    [SerializeField]
    private float min_angle = 10;
    [SerializeField]
    private float max_angle = 90;

	[SerializeField]
	private SimpleColliderGenerator collider_gen;
	[SerializeField]
	private Transform bear;

	//Rock embellishment variables
	
	[SerializeField]
	private int rock_embellishment_perentage;

	//tree variables
	[SerializeField]
	private int chanceOfTrees; //code generates between minTreesPerSegment and maxTreesPerSegment at random.

	[SerializeField]
	private int minTreesPerSegment; //code generates between minTreesPerSegment and maxTreesPerSegment at random.
	
	[SerializeField]
	private int maxTreesPerSegment;


    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Private Fuctions
    /////////////////////////////////////////////

    private Mesh create_mask( Vector3 start, Vector3 end )
    {
        Mesh mesh = new Mesh( );


        float w = end.x - start.x;
        float h = 100;
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

    private void generate_next_piece( Vector3 start_position )
    {	
        GameObject piece = Instantiate( getTerrain () ) as GameObject;

        Transform trans_ref = piece.transform;
        TerrainSegment ts   = piece.GetComponent<TerrainSegment>( );

		GameObject terrain_piece_container = new GameObject();

		piece.transform.parent = terrain_piece_container.transform; //assign terrain as a child of the terrain container

        trans_ref.eulerAngles = new Vector3( trans_ref.eulerAngles.x, trans_ref.eulerAngles.y, -(min_angle) + Random.RandomRange( -1f, 0f ) * (max_angle - min_angle) );  

        trans_ref.position = start_position - ts.StartPoint;

        next_point = ts.EndPoint;

        AvalanceController.AddPoint( next_point );

        GameObject mask_obj = (GameObject)Instantiate( mask_prefab );
        mask_obj.GetComponent<MeshFilter>( ).mesh = create_mask( start_position, next_point );
        mask_obj.transform.position = start_position;

        delta_x = next_point.x - start_position.x;

        save_point = bear.position;

		active_terrain_segment_containers.Enqueue (terrain_piece_container);


        active_terrain_segments.Enqueue( piece );
        active_masks.Enqueue( mask_obj );

		generateMountainRocks(next_point, terrain_piece_container);
		generateTrees(start_position, next_point, terrain_piece_container, ts);


    }

	private void generateMountainRocks(Vector3 EndOfLastTerrainPiece, GameObject currentPiece_Container){
		
		//use embellishment percentage to determine whether to actually add one or not
		float random = Random.Range (0f, 100f);
		
		if (random < rock_embellishment_perentage) {
			return;
		} 

		//rock_embellishment_prefab = Resources.LoadAssetAtPath<GameObject>(rock_embellishment_prefab_path);

		GameObject rock_embellishment_obj = Instantiate(Resources.LoadAssetAtPath(rock_embellishment_prefab_path, typeof(GameObject))) as GameObject;

		Transform trans_ref = rock_embellishment_obj.transform;	

		rock_embellishment_obj.transform.parent = currentPiece_Container.transform;

		Sprite[] spritesSingle = Resources.LoadAll<Sprite> (rock_embellishment_sprites_path);
	
		Sprite[] sprites = new Sprite[3];
		Sprite spriteToDraw = sprites[Random.Range (0, sprites.Length)];
		
		//we've chosen the sprite to draw on the mountain. Now we use it's length to determine where to draw it. 
		//We must draw it at <= (EndOfLastTerrainPiece.X - spriteWidth). Probably add some random padding to that so embellishments dont end right where 
		//terrain pieces do.
		
		trans_ref.position = new Vector3 (EndOfLastTerrainPiece.x - 10f, EndOfLastTerrainPiece.y - 3f, EndOfLastTerrainPiece.z);//EndOfLastTerrainPiece;
		
		rock_embellishment_obj.GetComponent<SpriteRenderer>().sprite = spriteToDraw;

		active_rock_embellishments.Enqueue (rock_embellishment_obj);
		
	}

	private void generateTrees(Vector3 startPoint, Vector3 endPoint, GameObject currentPiece_container, TerrainSegment currentSegment){

		//generate x random points between end and start

		int numberOfTrees = 0;

		float random = Random.Range (0f, 100f);
		
		if (random < chanceOfTrees) {
			numberOfTrees = Random.Range (minTreesPerSegment, maxTreesPerSegment);
		} 

		//now we have a function which represents the slope of the hill. lets get 3 random x points and find the cooresponding y. 
		Vector2[] colliderPoints = currentSegment.EdgeColliderPoints;

		for(int x = 0; x < numberOfTrees; x++ ){
			//instantiate trees
			tree_prefab = Resources.LoadAssetAtPath<GameObject>(tree_prefab_path);

			GameObject tree_obj = (GameObject)Instantiate( tree_prefab );
			Transform tree_trans = tree_obj.transform;	

			tree_obj.transform.parent = currentPiece_container.transform;

			Sprite[] sprites = Resources.LoadAll<Sprite> (tree_sprites_path);

			Sprite spriteToDraw = sprites[Random.Range (0, sprites.Length)]; 


		
			//get two points in the collider of the currentPiece with x values on either side of tree_base_x

			Vector2 lowerPoint = Vector2.zero;
			Vector2 upperPoint = Vector2.zero;

			//generate random x along segment for this tree. Find corresponding y:
			
			float tree_base_x = Random.Range (colliderPoints[0].x, colliderPoints[colliderPoints.Length-1].x);

			foreach (Vector2 point in colliderPoints)
			{

				int indexOfCurrentPoint = System.Array.IndexOf(colliderPoints, point);

				if(indexOfCurrentPoint == colliderPoints.Length-1){
					break;
				}
				if(point.x < tree_base_x && colliderPoints[indexOfCurrentPoint + 1].x > tree_base_x)
				{
					lowerPoint = point;
					upperPoint = colliderPoints[indexOfCurrentPoint + 1];
					break;
				}


			}

			float localSlope = (upperPoint.y - lowerPoint.y) / (upperPoint.x - lowerPoint.x); // M value in y=mx+b
			float lineOffset = lowerPoint.y - (localSlope * lowerPoint.x); // b value in y=mx+b

			tree_obj.GetComponent<SpriteRenderer>().sprite = spriteToDraw;


			float tree_height = tree_obj.transform.localScale.y * tree_obj.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

			float tree_base_y = tree_base_x*localSlope+lineOffset-(tree_height/2)-treeDrawingOffset;

			tree_trans.position = new Vector3(tree_base_x, tree_base_y + tree_height, -1.01f);

			active_trees.Enqueue(tree_obj);

		} 
	
	}
    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

    void Awake( )
    {
		active_terrain_segment_containers = new Queue<GameObject>( );
        active_terrain_segments = new Queue<GameObject>( );
        active_masks = new Queue<GameObject>( );
		active_rock_embellishments = new Queue<GameObject> ();
		active_trees = new Queue<GameObject> ();
    }

	// Use this for initialization
	void Start () 
    {

        next_point = new Vector3( 0, 0, 0 );
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

			if( active_terrain_segment_containers.Count > 30 )
			{
				Destroy( active_terrain_segment_containers.Dequeue( ) );
				//Destroy( active_masks.Dequeue( ) );
			}

        }
	}

    /////////////////////////////////////////////
}
