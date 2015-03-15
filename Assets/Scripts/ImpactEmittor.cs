﻿using UnityEngine;
using System.Collections;

public class ImpactEmittor : MonoBehaviour {

	// Use this for initialization
	public ParticleSystem snow;
    public bool can_blood = true;

	void OnTriggerEnter2D(Collider2D other){


        can_blood = true;
			

			
		}


    void OnCollisionEnter2D( Collision2D other )
    {
        Debug.Log( "Collision enter" );
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

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
