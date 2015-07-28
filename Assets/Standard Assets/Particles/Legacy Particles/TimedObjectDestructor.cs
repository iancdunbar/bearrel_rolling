using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedObjectDestructor : MonoBehaviour {

	public float timeOut = 1.0f;
	public bool detachChildren = false;

	void Awake ()
	{	
			StartCoroutine(DestroyNow ());
	}
	IEnumerator DestroyNow ()
	{
		yield return new WaitForSeconds(timeOut);
		DestroyObject (gameObject);
		if (detachChildren == true) {
			transform.DetachChildren ();
		}
	

	
	}
}