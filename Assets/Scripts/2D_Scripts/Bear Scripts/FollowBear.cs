using UnityEngine;
using System.Collections;

public class FollowBear : MonoBehaviour {
	
	public float interpVelocity;
	public float minDistance;
	public float followDistance;
	public GameObject target;
	public Vector3 offset;
	Vector3 targetPos;
	private BearController bc;
	public Animator anim;





	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

		targetPos = transform.position;
		target = GameObject.Find ("Bear_Body");
		bc = target.GetComponent<BearController>();
	
	
	}
	void FixedUpdate (){

		if (bc.isSloped == true)

		{

			//Debug.Log ("Terrain is Sloped");

			//Set the animator parameter to true (triggers animation)
			anim.SetBool ("isSloped", true);


		}
		else
		{
			//Debug.Log ("Terrain is flat");

			//Set the animator parameter to false (triggers animation)
			anim.SetBool ("isSloped", false);



		}
			                 

			                


			            

		if(target)

		{
			transform.position = new Vector2 ( target.GetComponent<Rigidbody2D>().position.x + offset.x, target.GetComponent<Rigidbody2D>().position.y + offset.y );

		}


	
}
}
	// Update is called once per frame
//	void FixedUpdate () {
//		if (target)
//		{
//			Vector3 posNoZ = transform.position;
//			posNoZ.z = target.transform.position.z;
//			
//			Vector3 targetDirection = (target.transform.position - posNoZ);
//			
//			interpVelocity = targetDirection.magnitude * 5f;
//			
//			targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 
//			
//			transform.position = Vector3.Lerp( transform.position, targetPos + offset, 0.25f);
//			
//		}
//	}

