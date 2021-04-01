using UnityEngine;
using System.Collections;

public class GameOverDetector : MonoBehaviour {
    public bool isActive;
	// Use this for initialization
	void Start () {
        isActive = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other) {
        if (isActive) {
            if (other.gameObject.name == "Player1") ScoreManager.GameOver();
        }
        if (other.gameObject.tag == "FriendlyProjectile" || other.gameObject.tag == "EnemyProjectile") {
            Destroy(other.gameObject);
        }
    }
}
