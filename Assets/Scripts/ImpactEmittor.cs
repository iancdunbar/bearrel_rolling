using UnityEngine;
using System.Collections;

public class ImpactEmittor : MonoBehaviour {

	// Use this for initialization
	public ParticleSystem snow;
	
	void OnTriggerEnter2D(Collider2D other){

			Debug.Log ("Collision Occured");
			Instantiate(snow, transform.position, Quaternion.identity);
			

			
		}
		

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
