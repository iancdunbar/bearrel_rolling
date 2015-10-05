using UnityEngine;
using System.Collections;

public class TestBearController : MonoBehaviour {

	/////////////////////////////////////////////
	// Private Variables
	/////////////////////////////////////////////

	private CollectabitCloud active_cloud;
	
	private Rigidbody2D rbdy;
	private Vector2 saved_velocity;
	private float default_gravity_scale;
	private bool kinematic_mode;
	private bool do_vel;
	private LTDescr active_tween;

	/////////////////////////////////////////////


	/////////////////////////////////////////////
	// Inspector Variables
	/////////////////////////////////////////////

	[SerializeField]
	private GameObject bit_prefab;

	/////////////////////////////////////////////
	

	/////////////////////////////////////////////
	// Private Functions
	/////////////////////////////////////////////

	private void toggle_kinematic( bool state )
	{
		if( state )
		{

			GameObject bits = (GameObject)Instantiate( bit_prefab );
			bits.transform.position = transform.position;

			active_cloud = bits.GetComponent<CollectabitCloud>( );

			saved_velocity = rbdy.velocity;
			do_vel = true;

			Time.timeScale = 0.3f;
		}
		else
		{
			rbdy.velocity = saved_velocity;
			if( active_tween != null )
			{
				active_tween.cancel( );
				active_tween = null;
			}

			Time.timeScale = 1;
		}

		
		
		rbdy.isKinematic = state;
		kinematic_mode = state;

	}

	/////////////////////////////////////////////
	

	/////////////////////////////////////////////
	// Public Functions
	/////////////////////////////////////////////

	public void CollisionNotify( Collision2D collision )
	{
		if( kinematic_mode )
		{
			toggle_kinematic( false );

		}
	}

	/////////////////////////////////////////////
	

	/////////////////////////////////////////////
	// Input Messages
	/////////////////////////////////////////////

	void OnSwipe( object arg )
	{
		if( kinematic_mode && active_tween == null )
		{
			Vector2 direction = (Vector2)arg;

			//Debug.Log( direction );

			if( active_cloud != null )
			{
				CollectabitData result = active_cloud.TestBits( direction, transform.position );

				if( result != null )
				{
					do_vel = false;
					active_tween = LeanTween.move( this.gameObject, result.pos, 0.1f ).setOnComplete( 
		                ( )=>
		                { 
						 	if( active_cloud.CollectBit( result.index ) )
							{
								Destroy( active_cloud );
								active_cloud = null;
								toggle_kinematic( false );	
							}
							active_tween = null; 
						} );
				}
				else
				{
					toggle_kinematic( false );
				}
			}


//			do_vel = false;
//
//			Vector3 tgt = transform.position + new Vector3( direction.x, direction.y, 0 ) * saved_velocity.magnitude;
//
//			if( active_tween != null ) active_tween.cancel( );
//			active_tween = LeanTween.move( this.gameObject, tgt, 0.33f ).setOnComplete( ( )=>{ do_vel = true; active_tween = null; } );
		}
	}

	void OnTap( object arg )
	{
		Debug.Log( "Tap!" );
	}

	void OnTouchBegin( object arg )
	{
		Debug.Log( "Touch Began" );
	}

	void OnTouchEnd( object arg )
	{
		Debug.Log( "Touch Ended" );
	}

	/////////////////////////////////////////////


	/////////////////////////////////////////////
	// Unity Messages
	/////////////////////////////////////////////

	// Use this for initialization
	void Start () 
	{
		rbdy = GetComponent<Rigidbody2D> ();
		default_gravity_scale = rbdy.gravityScale;

		MessageDispatch.RegisterListener( "OnSwipe", OnSwipe );
		MessageDispatch.RegisterListener( "OnTap", OnTap );
//		MessageDispatch.RegisterListener( "OnTouchBegin", OnTouchBegin );
//		MessageDispatch.RegisterListener( "OnTouchEnd", OnTouchEnd );

		kinematic_mode = false;

	}

	void FixedUpdate( )
	{
		if( kinematic_mode && do_vel ) 
		{
			transform.position = transform.position + ( new Vector3( saved_velocity.x, saved_velocity.y, 0 ) * Time.fixedDeltaTime );
		}
	}

	// Update is called once per frame
	void Update () 
	{


		if (Input.GetKeyDown (KeyCode.Space) ) 
		{

			toggle_kinematic( !kinematic_mode );

		}
	}	




	/////////////////////////////////////////////
	
}
