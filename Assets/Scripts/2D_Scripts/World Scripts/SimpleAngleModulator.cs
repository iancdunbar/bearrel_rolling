using UnityEngine;
using System.Collections;


public class SimpleAngleModulator : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variables
    /////////////////////////////////////////////

    private Turntable modulation_target;
    private float initial_angle;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private float modulation_angle;
    [SerializeField]
    private float frequency;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

	// Use this for initialization
	void Awake () 
    {
        modulation_target = GetComponent<Turntable>( );
        initial_angle = modulation_target.Generation_Angle;

	}
	
	// Update is called once per frame
	void Update () 
    {
        modulation_target.Generation_Angle = initial_angle + Mathf.Sin( Time.realtimeSinceStartup * 3.14f * frequency) * Mathf.Deg2Rad * modulation_angle;
	}
}
