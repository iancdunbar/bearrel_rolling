﻿using UnityEngine;
using System.Collections;

public class ImpactEmittor : MonoBehaviour {

	// Use this for initialization
	public ParticleSystem snow;
	public ParticleSystem Impact;
    public bool can_blood = true;
	public bool grounded = false;
	public ParticleSystem ContactEmitter;
	public ParticleSystem MovingFastEmitter;
	private bool movingfast = false;
	private bool reallyfast = false;
	public float MovingFastSpeed;
	public float ReallyFastSpeed;
	private float ContactClamp;
	private float MovingFastClamp;
	private bool slamming = false;
	private bool jumping = false;
	public ParticleSystem slam;
	public ParticleSystem slamImpact;
	public ParticleSystem dash;
	private BearController bc;



	void Start ()
	{
		bc = GameObject.Find ("Bear_Body").GetComponent<BearController>();
		MessageDispatch.RegisterListener( "OnEnterState", OnEnterState );
		MessageDispatch.RegisterListener( "OnExitState", OnExitState );
	}
	void OnEnterState ( object arg){
		BearState state = (BearState)arg;
		if (state == BearState.SLAMMING){
			slamming = true;
			
		}
		if(state == BearState.JUMPING){
			jumping = true;
		}
		}

	void OnExitState (object arg){
		BearState  state = (BearState)arg;
		if (state == BearState.SLAMMING){

		}
	}
	void Update () 
	{


		//clamp the min max value of the bears velocity for particle speed
		ContactClamp = Mathf.Clamp (rigidbody2D.velocity.x, 0,10);
		MovingFastClamp = Mathf.Clamp (rigidbody2D.velocity.x, 0,1);
		ContactEmitter.startSpeed = ContactClamp;
		MovingFastEmitter.startSpeed = MovingFastClamp;

		if (jumping == true)
		{
			ContactEmitter.Stop ();
		}
		//SLAMMING VFX

		if (slamming == true)
		{
			slam.Play ();
			reallyfast = false;
		}
		else
		{
			slam.Stop ();
		}

		//DASH VFX
		if (bc.dashed == true)
		{
			dash.Play ();
		}
		if(bc.dashed == false)
		{
			dash.Stop ();
		}

		//Establish if the bear is moving fast
		if (rigidbody2D.velocity.x >= MovingFastSpeed){
			movingfast = true;
		}
		else {
			movingfast = false;
		}

		//Establish if the bear is moving REALLY fast in x
			if (rigidbody2D.velocity.x >= ReallyFastSpeed){
				reallyfast = true;
			}
			else {
				reallyfast = false;
			}



		//Emit particles if the bear is touching the ground while moving fast
		if(grounded == true && movingfast == true){
			ContactEmitter.Play();
		}
		else { 
			ContactEmitter.Stop();
		
		}

		//Emit particles if the bear is moving REALLY fast ANYTIME.
		if (reallyfast == true){
			MovingFastEmitter.Play();

		}
		else {
			MovingFastEmitter.Stop ();

		}


	}
	void OnTriggerEnter2D(Collider2D other)
	{

		if ( other.tag == "tree")
		{
			Instantiate( Impact, transform.position, Quaternion.identity );
		}
        can_blood = true;
			

			
	}


    void OnCollisionEnter2D( Collision2D other )
    {



		grounded = true;
		jumping = false;
		slam.Stop ();


		if( slamming == true && can_blood )
		{
			
			Vector3 pos = other.contacts[0].point;
			pos.z = transform.position.z;
			
			Instantiate( slamImpact, pos, Quaternion.identity );
			Instantiate( Impact, transform.position, Quaternion.identity );
			SimpleAudioController.PlayCrashEmote( );
			
			can_blood = false;
			slamming = false;
			

		}
		if( can_blood )
        {
            
            Vector3 pos = other.contacts[0].point;
            pos.z = transform.position.z;

            Instantiate( snow, pos, Quaternion.identity );
            SimpleAudioController.PlayCrashEmote( );

            can_blood = false;

        }

    }
	void OnCollisionExit2D( Collision2D other)
	{


			grounded = false;


	}



}
