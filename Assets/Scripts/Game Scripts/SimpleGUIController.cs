using UnityEngine;
using System.Collections;

public class SimpleGUIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnGUI( )
    {
        GUI.skin.textArea.alignment = TextAnchor.MiddleCenter;
        GUI.TextArea( new Rect( 400, 10, 230, 40 ), "Press UP to get things rolling!" );

        if( GUI.Button( new Rect( 10, 10, 115, 30 ), "Reset" ) )
        {
            BroadcastMessage( "OnReset" );
        }
    }
}
