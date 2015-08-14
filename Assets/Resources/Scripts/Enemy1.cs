using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour {

    Rigidbody rBody;
    public Vector3 destination;
    public float moveSpeed;
    public float waitMoveSpeed;
    public float waitRadius;
    bool isTravelling;
    Vector3 enemyToDestination; 
    Vector3 waitingVelocity;
    public bool isHooked;

	// Use this for initialization
	void Start () {
        rBody = gameObject.GetComponent<Rigidbody>();
        isTravelling = true;
        isHooked = false;
        waitingVelocity = Vector3.zero;
        waitRadius = 1 / waitRadius;
	}
	
	// Update is called once per frame
	void Update () {
        if (isTravelling) {
            if (transform.position.x < destination.x + .5f && transform.position.x > destination.x - .5f && transform.position.y < destination.y + .5f
                && transform.position.y > destination.y - .5f) {

                isTravelling = false;
                enemyToDestination = Vector3.zero;
            }
            else enemyToDestination = (destination - transform.position).normalized * moveSpeed;
        }
        else if (!isHooked) {
            waitingVelocity = new Vector3(waitMoveSpeed * Mathf.Cos(waitRadius * Time.time), waitMoveSpeed * Mathf.Sin(waitRadius * Time.time), 0);
            rBody.velocity = enemyToDestination + waitingVelocity;
        }

        if (isHooked) {
            waitingVelocity = Vector3.zero;
            rBody.useGravity = true;
        }
	}
}
