using UnityEngine;
using System.Collections;

public class BearInputController : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    private Vector2 touch_begin;
    private bool gesture_processed;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private float swipe_minimum;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    // Use this for initialization
    void Start( )
    {

    }

    // Update is called once per frame
    void Update( )
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        // Process Touch Input
        if( Input.touchCount > 0 )
        {
            Touch curr = Input.GetTouch( 0 );

            if( curr.phase == TouchPhase.Began )
            {
                gesture_processed = false;
                touch_begin = curr.position;
            }
            else if( curr.phase == TouchPhase.Moved )
            {
                if( !gesture_processed && curr.position.y - touch_begin.y > swipe_minimum )
                {
                    gesture_processed = true;
                    MessageDispatch.BroadcastMessage( "OnSwipeUp" );
                }
                else if( !gesture_processed && touch_begin.y - curr.position.y > swipe_minimum )
                {
                    gesture_processed = true;
                    MessageDispatch.BroadcastMessage( "OnSwipeDown" );
                }
            }
            else if( curr.phase == TouchPhase.Ended )
            {
                
            }
        }
#else
        if( Input.GetKeyUp( KeyCode.Space ) )
        {
            MessageDispatch.BroadcastMessage( "OnSwipeUp" );
        }

        if( Input.GetKeyUp( KeyCode.LeftShift ) )
        {
            MessageDispatch.BroadcastMessage( "OnSwipeDown" );
        }
		if (Input.GetKeyDown( KeyCode.D ) )
		{
			MessageDispatch.BroadcastMessage( "OnTap");
		}

#endif
    }

    /////////////////////////////////////////////
}
