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
            Rigidbody otherRigidbody = gameObject.transform.parent.gameObject.GetComponent<Rigidbody>();
            if (otherRigidbody==null) {
                //rBody.velocity = Vector3.zero;
            }
            else {
                //rBody.velocity = otherRigidbody.velocity;
            }
            hooked = true;
        }
        else {
            hooked = false;
        }
	}
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Obstacle") {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.transform.parent = other.gameObject.transform;
        }
        else if (other.gameObject.tag == "Enemy") {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.transform.parent = other.gameObject.transform;
            other.gameObject.GetComponent<Enemy>().isHooked = true;
        }
    }
}
