﻿using UnityEngine;
using System.Collections;

public class ImpactEmittor : MonoBehaviour {

    private Rigidbody2D rbdy;

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
	public Vector2 ReallyFastSpeed;
	private float ContactClamp;
	private float MovingFastClamp;
	private float RotationClamp;
	private bool slamming = false;
	private bool jumping = false;
	private bool bearInvuln = false;
	public ParticleSystem slam;
	public ParticleSystem slamImpact;
	public ParticleSystem dash;
	public ParticleSystem invulnerable;
	private BearController bc;
	private Vector2 bearpos;
	private Vector2 bearspeed;
	public Animator anim;
	public Component[] Trails;
	private GameObject mCamera;
	private Vector3 originalOffset;
	private float startTime;
	public float resetTime = 1;
	
	
	void Awake () 
	{
		bc = GameObject.Find ("Bear_Body").GetComponent<BearController>();
        rbdy = GetComponent<Rigidbody2D>();
	}
	
	void Start ()
	{	
		startTime = Time.time;
		mCamera = GameObject.FindGameObjectWithTag("MainCamera");
		originalOffset = mCamera.GetComponent<GameCamera>().offset;
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
		bearspeed = rbdy.velocity;
		bearpos = rbdy.position;
		//CLAMP the min max value of the bears velocity for particle speed
		
		ContactClamp = Mathf.Clamp (rbdy.velocity.x, 4,14);
		MovingFastClamp = Mathf.Clamp (rbdy.velocity.x, 0,1f);
		RotationClamp = Mathf.Clamp (rbdy.velocity.x, 0, 0.1f);
		
		//ContactEmitter.startSpeed = ContactClamp;
		//ContactEmitter.startSize = MovingFastClamp;
		MovingFastEmitter.startSpeed = ContactClamp;
		
		
		
		
		
		//SLAMMING VFX //////
		
		if (slamming == true)
		{
            if( slam.isStopped )
			    slam.Play ();
			//reallyfast = false;
		}
		else
		{
            if( slam.isPlaying )
			 slam.Stop ();
		}

		if (bc.bearInvuln) {
			if( invulnerable.isStopped )
				invulnerable.Play ();
		}else
		{
			if( invulnerable.isPlaying )
				invulnerable.Stop ();
		}
		
		
		
		
		
		//DASH VFX //////
		
		if (bc.dashed == true)
		{
            if( dash.isStopped )
			dash.Play ();
		}
		if(bc.dashed == false)
		{
            if( dash.isPlaying )
			dash.Stop ();
		}
		
		
		
		
		
		
		
		
		//CONTACT EMITTER (Rooster Tail) ////// 
		
		//Emit particles if the bear is touching the ground while moving fast
		if(grounded == true && movingfast == true)
        {
            if( ContactEmitter.isStopped )
			    ContactEmitter.Play();
		}
		else
        { 
            if( ContactEmitter.isPlaying )
			    ContactEmitter.Stop();	
		}
		//The contact emitter shouldn't be running if the bear moves off of the terrain
		if (jumping == true)
		{
            if( ContactEmitter.isPlaying )
			ContactEmitter.Stop ();
		}
		
		
		//FAST MOVEMENT //
		if (rbdy.velocity.x >= MovingFastSpeed)
		{
			movingfast = true;
			mCamera.GetComponent<GameCamera>().offset = Vector3.Slerp (mCamera.GetComponent<GameCamera>().offset, mCamera.GetComponent<GameCamera>().MovingOffset, 0.008f);
		}
		else 
		{
			mCamera.GetComponent<GameCamera>().offset = Vector3.Slerp (mCamera.GetComponent<GameCamera>().offset, originalOffset, 0.008f);
			movingfast = false;
		}
		
		
		//REALLY FAST MOVEMENT//
		
		if (rbdy.velocity.x >= ReallyFastSpeed.x && rbdy.velocity.y <= ReallyFastSpeed.y){
			reallyfast = true;
			
			//if Bear is moving REALLY FAST zoom the camera out and give the bear a slight lead
			mCamera.GetComponent<GameCamera>().offset = Vector3.Slerp (mCamera.GetComponent<GameCamera>().offset, mCamera.GetComponent<GameCamera>().FastOffset, 0.008f);
			
		}
		else {
			
			mCamera.GetComponent<GameCamera>().offset = Vector3.Slerp (mCamera.GetComponent<GameCamera>().offset, mCamera.GetComponent<GameCamera>().MovingOffset, 0.008f);
			reallyfast = false;
			
		}
		
		
		//Emit TRAILS if the bear is moving REALLY fast ANYTIME
		if (reallyfast == true)
		{
			Trails = GetComponentsInChildren<TrailRenderer>();
			
			foreach (TrailRenderer Trail in Trails) 
			{
				Trail.enabled = true;
				Trail.time = 0.3f;
			}
			
			
		}
		else
		{
			if (grounded == false)
			{
				Trails = GetComponentsInChildren<TrailRenderer>();
				
				foreach (TrailRenderer Trail in Trails) 
				{
					Trail.enabled = true;
					Trail.time = 0.05f;
				}
			}
			if (grounded == true)
			{
				Trails = GetComponentsInChildren<TrailRenderer>();
				
				foreach (TrailRenderer Trail in Trails) 
				{
					
					Trail.enabled = false;
				}
			}
		}
	}
	
	
	//TREE COLLISION//
	void OnTriggerEnter2D(Collider2D other)
	{
		//If colliding with object tagged Avalanche then shake the main Camera. Currently this seems to shake on collision with anything.
		if (other.tag == "Avalanche");
		{
			//tell the GameCamera script to shake the cam
			//mCamera.GetComponent<GameCamera>().Shake = true;
		}
		
		
		if ( other.tag == "tree")
		{
			//Ping VFX if tree is collided with
			Instantiate( Impact, transform.position, Quaternion.identity );
			//Momentarily interrupt contact vfx if collided with tree.
			
			
		}
		if (other.tag == "Rock")
		{
			Instantiate( Impact, transform.position, Quaternion.identity );
			mCamera.GetComponent<GameCamera>().Shake = true;
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
