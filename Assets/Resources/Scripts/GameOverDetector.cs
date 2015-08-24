using UnityEngine;
using System.Collections;

public class GameOverDetector : MonoBehaviour {
    public bool isActive;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other) {
        if (isActive) {
            //if there are 2 players
            if (GameObject.Find("Player2") != null) {
                if (other.gameObject.name == "Player2") Application.LoadLevel("P1Wins");
                else if (other.gameObject.name == "Player1") Application.LoadLevel("P2Wins");
            }
            //else there must be 1 player
            else {
                if (other.gameObject.name == "Player1") GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>().GameOver();
            }
        }
        if (other.gameObject.tag == "FriendlyProjectile" || other.gameObject.tag == "EnemyProjectile") {
            Destroy(other.gameObject);
        }
    }
}
