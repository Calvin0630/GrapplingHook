using UnityEngine;
using System.Collections;

public class GameOverDetector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision col) { 
		if (col.gameObject.tag == "Player1") {
			Application.LoadLevel("P2Wins");
		} 
		else if (col.gameObject.tag == "Player2") {
			Application.LoadLevel("P1Wins");
		}
	}
}
