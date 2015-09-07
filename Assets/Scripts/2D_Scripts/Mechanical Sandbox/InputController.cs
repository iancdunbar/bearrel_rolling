using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	
	/////////////////////////////////////////////
	// Private Variable
	/////////////////////////////////////////////
	
	private bool active_touch;
	private Vector2 touch_begin;
	private float sqr_swipe_minimum;
	private float sqr_tap_tolerance;
	private bool gesture_processed;
	
	/////////////////////////////////////////////
	
	
	/////////////////////////////////////////////
	// Inspector Variables
	/////////////////////////////////////////////
	
	[SerializeField] private float swipe_minimum;
	[SerializeField] private float tap_tolerance;
	[SerializeField] private float touchDuration;
	[SerializeField] private Touch touch;
	
	/////////////////////////////////////////////
	
	
	/////////////////////////////////////////////
	// Inspector Variables
	/////////////////////////////////////////////
	
	// Use this for initialization
	void Start( )
	{
		sqr_swipe_minimum = swipe_minimum * swipe_minimum;
		sqr_tap_tolerance = tap_tolerance * tap_tolerance;
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
				MessageDispatch.BroadcastMessage( "OnTouchBegin", touch_begin );
			}
			else if( curr.phase == TouchPhase.Moved )
			{

				if( !gesture_processed )
				{

					Vector2 curr_pos = curr.position;
					Vector2 delta = curr_pos - touch_begin;

					if( delta.sqrMagnitude > sqr_swipe_minimum )
					{
						MessageDispatch.BroadcastMessage( "OnSwipe", delta.normalized );

						gesture_processed = true;
					}

				}

//				if( !gesture_processed && curr.position.y - touch_begin.y > swipe_minimum )
//				{
//					gesture_processed = true;
//					MessageDispatch.BroadcastMessage( "OnSwipeUp" );
//				}
//				else if( !gesture_processed && touch_begin.y - curr.position.y > swipe_minimum )
//				{
//					gesture_processed = true;
//					MessageDispatch.BroadcastMessage( "OnSwipeDown" );
//				}
				
			}
			else if( curr.phase == TouchPhase.Ended )
			{
				MessageDispatch.BroadcastMessage( "OnTouchEnded", curr.position );
			}
		}
		
#else
		
		if( Input.GetKey( KeyCode.Mouse0 ) )
		{
			if( !gesture_processed )
			{
				if( active_touch )
				{
					// Get the delta vector
					Vector2 curr_pos = Input.mousePosition;
					Vector2 delta = curr_pos - touch_begin;

					// If its greater than the swipe minimum, process a swipe
					if( delta.sqrMagnitude > sqr_swipe_minimum )
					{
						// Send the swipe message
						MessageDispatch.BroadcastMessage( "OnSwipe", delta.normalized );
						
						// Process the swipe
						gesture_processed = true;
						
					}
				}
				else
				{
					active_touch = true;
					touch_begin = Input.mousePosition;

					MessageDispatch.BroadcastMessage( "OnTouchBegin", touch_begin );
				}
			}
			
		}
		else
		{
			if( active_touch )
			{
				// Get the current touch location
				Vector2 curr_pos = Input.mousePosition;

				// Check to see for a tap
				if( !gesture_processed )
				{
					// Calculate the delta and compare the distance against the tap allowance
					Vector2 delta = curr_pos - touch_begin;
					if( delta.sqrMagnitude < sqr_tap_tolerance )
					{
						MessageDispatch.BroadcastMessage( "OnTap" );
					}	
				}

				MessageDispatch.BroadcastMessage( "OnTouchEnd", curr_pos );

				// Set active touch to false
				active_touch = false;
				gesture_processed = false;
			}
		}
		
		
		
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			MessageDispatch.BroadcastMessage( "OnSwipeUp" );
		}
		
		if( Input.GetKeyDown( KeyCode.LeftShift ) )
		{
			MessageDispatch.BroadcastMessage( "OnSwipeDown" );
		}
		
		if (Input.GetKeyDown( KeyCode.D ) )
		{
			MessageDispatch.BroadcastMessage( "OnTap" );
		}
		
#endif

	}

	/////////////////////////////////////////////
}