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
	private float touchDuration;
	private Touch touch;

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

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
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
		if(Input.touchCount > 0){ //if there is any touch
			touchDuration += Time.deltaTime;
			touch = Input.GetTouch(0);
			
			if(touch.phase == TouchPhase.Ended && touchDuration < 0.3f) //making sure it only check the touch once && it was a short touch/tap and not a dragging.
				StartCoroutine("singleOrDouble");
		}
		else
			touchDuration = 0.0f;
			
#else
		
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
            MessageDispatch.BroadcastMessage( "OnSwipeUp" );
        }

        if( Input.GetKeyDown( KeyCode.LeftShift ) )
        {
            MessageDispatch.BroadcastMessage( "OnSwipeDown" );
        }
		if (Input.GetKeyDown( KeyCode.D ) || Input.GetKeyDown (KeyCode.Mouse0))
		{
			MessageDispatch.BroadcastMessage( "OnTap");
		}

#endif
    }
	IEnumerator singleOrDouble(){
		yield return new WaitForSeconds(0.3f);
		if(touch.tapCount == 1)
			Debug.Log ("Single");
		else if(touch.tapCount == 2){
			//this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
			StopCoroutine("singleOrDouble");
			Debug.Log ("Double");
			MessageDispatch.BroadcastMessage( "OnTap" );

		}
	}


    /////////////////////////////////////////////
}
