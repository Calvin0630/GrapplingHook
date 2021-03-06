﻿using UnityEngine;
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
            player.isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Obstacle" /*|| other.tag == "Player1" || other.tag == "Player2"*/) {
			player.isGrounded = false; 
		}
    }
}
