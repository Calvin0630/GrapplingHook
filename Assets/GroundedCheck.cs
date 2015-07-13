using UnityEngine;
using System.Collections;

public class GroundedCheck : MonoBehaviour {
    Player player;
	// Use this for initialization
	void Start () {
        player = transform.parent.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "ground") player.isGrounded = true;

    }

    void OnTriggerExit(Collider other) {

    }
}
