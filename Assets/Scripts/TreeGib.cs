using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeGib : MonoBehaviour {
	public GameObject[] gibs;
	public float explosionForce = 2000;
	public float spawnRadius = 1.0f;
	private GameObject gibspawn;
	public float breakforce;
	public ParticleSystem snow;
	public ParticleSystem branches;
	private bool slamming = false;
	private bool dashing = false;
	private bool bearInvuln = false;
	private BearStateController bsc;
	private BearController bc;
	private GameObject mCamera;

	void Awake (){

	}
	// Use this for initialization
	void Start () {
		MessageDispatch.RegisterListener( "OnEnterState", OnEnterState );
		MessageDispatch.RegisterListener( "OnExitState", OnExitState );
		bc = GameObject.Find("Bear_Body").GetComponent<BearController>();
		mCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}

	void Update () {
		if (bc.dashed == true){
			dashing = true;
		} 
		if (bc.dashed == false) {
			dashing = false;
		}
		if (bc.bearInvuln == true) {
			bearInvuln = true;
		} 
		if (bc.bearInvuln == false) {
			bearInvuln = false;
		}

	}
	
	// Update is called once per frame


	void OnEnterState ( object arg){
		BearState state = (BearState)arg;

		if (state == BearState.SLAMMING){

			slamming = true;

		}


	}
	void OnExitState (object arg){
		BearState  state = (BearState)arg;
		if (state == BearState.SLAMMING){
			slamming = false;
		}

	}

	
	


	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.tag == "Bear" && (dashing || slamming || bearInvuln))
		{ 
			//let's make every gib shoot off in a direction + or - 65 degrees from the bear's velocity vector. 
			//We'll also make their speed relted to the bear's speed. 
			//Sick gib explosion!
			//bc.decreaseInvulnSlider(bc.invuln_bar_tree_penalty);

			Vector2 currentBearVelocity = bc.currentVelocity;

			foreach (GameObject gib in gibs)
			{
				Instantiate( snow, transform.position, Quaternion.identity );
				Instantiate( branches, transform.position, Quaternion.identity);
				SimpleAudioController.PlayCrashEmote();

				gibspawn = (GameObject)Instantiate( gib, transform.position + Random.insideUnitSphere*spawnRadius, transform.rotation * Quaternion.Euler(0,0,Random.Range(0,360)));

				float randomTrajectoryAngle = Random.Range(-65, 65);
				Vector2 gibTrajectory = Vector2Extension.Rotate(currentBearVelocity, randomTrajectoryAngle);

				gibspawn.GetComponent<Rigidbody2D>().AddForce(gibTrajectory * 20, ForceMode2D.Impulse);
				Destroy(gameObject);
			}
		}
		if (other.tag == "Bear" && dashing == false && slamming == false)
		{
			Instantiate( snow, transform.position, Quaternion.identity );
			SimpleAudioController.PlayCrashEmote();
			//tell the GameCamera script to shake the cam
			mCamera.GetComponent<GameCamera>().Shake = true;
		}

	}
		

}

public static class Vector2Extension {

	//some code to rotate vecotr2s by x degrees around z axis

	public static Vector2 Rotate(this Vector2 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
		
		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);
		return v;
	}
}
