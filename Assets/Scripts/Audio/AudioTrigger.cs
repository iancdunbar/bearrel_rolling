using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour 
{

	[SerializeField]
	private bool onTrigger = true;
	[SerializeField]
	private bool onCollision = false;

	[SerializeField]
	private string triggerTag = "Bear";
	
	[SerializeField]
	private AudioClip[] clips;

	void Start( )
	{
		bool one_collider = false;
		bool two_collider = false;

		Collider2D[] colliders = GetComponents<Collider2D> ();

		for (int i = 0; i < colliders.Length; i++) 
		{
			if( onTrigger )
			{
				if( colliders[ i ].enabled && colliders[ i ].isTrigger )
				{
					if( !one_collider )
					{
						one_collider = true;
					}
					else
					{
						two_collider = true;
					}
				}
			}

			if( onCollision )
			{
				if( colliders[ i ].enabled && !colliders[ i ].isTrigger )
				{
					if( !one_collider )
					{
						one_collider = true;
					}
					else
					{
						two_collider = true;
					}
				}
			}
		}



		if (!one_collider)
			Debug.LogWarning ("WARNING: No active collider found on " + this.name + ". Audio Trigger may not work properly. ");

		if (two_collider)
			Debug.LogWarning ("WARNING: Multiple trigger objects found on " + this.name + ". Audio events may trigger multiple times.");

	}


	void OnTriggerEnter2D( Collider2D other )
	{

		if ( onTrigger && other.tag == triggerTag && clips.Length > 0 ) 
		{
			SimpleAudioController.PlaySound( clips[ Random.Range( 0, clips.Length ) ] );
		}

	}

	void OnCollisionEnter2D( Collision2D other )
	{
		
		if (onCollision && other.gameObject.tag == triggerTag && clips.Length > 0 ) 
		{
			SimpleAudioController.PlaySound( clips[ Random.Range( 0, clips.Length ) ] );
		}
		
	}


}
