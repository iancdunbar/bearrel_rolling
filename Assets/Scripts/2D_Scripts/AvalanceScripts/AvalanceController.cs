using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvalanceController : MonoBehaviour {


    ///////////////////////////////////////////////
    // Private Variables
    ///////////////////////////////////////////////

    private static AvalanceController instance;
    public static AvalanceController Instance { get { return instance; } }
    
    private Queue<Vector3> path;


    ///////////////////////////////////////////////


    ///////////////////////////////////////////////
    // Inspector Variables
    ///////////////////////////////////////////////

    [SerializeField]
    private float avalance_speed;
	public ParticleSystem snow;
	public bool can_blood = true;

    ///////////////////////////////////////////////


    ///////////////////////////////////////////////
    // Public Functions
    ///////////////////////////////////////////////

    public static void AddPoint( Vector3 tgt )
    {
        instance.path.Enqueue( tgt );
    }


    ///////////////////////////////////////////////


    ///////////////////////////////////////////////
    // Unity Messages
    ///////////////////////////////////////////////

    void Awake( )
    {
        if( instance == null )
            instance = this;
        else
            DestroyImmediate( this.gameObject );

        path = new Queue<Vector3>( );
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if( path.Count == 0 ) return;
        Vector3 target = path.Peek( );

        transform.position = Vector3.MoveTowards( transform.position, target, avalance_speed * Time.deltaTime );

        if( Vector3.Distance( transform.position, target ) < 0.05f )
            path.Dequeue( );

	}







    ///////////////////////////////////////////////

}
