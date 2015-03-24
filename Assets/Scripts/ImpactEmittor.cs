using UnityEngine;
using System.Collections;

public class ImpactEmittor : MonoBehaviour {

	// Use this for initialization
	public ParticleSystem snow;
    public bool can_blood = true;
	public bool grounded = false;
	public ParticleSystem ContactEmitter;
	private bool movingfast = false;
	public float movingfastSpeed;

	void Start ()
	{

	}

	void Update () 
	{
		if (rigidbody2D.velocity.x >= movingfastSpeed){
			movingfast = true;
		}
		else {
			movingfast = false;
		}

		if(grounded == true && movingfast == true){
			ContactEmitter.Play();
		}
		else { 
			ContactEmitter.Stop();
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
