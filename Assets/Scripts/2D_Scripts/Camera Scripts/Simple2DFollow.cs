using UnityEngine;
using System.Collections;

public class Simple2DFollow : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    private Transform trans_ref;
    private Vector3 previous_position;
    private Vector3 position_delta;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private Transform follow_target;
    [SerializeField]
    private Vector3 follow_amount;

    /////////////////////////////////////////////



	// Use this for initialization
	void Awake () 
    {
        trans_ref = this.transform;
	}

    void Start( )
    {
        // Initialize the previous position
        previous_position = follow_target.position;

    }

	// Update is called once per frame
	void Update () 
    {
        // Calculate the change in position of the target object
        position_delta = follow_target.position - previous_position;

        // Add the position delta scaled by the follow amount to this transform's position
        trans_ref.position = trans_ref.position + Vector3.Scale( position_delta, follow_amount );

        // Capture the current target position for use in the next frame
        previous_position = follow_target.position;


		}


}
