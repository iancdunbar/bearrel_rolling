using UnityEngine;
using System.Collections;

public class CameraResetController : MonoBehaviour {

    //////////////////////////////////////
    // Private Variables
    //////////////////////////////////////

    private Transform transRef;
    private Vector3 defaultPos;
    private Quaternion defaultAngles;

    //////////////////////////////////////

    //////////////////////////////////////
    // GUI Messages
    //////////////////////////////////////

    private void OnReset( )
    {
        transRef.position = defaultPos;
        transRef.rotation = defaultAngles;
    }



    //////////////////////////////////////
    // Unity Messages
    //////////////////////////////////////

    // Use this for initialization
    void Awake( )
    {
        transRef = this.transform;
    }

    void Start( )
    {
        defaultPos = transRef.position;
        defaultAngles = transRef.rotation;
    }

    //////////////////////////////////////
}
