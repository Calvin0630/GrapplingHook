using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    GameObject healthBarPrefab;
    public int shotDelay;
    GameObject projectile;
    GameObject target;

	void Start () {
        healthBarPrefab = (GameObject) Resources.Load("Prefab/UI/EnemyHealth/EnemyHealthBar");
        GameObject healthBar = (GameObject)Instantiate(healthBarPrefab, 40 * Vector3.left, Quaternion.identity);
        healthBar.GetComponent<EnemyHealthBar>().enemy = gameObject;
        healthBar.transform.parent = GameObject.Find("Canvas").transform;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
