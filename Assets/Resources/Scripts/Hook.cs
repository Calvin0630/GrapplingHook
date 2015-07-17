using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour {
    Rigidbody rBody;
    public bool hooked;
	// Use this for initialization
	void Start () {
        rBody = gameObject.GetComponent<Rigidbody>();
        hooked = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.parent != null) {
            rBody.velocity = gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().velocity;
            hooked = true;
        }
        else {
            hooked = false;
        }
	}
}
