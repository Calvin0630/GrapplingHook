using UnityEngine;
using System.Collections;

public class Chaser : Enemy {

    Rigidbody rBody;
    public float moveSpeed;
    float relativeWorldSpeed;
    Vector3 cameraSize;
    Vector3 enemyToDestination;
    GameObject player;
    GameObject spawner;
    GameObject scoreManager;
    float worldVelocityX;
    GameObject projectile;
    float randomnessScalar;

    // Use this for initialization
    void Start() {
        base.Start();
        player = GameObject.FindWithTag("Player1");
        relativeWorldSpeed = .04f;
        rBody = gameObject.GetComponent<Rigidbody>();
        randomnessScalar = .5f;
    }

    // Update is called once per frame
    void Update() {
        //if (moveSpeed == 0) Debug.Log("Enemy's moveSpeed is 0");
        worldVelocityX = ObstacleSpawner.worldVelocityX;
        enemyToDestination = (player.transform.position - transform.position).normalized * moveSpeed;
        //Debug.Log(enemyToDestination);
        float edgeOfCameraToPlayer = player.transform.position.x + cameraSize.x;
        float enemyToPlayer = player.transform.position.x - transform.position.x;
        //enemyToDestination *= 1 + enemyToPlayer/edgeOfCameraToPlayer;
        //Debug.Log(enemyToDestination);
        //Debug.Log("edgeOfCameraToPlayer " + edgeOfCameraToPlayer);
        //Debug.Log("enemyToPlayer " + enemyToPlayer);
        if (isHooked) {
            rBody.useGravity = true;
        }
        else if (moveSpeed > 0) {
            rBody.velocity = enemyToDestination + new Vector3(-worldVelocityX * relativeWorldSpeed, 0, 0);
        }

    }

    void OnCollisionEnter(Collision other) {
        base.OnCollisionEnter(other);
        if (other.collider.gameObject.tag == "Player1") {
            StartCoroutine(PlayerIsCaught(0));
        }
        else if (other.collider.gameObject.tag == "Shield") {
            Destroy(gameObject);
        }
    }

    IEnumerator PlayerIsCaught(float delay) {
        //this causes the delay
        yield return new WaitForSeconds(delay);
        ScoreManager.GameOver();
    }
}
