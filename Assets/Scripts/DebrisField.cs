using UnityEngine;
using System.Collections;

public class DebrisField : MonoBehaviour {
	private Component rbody;
	private Component timedDestructor;
	private Component[] CabinBoards;
	public ParticleSystem slamImpact;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D( Collider2D other){

		Instantiate( slamImpact, other.transform.position, Quaternion.identity );
		if (other.tag == "Bear"){
		
		}
		else{
			if(other.tag == "Shrub" || other.tag == "tree" || other.tag == "Rock" || other.tag == "tree_gib")
			{
				other.GetComponent<Rigidbody2D>().isKinematic = true;
				other.transform.parent = gameObject.transform;
				rbody = other.GetComponent<Rigidbody2D>();
				timedDestructor = other.GetComponent("TimedObjectDestructor");
				Destroy(rbody);
				Destroy(timedDestructor);
				Destroy (other.GetComponent<Collider2D>());
				
			}
			if(other.tag == "Cabin"){
				CabinBoards = other.GetComponentsInChildren<Rigidbody>();
				
				foreach(Rigidbody board in CabinBoards){
					board.isKinematic = false;
					//board.transform.parent = gameObject.transform;
					timedDestructor = board.GetComponent("TimedObjectDestructor");
					Destroy (timedDestructor);
					Destroy(board);
					
				}
				//other.transform.parent = gameObject.transform;
				rbody = other.GetComponent<Rigidbody>();
				timedDestructor = other.GetComponent<TimedObjectDestructor>();
				Destroy(rbody);
				Destroy(timedDestructor);
				Destroy (other.GetComponent<Collider2D>());
				//other.gameObject.AddComponent<TimedObjectDestructor>().timeOut = 5;


			}
		}
	}
}
