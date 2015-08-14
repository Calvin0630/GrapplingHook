﻿using UnityEngine;
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
    GameObject player;
    GameObject spawner;
    float worldVelocityX;

	// Use this for initialization
	void Start () {
        rBody = gameObject.GetComponent<Rigidbody>();
        isTravelling = true;
        isHooked = false;
        waitingVelocity = Vector3.zero;
        waitRadius = 1 / waitRadius;
        player = GameObject.FindWithTag("Player1");
        spawner = GameObject.FindWithTag("Spawn");
	}
	
	// Update is called once per frame
	void Update () {
        worldVelocityX = spawner.GetComponent<ObstacleSpawner>().worldVelocityX;
        Debug.Log(gameObject.transform.position.x - player.transform.position.x);
        if (isTravelling) {
            if (transform.position.x < destination.x + .5f && transform.position.x > destination.x - .5f && transform.position.y < destination.y + .5f
                && transform.position.y > destination.y - .5f) {

                isTravelling = false;
                enemyToDestination = Vector3.zero;
            }
            else {
                enemyToDestination = (destination - transform.position).normalized * moveSpeed;
            }
        }
        else if (!isHooked && !isTravelling) {
            waitingVelocity = new Vector3(waitMoveSpeed * Mathf.Cos(waitRadius * Time.time), waitMoveSpeed * Mathf.Sin(waitRadius * Time.time), 0);
            waitingVelocity += new Vector3((player.transform.position.x - gameObject.transform.position.x) - .5f * worldVelocityX, 0, 0);
        }

        if (isHooked) {
            waitingVelocity = Vector3.zero;
            rBody.useGravity = true;
        }
        else {

            rBody.velocity = enemyToDestination + waitingVelocity;
        }
        
    }
}