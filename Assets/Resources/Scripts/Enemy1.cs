using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour {

    Rigidbody rBody;
    public Vector3 destination;
    float moveSpeed;
    float relativeWorldSpeed;
    Vector3 enemyToDestination;
    public bool isHooked;
    GameObject player;
    GameObject spawner;
    GameObject scoreManager;
    float worldVelocityX;
    GameObject projectile;

	// Use this for initialization
	void Start () {
        moveSpeed = 2;
        relativeWorldSpeed = .1f;
        rBody = gameObject.GetComponent<Rigidbody>();
        isHooked = false;
        player = GameObject.FindWithTag("Player1");
        spawner = GameObject.FindWithTag("Spawn");
        scoreManager = GameObject.Find("ScoreManager(Clone)");
        projectile = (GameObject)Resources.Load("Prefab/Projectile");
	}
	
	// Update is called once per frame
	void Update () {
        worldVelocityX = spawner.GetComponent<ObstacleSpawner>().worldVelocityX;
        enemyToDestination = (player.transform.position - transform.position).normalized * moveSpeed;
        Debug.Log(player.transform.position - transform.position);
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
    }
    IEnumerator PlayerIsCaught(float delay) {
        //this causes the delay
        yield return new WaitForSeconds(delay);
        if (scoreManager == null) scoreManager = GameObject.Find("ScoreManager(Clone)");
        scoreManager.GetComponent<ScoreManager>().GameOver();
        /*
        scoreManager.GetComponent<ScoreManager>().AddScore(new Score((int) spawner.GetComponent<ObstacleSpawner>().distanceTravelled, 10, (int) Time.time));
        Debug.Log(scoreManager.GetComponent<ScoreManager>().ToString());
        Application.LoadLevel("SinglePlayerGameOver");
        */
    }
}
