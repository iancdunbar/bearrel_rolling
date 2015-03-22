using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private float min_angle = 10;
    [SerializeField]
    private float max_angle = 90;

	[SerializeField]
	private SimpleColliderGenerator collider_gen;
	[SerializeField]
	private Transform bear;

	//Rock embellishment variables
	[SerializeField]
	private GameObject rock_embellishment_prefab;
	
	[SerializeField]
	private int rock_embellishment_perentage;
	
	[SerializeField]
	private Texture2D rock_embellishment_texture; //texture where we keep all the rock decoration sprites

	//tree variables
	[SerializeField]
	private GameObject tree_prefab;

	[SerializeField]
	private int chanceOfTrees; //code generates between minTreesPerSegment and maxTreesPerSegment at random.

	[SerializeField]
	private int minTreesPerSegment; //code generates between minTreesPerSegment and maxTreesPerSegment at random.
	
	[SerializeField]
	private int maxTreesPerSegment;
	
	[SerializeField]
	private Texture2D treeTexture1; //currently we have 3 seperate textures for tree sprites, so thats why there are 3 here. May want to consolidate into one sheet.
	
	[SerializeField]
	private Texture2D treeTexture2;
	
	[SerializeField]
	private Texture2D treeTexture3; 

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
		GameObject terrain_piece_container = new GameObject (); //create container to hold all terrain obects

        GameObject piece = (GameObject)Instantiate( terrain_piece );
        Transform trans_ref = piece.transform;

		piece.transform.parent = terrain_piece_container.transform; //assign terrain as a child of the terrain container

        trans_ref.eulerAngles = new Vector3( trans_ref.eulerAngles.x, trans_ref.eulerAngles.y, -(min_angle) + Random.RandomRange( -1f, 0f ) * (max_angle - min_angle) );  

        trans_ref.position = start_position - trans_ref.FindChild("Start").position;

        next_point = trans_ref.FindChild( "End" ).position;

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
		generateTrees(start_position, next_point, terrain_piece_container);


    }

	private void generateMountainRocks(Vector3 EndOfLastTerrainPiece, GameObject currentPiece_Container){
		
		//use embellishment percentage to determine whether to actually add one or not
		float random = Random.Range (0f, 100f);
		
		if (random < rock_embellishment_perentage) {
			return;
		} 
		
		GameObject rock_embellishment_obj = (GameObject)Instantiate( rock_embellishment_prefab );
		Transform trans_ref = rock_embellishment_obj.transform;	

		rock_embellishment_obj.transform.parent = currentPiece_Container.transform;
		
		string assetPath = AssetDatabase.GetAssetPath (rock_embellishment_texture);
		
		Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath (assetPath).OfType<Sprite>().ToArray();
		
		Sprite spriteToDraw = sprites[Random.Range (0, sprites.Length)];
		
		//we've chosen the sprite to draw on the mountain. Now we use it's length to determine where to draw it. 
		//We must draw it at <= (EndOfLastTerrainPiece.X - spriteWidth). Probably add some random padding to that so embellishments dont end right where 
		//terrain pieces do.
		
		trans_ref.position = new Vector3 (EndOfLastTerrainPiece.x - 10f, EndOfLastTerrainPiece.y - 3f, EndOfLastTerrainPiece.z);//EndOfLastTerrainPiece;
		
		rock_embellishment_obj.GetComponent<SpriteRenderer>().sprite = spriteToDraw;

		active_rock_embellishments.Enqueue (rock_embellishment_obj);
		
	}

	private void generateTrees(Vector3 startPoint, Vector3 endPoint, GameObject currentPiece_container){

		//generate x random points between end and start

		int numberOfTrees = 0;

		float random = Random.Range (0f, 100f);
		
		if (random < chanceOfTrees) {
			numberOfTrees = Random.Range (minTreesPerSegment, maxTreesPerSegment);
		} 

		float lineSlope = (endPoint.y - startPoint.y) / (endPoint.x - startPoint.x); // M value in y=mx+b

		float lineOffset = startPoint.y - (lineSlope * startPoint.x);// b value in y=mx+b

		//now we have a function which represents the slope of the hill. lets get 3 random x points and find the cooresponding y. 

		for(int x = 0; x < numberOfTrees; x++ ){
			//instantiate trees
			GameObject tree_obj = (GameObject)Instantiate( tree_prefab );
			Transform tree_trans = tree_obj.transform;	

			tree_obj.transform.parent = currentPiece_container.transform;

			string assetPath = AssetDatabase.GetAssetPath (treeTexture1); //update later to use texture files 2 and 3

			int treeTextureNumber = Random.Range (1,3);

			switch(treeTextureNumber)
			{
			case 2:
				assetPath = AssetDatabase.GetAssetPath (treeTexture2);
				break;
			case 3:
				assetPath = AssetDatabase.GetAssetPath (treeTexture3); 
				break;
			}

			Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath (assetPath).OfType<Sprite>().ToArray();

			Sprite spriteToDraw = sprites[Random.Range (0, sprites.Length)]; 

			//generate random x along segment for this tree. Find corresponding y:

			float tree_base_x = Random.Range (startPoint.x, endPoint.x);

			tree_obj.GetComponent<SpriteRenderer>().sprite = spriteToDraw;


			float tree_height = tree_obj.transform.localScale.y * tree_obj.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

			float tree_base_y = tree_base_x*lineSlope+lineOffset-(tree_height/2)-treeDrawingOffset;

			tree_trans.position = new Vector3(tree_base_x, tree_base_y + tree_height, -2f);

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

			if( active_terrain_segment_containers.Count > 30 )
			{
				Destroy( active_terrain_segment_containers.Dequeue( ) );
				//Destroy( active_masks.Dequeue( ) );
			}

        }
	}

    /////////////////////////////////////////////
}
