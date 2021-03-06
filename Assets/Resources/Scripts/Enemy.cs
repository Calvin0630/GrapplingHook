﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int health;
    public int initialHealth;
    GameObject healthBarPrefab;
    public GameObject healthBar;
    public bool isHooked = false;
    Vector3 cameraSize;

    // Use this for initialization
    public void Start () {
        cameraSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        InstantiateHealthBar();
	}

    // Update is called once per frame
    void Update() {

    }

    public void InstantiateHealthBar() {
        //if (health == 0) health = 1;
        health = initialHealth;
        healthBarPrefab = (GameObject)Resources.Load("Prefab/UI/EnemyHealth/EnemyHealthBar");
        healthBar = (GameObject)Instantiate(healthBarPrefab, 40 * Vector3.left, Quaternion.identity);
        healthBar.GetComponent<EnemyHealthBar>().enemy = gameObject;
        healthBar.transform.parent = GameObject.Find("Canvas").transform;
    }



    void OnDestroy() {
        Destroy(healthBar);
    }

    public void TakeDamage(int damage) {
        health -= damage;
        float fhealth = (float)health;
        float fInitialHealth = (float)initialHealth;
        healthBar.GetComponent<EnemyHealthBar>().SetValue((int)(fhealth / fInitialHealth * 100));
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "FriendlyProjectile") {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Player1") {
            if (other.gameObject.GetComponent<Player>().HasShield()) Destroy(gameObject);
        }
    }
}
