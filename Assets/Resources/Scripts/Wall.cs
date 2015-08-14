using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	// Use this for initialization
    public GameObject player;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    /*
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Hook") {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			other.gameObject.transform.parent = gameObject.transform;
        }
    }*/
}
