using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	public float shake = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	public bool Shake = false;
	public GameObject Bear;
	private Transform trans_ref;
	private Vector3 previous_position;
	private Vector3 position_delta;
	public Vector3 offset;
	public Vector3 FastOffset;
	private float shakeOriginal;
	private GameObject BearLight;

	/////////////////////////////////////////////
	// Inspector Variables
	/////////////////////////////////////////////
	
	[SerializeField]
	private Transform follow_target;
	[SerializeField]
	private Vector3 follow_amount;
	
	/////////////////////////////////////////////
	
	Vector3 originalPos;

	void Awake () {

		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
		trans_ref = this.transform;



	}

	// Use this for initialization
	void Start () {
		BearLight = GameObject.Find("BearLight");
		Bear = GameObject.Find("Bear_Body");
		follow_target = Bear.GetComponent(typeof(Transform)) as Transform;
		// Initialize the previous position
		previous_position = follow_target.position + offset;
		shakeOriginal = shakeAmount;


	}
	

	
	// Update is called once per frame
	void Update () {

		// Follow the bear at an offset that is being updated every frame
		trans_ref.position = follow_target.position +offset ;

		// Capture the current target position for use in the next frame
		previous_position = follow_target.position + offset;

		

		//SHAKE original pos
		originalPos = previous_position;

		//Tell the camera to shake if called externally ex. ImpactEmitter
		if (Shake == true )
		{
			StartCoroutine(ShakeCam());
			

		}
		BearLight.transform.position = new Vector3 (BearLight.transform.position.x, BearLight.transform.position.y, offset.z / 2);

	}
	IEnumerator ShakeCam(){

		camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
		yield return new WaitForSeconds(0.2f);
		Shake = false;
		// Return to position after shake
		camTransform.localPosition = trans_ref.position;
		shakeAmount = shakeOriginal;

	}
}
