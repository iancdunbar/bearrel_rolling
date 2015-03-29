using UnityEngine;
using System.Collections;

public class ImpactEmittor : MonoBehaviour {

	// Use this for initialization
	public ParticleSystem snow;
    public bool can_blood = true;
	public bool grounded = false;
	public ParticleSystem ContactEmitter;
	public ParticleSystem MovingFastEmitter;
	private bool movingfast = false;
	private bool reallyfast = false;
	public float MovingFastSpeed;
	public float ReallyFastSpeed;
	private float BearVelocityX;
	private float BearVelocityY;
	private bool slamming = false;
	public ParticleSystem slam;
	public ParticleSystem slamImpact;


	void Start ()
	{
		MessageDispatch.RegisterListener( "OnEnterState", OnEnterState );
		MessageDispatch.RegisterListener( "OnExitState", OnExitState );
	}
	void OnEnterState ( object arg){
		BearState state = (BearState)arg;
		if (state == BearState.SLAMMING){
			slamming = true;
			
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
		BearVelocityX = Mathf.Clamp (rigidbody2D.velocity.x, 0,10);
		BearVelocityY = Mathf.Clamp (rigidbody2D.velocity.y, 0,8);
		ContactEmitter.startSpeed = BearVelocityX;
		MovingFastEmitter.startSpeed = BearVelocityX;

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


        can_blood = true;
			

			
	}


    void OnCollisionEnter2D( Collision2D other )
    {



		grounded = true;

		slam.Stop ();
		
		if( slamming == true && can_blood )
		{
			
			Vector3 pos = other.contacts[0].point;
			pos.z = transform.position.z;
			
			Instantiate( slamImpact, pos, Quaternion.identity );
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
