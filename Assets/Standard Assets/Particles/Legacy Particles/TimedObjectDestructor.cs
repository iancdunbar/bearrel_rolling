using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedObjectDestructor : MonoBehaviour {

	public float timeOut = 1.0f;
	public bool detachChildren = false;

	void Awake ()
	{	
		if (gameObject.GetComponent<TimedObjectDestructor>().enabled == true){
		Invoke ("DestroyNow", timeOut);
		}
	}

	void DestroyNow ()
	{
		if (detachChildren == true) {
			transform.DetachChildren ();
		}
	
		DestroyObject (gameObject);
	
	}
}