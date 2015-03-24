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

	void Start ()
	{

	}

	void Update () 
	{
		ContactEmitter.startSpeed = rigidbody2D.velocity.x;
		MovingFastEmitter.startSpeed = rigidbody2D.velocity.x;
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

		//Establish if the bear is moving REALLY fast in y
		if (rigidbody2D.velocity.y <= -ReallyFastSpeed){
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


		if( can_blood )
        {
            Debug.Log( "Collision Occured" );
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
