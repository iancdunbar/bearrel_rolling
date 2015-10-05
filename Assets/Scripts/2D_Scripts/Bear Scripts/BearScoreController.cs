using UnityEngine;
using System.Collections;

public class BearScoreController : Singleton<BearScoreController> {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int currentScore;

	public void AddPoints(int pointsToAdd){


	}

	public BearScoreController () {

		currentScore = 0;

	}
}
