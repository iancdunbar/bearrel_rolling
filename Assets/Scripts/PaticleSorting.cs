using UnityEngine;
using System.Collections;

public class PaticleSorting : MonoBehaviour {
	public ParticleSystem particleSystem;

	// Use this for initialization
	void Start ()
	{
		//Change Foreground to the layer you want it to display on 
		//You could prob. make a public variable for this
		particleSystem.renderer.sortingLayerName = "Foreground";
	}
}
