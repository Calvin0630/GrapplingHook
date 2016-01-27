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
    public bool isCollidingWithChaser;

    // Use this for initialization
    void Start() {
        isCollidingWithChaser = false;
        base.Start();
        player = GameObject.FindWithTag("Player1");
        relativeWorldSpeed = .06f;
        rBody = gameObject.GetComponent<Rigidbody>();
        randomnessScalar = .5f;
        cameraSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    // Update is called once per frame
    void Update() {
        Vector3 enemyToPlayer = player.transform.position - transform.position;
        float enemyToPlayerAngle = Vector3.Angle(player.transform.position - transform.position, new Vector3(10, 0, 0));
        if (enemyToPlayer.y < 0) enemyToPlayerAngle *= -1;
        transform.localEulerAngles = new Vector3(0, 0, enemyToPlayerAngle);
        worldVelocityX = BuildingSpawner.worldVelocityX;
        enemyToDestination = (player.transform.position - transform.position).normalized * moveSpeed;
        //Debug.Log(enemyToDestination);
        float edgeOfCameraToPlayer = player.transform.position.x + cameraSize.x;
        //print((5.3f + 1/8)/cameraSize.y);
        float relativeSpeed = (enemyToPlayer.x + 1 / 8) / (cameraSize.y) + 1  ;
        if (isHooked) {
            rBody.useGravity = true;
        }
        else if (moveSpeed > 0) {
            rBody.velocity = relativeSpeed * enemyToDestination + new Vector3(-worldVelocityX * relativeWorldSpeed, 0, 0);
        }

    }

    void OnCollisionEnter(Collision other) {
        base.OnCollisionEnter(other);
        //convoluted code because unity doesnt know how children colliders work
        //P.S. Unity is fucking garbage
        //P.P.S. it checks shield stuff and player stuff
        if (other.gameObject.tag == "Player1") {
            if (other.gameObject.GetComponent<Player>().HasShield()) Destroy(gameObject);
            else StartCoroutine(PlayerIsCaught(0));
        }
        if (other.gameObject.name == "Chaser(Clone)" && !isCollidingWithChaser) {
            other.gameObject.GetComponent<Chaser>().isCollidingWithChaser = true;
            initialHealth += other.gameObject.GetComponent<Chaser>().initialHealth;
            health += other.gameObject.GetComponent<Chaser>().health;
            transform.localScale += .25f * other.gameObject.transform.localScale;
            gameObject.GetComponent<Chaser>().healthBar.GetComponent<EnemyHealthBar>().SetScale();
            Destroy(other.gameObject);
        }
    }

    IEnumerator PlayerIsCaught(float delay) {
        //this causes the delay
        yield return new WaitForSeconds(delay);
        ScoreManager.GameOver();
    }
}
