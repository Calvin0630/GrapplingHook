using UnityEngine;
using System.Collections;

public class Turret : Enemy {
    
    public float shotDelay;
    public float shotSpeed;
    GameObject projectilePrefab;
    GameObject target;
    GameObject player;

	void Start () {
        base.Start();
        shotSpeed = 10;
        shotDelay = .5f;
        projectilePrefab = (GameObject)Resources.Load("Prefab/EnemyProjectile");
        player = GameObject.FindWithTag("Player1");
        //StartCoroutine(ShootAtPlayer());
    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter(Collision other) {
        base.OnCollisionEnter(other);
    }

    public IEnumerator ShootAtPlayer() {
        yield return new WaitForSeconds(shotDelay);
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = (player.transform.position - gameObject.transform.position).normalized * shotSpeed;
        StartCoroutine(ShootAtPlayer());
    }
    
}
