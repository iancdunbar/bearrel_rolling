﻿using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {

    /////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    private Rigidbody2D rbody;
    private bool jumped;
    private bool slammed;
	public bool dashed;
    private BearStateController bsc;
	private bool deathBool = false;
	public bool isSloped = false;


    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////

    [SerializeField]
    private float jump_strength;
	[SerializeField]
	private float jump_distance;
    [SerializeField]
    private float max_speed;
	[SerializeField]
	private float collision_speed;
	[SerializeField]
	private float slow_duration;
	private float return_value;
	[SerializeField]
	private float boostSpeed;
	[SerializeField]
	private float slam_speed;
	public Vector2 normal;
	private bool Grounded;
	public float dash_cooldown;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // State Messages
    /////////////////////////////////////////////

    public void OnEnterState( object arg )
    {
        BearState state = (BearState)arg;



        Debug.Log( "Entering " + state + " from BearController" );
    }

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Input Messages
    /////////////////////////////////////////////

    public void OnSwipeUp( object unused )
    {
        if( !jumped && dashed == false)
        {
            rigidbody2D.AddForce( Vector2.up * jump_strength, ForceMode2D.Impulse );
			rigidbody2D.AddForce( Vector2.right * jump_distance, ForceMode2D.Impulse );
            bsc.ChangeState( BearState.JUMPING );
            jumped = true;
        }
    }

	public void OnTap( object unused )
	{
		if( !dashed ) 
		{
			rigidbody2D.AddTorque (-200, ForceMode2D.Impulse);
			dashed = true;
			bsc.ChangeState( BearState.DASHING );
			StartCoroutine (DashCoolDown());
		}
	}
    public void OnSwipeDown( object unused )
    {

        if( !slammed && Grounded == false)
        {
			//Jared's temp adjustment
			rigidbody2D.AddForce (Vector2.up * -slam_speed, ForceMode2D.Impulse);
			rigidbody2D.AddTorque (-200, ForceMode2D.Impulse);
			//Original slamming
            //rigidbody2D.velocity = new Vector2( rigidbody2D.velocity.x, -10 );
            bsc.ChangeState( BearState.SLAMMING );
            slammed = true;
            jumped = true;
        }

    }


    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Unity Messages
    /////////////////////////////////////////////

    void Awake( )
    {
        rbody = GetComponent<Rigidbody2D>( );
        bsc = new BearStateController( );

        jumped = true;
    }

    // Use this for initialization
	void Start () 
    {
		return_value = max_speed;
        MessageDispatch.RegisterListener( "OnSwipeUp", OnSwipeUp );
        MessageDispatch.RegisterListener( "OnSwipeDown", OnSwipeDown );
        MessageDispatch.RegisterListener( "OnEnterState", OnEnterState );
		MessageDispatch.RegisterListener( "OnTap", OnTap );

        bsc.ChangeState( BearState.IDLE );
	}

    void OnCollisionEnter2D( Collision2D other )
    {

        if( other.gameObject.tag == "Ground" )
        {
            bsc.ChangeState( BearState.IDLE );
			Grounded = true;
            jumped = false;
            slammed = false;

			max_speed = return_value;
        }
	
    }
	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.tag == "Ground")
		{
			Grounded = false;
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
    {
		// If the bear is on the ground it is impossible for it to slam.
		if (Grounded == true)
		{
			slammed = false;
		}
		//Detecting the normal direction of the terrain below
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 1, transform.position.y + 2), -Vector2.up * 1);
		Debug.DrawRay (new Vector2(transform.position.x - 1, transform.position.y + 2), -Vector2.up * 1, Color.green);

		//What is the Vector2 normal of the object being hit by the raycast
		//Debug.Log (hit.normal.y);

		//Whats the tag of the collider hit by the raycast
		//Debug.Log (hit.normal.y);
	

		//If the x value of the normal being detected is greater than or = to 0.1 and the collider is named Ground then the terrain is sloped
		if( hit.collider == null ){ 
			Debug.Log( "The hit is null, this is what is causing the program to crash"); 
		}
		if (hit.normal.y <= 0.83f && hit.collider.tag == "Ground")
		{
			isSloped = true;
		}
		else{
			isSloped = false;
		}


		//Clamp the maximum velocity of the bear to max_speed
		rbody.velocity = Vector3.ClampMagnitude( rbody.velocity, max_speed );
		//Debug.Log (rigidbody2D.velocity.x);
		

	}

	void OnGUI(){
		if(deathBool){
			GUI.Box (new Rect (0,0,Screen.width,Screen.height), "<color=red><size=80>BEAR DEATH</size></color>");
		}
	}

	//Slow the bear down if it collides with a tree
	IEnumerator OnTriggerEnter2D( Collider2D other )
	{
	
		if ( other.tag == "tree" && dashed == true)
		{
			max_speed = return_value;
			other.gameObject.transform.Rotate (0,0,-2);
			StartCoroutine(SpeedLimitCooldown());
		}
		else
		{
			if(other.tag=="tree")
			{
				max_speed = collision_speed;
				other.gameObject.transform.Rotate (0,0,-4);
				StartCoroutine(SpeedLimitCooldown());
			}
		}

		if (other.tag =="Boost")
		{
			gameObject.rigidbody2D.AddForce(Vector2.right * boostSpeed);
			StartCoroutine(BoostCoolDown());
		}

		if ( other.tag =="Death")
		{
			deathBool = true;
			yield return new WaitForSeconds(5.0f);
			Application.LoadLevel(Application.loadedLevel);
		
		}
	}
	IEnumerator SpeedLimitCooldown (){
		yield return new WaitForSeconds(slow_duration);
		max_speed = return_value;
	}
	IEnumerator DashCoolDown (){
		yield return new WaitForSeconds(dash_cooldown);
		dashed = false;
	}
	IEnumerator BoostCoolDown(){
		yield return new WaitForSeconds(2);
		//gameObject.rigidbody2D.velocity = max_speed;
	}

    /////////////////////////////////////////////
}
