using UnityEngine;
using System.Collections;

public class TimedObjectDestructor : MonoBehaviour {

	private float delay = 2.0f; //This implies a delay of 2 seconds.
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Destroy(gameObject, delay);
	}
}
