using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour {

    Rigidbody rBody;
    public float moveSpeed;
    float relativeWorldSpeed;
    Vector3 enemyToDestination;
    public bool isHooked;
    GameObject player;
    GameObject spawner;
    GameObject scoreManager;
    float worldVelocityX;
    GameObject projectile;
    public int health;

	// Use this for initialization
	void Start () {
        relativeWorldSpeed = .2f;
        rBody = gameObject.GetComponent<Rigidbody>();
        isHooked = false;
        player = GameObject.FindWithTag("Player1");
        spawner = GameObject.FindWithTag("Spawn");
        scoreManager = GameObject.Find("ScoreManager(Clone)");
        projectile = (GameObject)Resources.Load("Prefab/Projectile");
	}
	
	// Update is called once per frame
	void Update () {
        if (moveSpeed == 0) Debug.Log("Enemy's moveSpeed is 0");
        worldVelocityX = spawner.GetComponent<ObstacleSpawner>().worldVelocityX;
        enemyToDestination = (player.transform.position - transform.position).normalized * moveSpeed;

        if (isHooked) {
            rBody.useGravity = true;
        }
        else {
            rBody.velocity = enemyToDestination + new Vector3(-worldVelocityX * relativeWorldSpeed ,0,0) ;
        }
        
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player1") {
            StartCoroutine(PlayerIsCaught(0));
        }
        else if (other.gameObject.tag == "Projectile") {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    IEnumerator PlayerIsCaught(float delay) {
        //this causes the delay
        yield return new WaitForSeconds(delay);
        if (scoreManager == null) scoreManager = GameObject.FindWithTag("ScoreManager");
        scoreManager.GetComponent<ScoreManager>().GameOver();
    }
    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
