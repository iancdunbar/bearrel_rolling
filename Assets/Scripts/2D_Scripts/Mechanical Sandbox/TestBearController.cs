using UnityEngine;
using System.Collections;

public class TestBearController : MonoBehaviour {

	private Rigidbody2D rbdy;

	// Use this for initialization
	void Start () 
	{
		rbdy = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			rbdy.isKinematic = !rbdy.isKinematic;
		}
	}	
}
