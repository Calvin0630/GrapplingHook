using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float movementSpeed;
    GameObject hook;
    Material material;
    public float jumpForce;
    public float firePower;
    LineRenderer playerToHookRenerer;
    Vector3 hookPoint;
    bool hookIsActive;
    GameObject projectile;
    GameObject scoreManager;
    Rigidbody rBody;
    public bool isGrounded;
    public bool isJumping;
    public float forceOfHookOnPlayer;
    public bool hasJetpack;
    public float projectileDelay;
    bool projectileTriggerDown;
    float projectileTimer;
    GameObject healthBar;
    GameObject shield;
    GameObject shieldInstance;
    float shieldPower;
    //should be 1
    public float normalTimeScale;
    //should be ~.25
    public float slowedTimeScale;
    //needed because controller toggles shield whereas keyboard button triggers it
    bool playerShieldInput = false;
    public int initialHealth;
    int health;
    // Use this for initialization
    void Start() {
        material = gameObject.GetComponent<MeshRenderer>().material;
        playerToHookRenerer = gameObject.GetComponent<LineRenderer>();
        hookIsActive = false;
        projectile = (GameObject)Resources.Load("Prefab/FriendlyProjectile");
        shield = (GameObject)Resources.Load("Prefab/Shield");
        healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        rBody = gameObject.GetComponent<Rigidbody>();
        isGrounded = false;
        projectileTimer = projectileDelay;
        projectileTriggerDown = false;
        shieldPower = 100;
        health = initialHealth;
    }

    // Update is called once per frame
    void Update() {
        //controls whether shield is active based on player input
        ShieldBar.SetValue(shieldPower / 100);

        playerShieldInput = Input.GetButton("Shield");
        //hook stuff n shit
        if (hookIsActive) {
            //updates line renderer
            playerToHookRenerer.SetVertexCount(2);
            playerToHookRenerer.SetPosition(0, transform.position);
            playerToHookRenerer.SetPosition(1,hookPoint);
            // moves player towards hook if hook is attatched to wall
            Vector3 PlayerToHook = hookPoint - gameObject.transform.position;
            GetComponent<Rigidbody>().AddForce(PlayerToHook.normalized* forceOfHookOnPlayer);
        }
        else {
            playerToHookRenerer.SetVertexCount(0);
        }

    }
    //called every physics iteration
    void FixedUpdate() {
        projectileTimer += Time.deltaTime;
        //if the player is on the ground
        if (isGrounded) {
            //moving controls
            rBody.AddForce(new Vector3(Input.GetAxis("Horizontal") * movementSpeed, 0, 0));
            // jumping controls
            if (Input.GetAxis("Jump") > .7f) {
                rBody.AddForce(new Vector3(0, jumpForce, 0));
            }
        }

        //hook shooting
        if (Input.GetButtonDown("FireHook") ) {
            //shoot hook
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 76); ;
            RaycastHit hit;
            Ray ray = new Ray(mousePos, Vector3.forward);
            if(Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Environment"))) {
                hookPoint = hit.point;
                hookIsActive = true;
            }
        }
        if (Input.GetButtonUp("FireHook")) {
            hookIsActive = false;
        }

        //controls for shooting
        if (Input.GetAxis("FireShot") > .7f && !projectileTriggerDown) {
            projectileTimer = 0;
            Vector3 shotDir = Vector3.zero;
            shotDir = FindFireDirMouse();
            //shotDir = Vector3.left;
            GameObject shot = (GameObject)Instantiate(projectile, gameObject.transform.position, Quaternion.identity);
            shot.GetComponent<Rigidbody>().velocity = firePower * .5f * shotDir;
            projectileTriggerDown = true;
        }
        else if (Input.GetAxis("FireShot") < .7f) {
            projectileTriggerDown = false;
        }

        //controls for shield
        //print(0.019f);
        //print(playerIsUsingShield);
        if (playerShieldInput && shieldPower >= 10 && shieldInstance == null) {
            //creates the shield
            Time.timeScale = .2f;
            shieldInstance = (GameObject)Instantiate(shield);
            shieldInstance.transform.position = gameObject.transform.position;
            shieldInstance.transform.SetParent(gameObject.transform);
            shieldInstance.transform.localScale = Vector3.one * (1.1f + (shieldPower / 100) * 1.9f);
        }
        if (!ScoreManager.gameIsOver && (shieldPower <= 0 || !playerShieldInput)) {
            Time.timeScale = normalTimeScale;
            Destroy(shieldInstance);
            if (shieldPower < 100) {
                shieldPower += 1;
            }
        }
        if (playerShieldInput && shieldPower > 0) {
            Time.timeScale = slowedTimeScale;
            shieldPower -= 1f;
            if (shieldInstance != null) shieldInstance.transform.localScale = Vector3.one * (1.1f + (shieldPower / 100) * 1.9f);
        }
        /*
        if (shieldPower <= 0) {
            Destroy(shieldInstance);
            Time.timeScale = 1;
        }*/

        if (hookIsActive) {
            // moves player towards hook if hook is attatched to wall
            Vector3 PlayerToHook = hookPoint - gameObject.transform.position;
            rBody.AddForce(forceOfHookOnPlayer * (hookPoint - gameObject.transform.position).normalized);
        }
    }

    public void TakeDamage( int damage) {
            health -= damage;
            healthBar.GetComponent<EnergyBar>().SetValue(100 * health / initialHealth);
            if (health <= 0) {
                ScoreManager.GameOver();
            }
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
        foreach (Component c in gameObject.GetComponentsInChildren<Component>()) {
            if (c.gameObject.tag == "Shield") hasShield = true;
        }
        return hasShield;
    }
}
