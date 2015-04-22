﻿using UnityEngine;
using System.Collections;

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
		if (bc.dashed == false){
			dashing = false;
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
		if (other.tag == "Bear" && dashing == true || slamming == true)
		{ 
			foreach (GameObject gib in gibs)
			{
				Instantiate( snow, transform.position, Quaternion.identity );
				Instantiate( branches, transform.position, Quaternion.identity);
				SimpleAudioController.PlayCrashEmote();
				gibspawn = (GameObject)Instantiate( gib, transform.position + Random.insideUnitSphere*spawnRadius, transform.rotation);
				gibspawn.GetComponent<Rigidbody2D>().AddForce(Vector2.up * breakforce, ForceMode2D.Impulse);
				gibspawn.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 40, ForceMode2D.Impulse);
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
