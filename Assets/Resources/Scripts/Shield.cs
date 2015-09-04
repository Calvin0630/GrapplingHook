using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (transform.parent != null) {
            transform.localScale = transform.localScale * 1.5f;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "EnemyProjectile") {
            Destroy(other.gameObject);
        }
        print(other.gameObject.tag);
    }
}
