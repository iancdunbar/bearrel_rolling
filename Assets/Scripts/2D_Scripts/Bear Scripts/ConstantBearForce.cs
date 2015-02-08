using UnityEngine;
using System.Collections;

public class ConstantBearForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate (5f * Time.deltaTime, 0, 0, Space.World);
	}
}
