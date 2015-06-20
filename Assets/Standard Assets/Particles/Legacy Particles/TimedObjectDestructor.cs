using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedObjectDestructor : MonoBehaviour {

	public float timeOut = 1.0f;
	public bool detachChildren = false;

	void Awake ()
	{	
		if (gameObject.tag == "treeGib"){
			StartCoroutine(DestroyNow ());
		}

		if (gameObject.tag == "Cabin" && gameObject.GetComponent<Rigidbody>().isKinematic == false){
			Debug.Log ("Destroying 3D physics object");
			StartCoroutine(DestroyNow ());
		}
	}
	IEnumerator DestroyNow ()
	{
		yield return new WaitForSeconds(timeOut);
		if (detachChildren == true) {
			transform.DetachChildren ();
		}
	
		DestroyObject (gameObject);
	
	}
}