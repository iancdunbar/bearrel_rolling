using UnityEngine;
using System.Collections;

public class ImpactEmittor : MonoBehaviour {

	// Use this for initialization
	public ParticleSystem snow;
	
	void OnCollisionEnter2D (Collider2D col){
		
		if(col.gameObject.tag == "Ground"){
			Debug.Log ("Collision Occured");
			snow.Play ();

			
		}
		
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
