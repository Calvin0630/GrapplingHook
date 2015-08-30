using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour {

    Rigidbody rBody;
    public float moveSpeed;
    float relativeWorldSpeed;
    Vector3 cameraSize;
    Vector3 enemyToDestination;
    public bool isHooked;
    GameObject player;
    GameObject spawner;
    GameObject scoreManager;
    float worldVelocityX;
    GameObject projectile;
    public int health;
    int initialHealth;
    GameObject healthBarPrefab;
    GameObject healthBar;
    float randomnessScalar;

	// Use this for initialization
	void Start () {
        relativeWorldSpeed = .04f;
        rBody = gameObject.GetComponent<Rigidbody>();
        isHooked = false;
        cameraSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        player = GameObject.FindWithTag("Player1");
        spawner = GameObject.FindWithTag("Spawn");
        scoreManager = GameObject.Find("ScoreManager(Clone)");
        projectile = (GameObject)Resources.Load("Prefab/Projectile");
        if (health == 0) health = 1;
        initialHealth = health;
        healthBarPrefab = (GameObject) Resources.Load("Prefab/UI/EnemyHealth/EnemyHealthBar");
        healthBar = (GameObject)Instantiate(healthBarPrefab, 40 * Vector3.left, Quaternion.identity);
        healthBar.GetComponent<EnemyHealthBar>().enemy = gameObject;
        healthBar.transform.parent = GameObject.Find("Canvas").transform;
        randomnessScalar = .5f;
	}
	
	// Update is called once per frame
	void Update () {
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
            rBody.velocity = enemyToDestination + new Vector3(-worldVelocityX * relativeWorldSpeed ,0,0) ;
        }
        
    }

    void OnCollisionEnter(Collision other) {
        if (other.collider.gameObject.tag == "Player1") {
            StartCoroutine(PlayerIsCaught(0));
        }
        else if (other.gameObject.tag == "FriendlyProjectile") {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        else if (other.collider.gameObject.tag == "Shield") {
            Destroy(gameObject);
        }
    }

    void OnDestroy() {
        Destroy(healthBar);
    }

    IEnumerator PlayerIsCaught(float delay) {
        //this causes the delay
        yield return new WaitForSeconds(delay);
        if (scoreManager == null) scoreManager = GameObject.FindWithTag("ScoreManager");
        scoreManager.GetComponent<ScoreManager>().GameOver();
    }
    public void TakeDamage(int damage) {
        health -= damage;
        float fhealth = (float)health;
        float fInitialHealth = (float)initialHealth;
        healthBar.GetComponent<EnemyHealthBar>().SetValue((int) (fhealth/fInitialHealth * 100));
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
