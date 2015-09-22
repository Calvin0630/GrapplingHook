﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    static string playerNum;
    public float movementSpeed;
    GameObject hook;
    Material material;
    public float jumpForce;
    public float firePower;
    LineRenderer playerToHook;
    GameObject hookInstance;
    GameObject projectile;
    GameObject scoreManager;
    Rigidbody rBody;
    public bool isGrounded;
    public bool isJumping;
    public float forceOfHookOnPlayer;
    public bool hasJetpack;
    public bool useController;
    public float projectileDelay;
    bool projectileTriggerDown;
    float projectileTimer;
    GameObject healthBar;
    GameObject shield;
    GameObject shieldInstance;
    float shieldPower;
    //needed because controller toggles shield whereas keyboard button triggers it
    bool playerIsUsingShield = false;
    public int initialHealth;
    int health;
    ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;
    // Use this for initialization
    void Start() {
        playerNum = gameObject.name;
        if (useController) playerNum = "Player2";
        material = gameObject.GetComponent<MeshRenderer>().material;
        playerToHook = gameObject.GetComponent<LineRenderer>();
        hook = (GameObject)Resources.Load("Prefab/Hook");
        projectile = (GameObject)Resources.Load("Prefab/FriendlyProjectile");
        shield = (GameObject)Resources.Load("Prefab/Shield");
        healthBar = GameObject.FindGameObjectWithTag("HealthBar" + playerNum);
        rBody = gameObject.GetComponent<Rigidbody>();
        isGrounded = false;
        projectileTimer = projectileDelay;
        projectileTriggerDown = false;
        shieldPower = 100;
        health = initialHealth;
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        //particle trail and shit
        if ((-rBody.velocity + ObstacleSpawner.worldVelocity).magnitude < 5) particleSystem.enableEmission = false;
        else particleSystem.enableEmission = true;
        InitializeParticleSystem();
        particles[0].velocity = new Vector3(100, 100, 100);
        int numParticlesAlive = particleSystem.GetParticles(particles);
        for(int i=0;i<numParticlesAlive;i++) {
            particles[i].velocity = -rBody.velocity + ObstacleSpawner.worldVelocity;
            particles[i].color = material.color;
        }
        particleSystem.SetParticles(particles, numParticlesAlive);
        //hook stuff n shit
        if (hookInstance != null) {
            //updates line renderer
            playerToHook.SetVertexCount(2);
            playerToHook.SetPosition(0, transform.position);
            playerToHook.SetPosition(1, hookInstance.transform.position);
            // moves player towards hook if hook is attatched to wall
            Vector3 PlayerToHook = hookInstance.transform.position - gameObject.transform.position;
        }
        else {
            playerToHook.SetVertexCount(0);
        }
        
    }
    //called every physics iteration
    void FixedUpdate() {
        if (scoreManager == null) scoreManager = GameObject.FindWithTag("ScoreManager");
        if (!ScoreManager.gameIsOver) {
            projectileTimer += Time.deltaTime;
            //if the player is on the ground
            if (isGrounded) {
                //moving controls
                rBody.AddForce(new Vector3(Input.GetAxis("Horizontal" + playerNum) * movementSpeed, 0, 0));
                // jumping controls
                if (Input.GetAxis("Jump" + playerNum) > .7f) {
                    rBody.AddForce(new Vector3(0, jumpForce, 0));
                }
            }

            //hook shooting
            if (Input.GetAxis("FireHook" + playerNum) > .7f && hookInstance == null) {
                if (hookInstance != null) Destroy(hookInstance);
                Vector3 fireDir = Vector3.zero;
                if (playerNum == "Player1") fireDir = FindFireDirMouse();
                else if (playerNum == "Player2") fireDir = FindFireDirJoystick();

                hookInstance = (GameObject)GameObject.Instantiate(hook, transform.position, Quaternion.identity);
                Rigidbody hookRigidBody = hookInstance.GetComponent<Rigidbody>();
                hookRigidBody.velocity = firePower * fireDir;
            }
            if (Input.GetAxis("FireHook" + playerNum) < .1f) {
                if (hookInstance != null) Destroy(hookInstance);
            }

            //controls for shooting
            if (Input.GetAxis("FireShot" + playerNum) > .7f && !projectileTriggerDown) {
                projectileTimer = 0;
                Vector3 shotDir = Vector3.zero;
                if (playerNum == "Player2" || useController) shotDir = FindFireDirJoystick();
                else if (playerNum == "Player1") shotDir = FindFireDirMouse();
                //shotDir = Vector3.left;
                GameObject shot = (GameObject)Instantiate(projectile, gameObject.transform.position, Quaternion.identity);
                shot.GetComponent<Rigidbody>().velocity = firePower * .5f * shotDir;
                projectileTriggerDown = true;
            }
            else if (Input.GetAxis("FireShot" + playerNum) < .7f) {
                projectileTriggerDown = false;
            }

            //controls for shield
            //print(0.019f);
            if (Input.GetButtonDown("Shield" + playerNum)) print("X");
            ShieldBar.SetValue(shieldPower/100);
            if (playerNum == "Player 1") {
                playerIsUsingShield = Input.GetButton("Shield" + playerNum);
            }
            else if (playerNum == "Player2") {
                if (Input.GetButtonDown("Shield" + playerNum)) {
                    playerIsUsingShield = !playerIsUsingShield;
                }
            }
            //print(playerIsUsingShield);

            if (playerIsUsingShield && shieldPower >= 10 && shieldInstance == null) {
                //creates the shield
                Time.timeScale = .2f;   
                shieldInstance = (GameObject)Instantiate(shield);
                shieldInstance.transform.position = gameObject.transform.position;
                shieldInstance.transform.SetParent(gameObject.transform);
                shieldInstance.transform.localScale = Vector3.one * (1.1f + (shieldPower / 100) * 1.9f);
            }
            if (!ScoreManager.gameIsOver && (shieldPower <= 0 || !playerIsUsingShield)) {
                Time.timeScale = 1;
                Destroy(shieldInstance);
                if (shieldPower < 100) {
                    shieldPower += 1;
                }
            }
            if (playerIsUsingShield && shieldPower > 0) {
                Time.timeScale = .25f;
                shieldPower -= 1f;
                if (shieldInstance != null) shieldInstance.transform.localScale = Vector3.one * (1.1f + (shieldPower / 100) * 1.9f) ;
            }
            /*
            if (shieldPower <= 0) {
                Destroy(shieldInstance);
                Time.timeScale = 1;
            }*/

            if (hookInstance != null && hookInstance.GetComponent<Hook>().hooked) {
                // moves player towards hook if hook is attatched to wall
                Vector3 PlayerToHook = hookInstance.transform.position - gameObject.transform.position;
                rBody.AddForce(forceOfHookOnPlayer * (hookInstance.transform.position - gameObject.transform.position).normalized);
            }
        }
    }

    public void TakeDamage(string target, int damage) {
        if (target == playerNum) {
            health -= damage;
            healthBar.GetComponent<EnergyBar>().SetValue(100 * health/initialHealth);
            if (health <= 0) {
                ScoreManager.GameOver();
            }
        }
    }

    void OnCollisionEnter(Collision other) {

    }

    Vector3 FindFireDirJoystick() {
        return new Vector3(Input.GetAxis("RStickX"), -Input.GetAxis("RStickY"), 0).normalized;
    }

    Vector3 FindFireDirMouse() {
        return SetZToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position).normalized;
    }

    public Vector3 SetZToZero(Vector3 v) {
        return new Vector3(v.x, v.y, 0);
    }
        
    public bool HasShield() {
        bool hasShield = false;
        foreach(Component c in gameObject.GetComponentsInChildren<Component>()) {
            if (c.gameObject.tag == "Shield") hasShield = true;
        }
        return hasShield;
    }

    public void InitializeParticleSystem() {
        if (particles == null || particles.Length < particleSystem.maxParticles) {
            particles = new ParticleSystem.Particle[particleSystem.maxParticles];
        }
    }
}
