using UnityEngine;
using System.Collections;

public class FollowParallax : MonoBehaviour {

    ////////////////////////////////////////////
    // Private Variables
    ////////////////////////////////////////////

    private Vector3 initial_position;
    private Vector3 previous_target_position;
    private Vector2 dimensions;

    ////////////////////////////////////////////


    ////////////////////////////////////////////
    // Inspector Variables
    ////////////////////////////////////////////

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 follow_amount;

    ////////////////////////////////////////////


    ////////////////////////////////////////////
    // Private Functions
    ////////////////////////////////////////////

    private Vector2 GetDimensionInPX( GameObject obj )
    {
        Vector2 tmpDimension;

        Vector3 size =  obj.GetComponent<SpriteRenderer>( ).sprite.bounds.size;

        tmpDimension.x = obj.transform.localScale.x * size.x;  // this is gonna be our width
        tmpDimension.y = obj.transform.localScale.y * size.y;  // this is gonna be our height

        return tmpDimension;
    }

    ////////////////////////////////////////////


    ////////////////////////////////////////////
    // Unity Messages
    ////////////////////////////////////////////

    void Awake( )
    {
        dimensions = GetDimensionInPX( gameObject );
        initial_position = transform.localPosition;
        previous_target_position = target.position;
    }

	// Use this for initialization
	void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () 
    {

        // HARD CODE
        if( transform.localPosition.x < (initial_position.x - dimensions.x) )
            transform.localPosition = initial_position;

        transform.Translate( Vector3.Scale( ( target.position - previous_target_position ), follow_amount ) );

        previous_target_position = target.position;

        
	}

    ////////////////////////////////////////////
}
