using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2") {
            other.gameObject.GetComponent<Player>().TakeDamage(other.gameObject.tag, 1);
        }
    }
}
