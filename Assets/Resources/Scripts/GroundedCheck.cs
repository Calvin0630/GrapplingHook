using UnityEngine;
using System.Collections;

public class GroundedCheck : MonoBehaviour {
    Player player;
	// Use this for initialization
	void Start () {
        player = transform.parent.gameObject.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Obstacle" || other.tag == "Player1" || other.tag == "Player2") {
            print("OnTriggerEnter: " + other.gameObject.tag);
            player.isGrounded = true;
            print("isGrounded: " + player.isGrounded);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Obstacle" /*|| other.tag == "Player1" || other.tag == "Player2"*/) {
            print("OnTriggerExit: " + other.gameObject.tag);
			player.isGrounded = false; 
		}
    }
}
