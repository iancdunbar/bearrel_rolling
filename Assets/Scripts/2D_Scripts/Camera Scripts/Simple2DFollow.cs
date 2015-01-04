using UnityEngine;
using System.Collections;

public class Simple2DFollow : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    Transform trans_ref;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    Transform target;
    [SerializeField]
    Vector2 offset;

    /////////////////////////////////////////////



	// Use this for initialization
	void Awake () 
    {
        trans_ref = this.transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
        trans_ref.position = new Vector3( target.position.x + offset.x, target.position.y + offset.y, trans_ref.position.z );
	}
}
