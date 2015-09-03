using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    GameObject healthBarPrefab;
    public float shotDelay;
    public float shotSpeed;
    GameObject projectilePrefab;
    GameObject target;
    GameObject player;

	void Start () {
        shotSpeed = 10;
        shotDelay = .5f;
        healthBarPrefab = (GameObject) Resources.Load("Prefab/UI/EnemyHealth/EnemyHealthBar");
        GameObject healthBar = (GameObject)Instantiate(healthBarPrefab, 40 * Vector3.left, Quaternion.identity);
        healthBar.GetComponent<EnemyHealthBar>().enemy = gameObject;
        healthBar.transform.parent = GameObject.Find("Canvas").transform;
        projectilePrefab = (GameObject)Resources.Load("Prefab/EnemyProjectile");
        player = GameObject.FindWithTag("Player1");
        StartCoroutine(ShootAtPlayer());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator ShootAtPlayer() {
        yield return new WaitForSeconds(shotDelay);
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = (player.transform.position - gameObject.transform.position).normalized * shotSpeed;
        StartCoroutine(ShootAtPlayer());
    }
}
