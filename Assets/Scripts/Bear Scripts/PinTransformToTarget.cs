using UnityEngine;
using System.Collections;

public class PinTransformToTarget : MonoBehaviour {


	// Use this for initialization
	//void Start () {
	
	//}
	public Transform ColliderContainer;

	// Update is called once per frame
	void Update () {
		Vector3 newPosition = this.transform.position;
		newPosition.z = 0f;
		newPosition.x = ColliderContainer.transform.position.x;
		newPosition.y = ColliderContainer.transform.position.y;



		this.transform.position = newPosition;
	}
}
