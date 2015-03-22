using UnityEngine;
using System.Collections;

public class TreeGib : MonoBehaviour {
	public GameObject[] gibs;
	public float explosionForce = 2000;
	public float spawnRadius = 1.0f;
	private GameObject gibspawn;
	public float breakforce;
	public ParticleSystem snow;
	public ParticleSystem branches;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Bear")
		{ 
			foreach (GameObject gib in gibs)
			{
				Instantiate( snow, transform.position, Quaternion.identity );
				Instantiate( branches, transform.position, Quaternion.identity);
				SimpleAudioController.PlayCrashEmote();
				gibspawn = (GameObject)Instantiate( gib, transform.position + Random.insideUnitSphere*spawnRadius, transform.rotation);
				gibspawn.rigidbody2D.AddForce(Vector2.up * breakforce, ForceMode2D.Impulse);
				gibspawn.rigidbody2D.AddForce(Vector2.right * 40, ForceMode2D.Impulse);
				Destroy(gameObject);
			}
		}
	}



		

}
