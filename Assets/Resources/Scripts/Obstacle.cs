using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter(Collision other) {
        bool isHittingTurret = false;
        foreach(ContactPoint c in other.contacts) {
            if (c.thisCollider.gameObject.tag == "Enemy") isHittingTurret = true;
        }
        Component[] components = gameObject.GetComponentsInChildren<Turret>();
        if (components.Length == 1 && isHittingTurret) {
            Turret turret = (Turret)components[0];
            turret.OnCollisionEnter(other);
        }
    }
}
