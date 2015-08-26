using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    string playerNum;
    public float movementSpeed;
    GameObject hook;
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
    GameObject shieldBar;
    GameObject shield;
    GameObject shieldInstance;
    float shieldPower;
    // Use this for initialization
    void Start() {
        playerNum = gameObject.name;
        if (useController) playerNum = "Player2";
        playerToHook = gameObject.GetComponent<LineRenderer>();
        hook = (GameObject)Resources.Load("Prefab/Hook");
        projectile = (GameObject)Resources.Load("Prefab/FriendlyProjectile");
        shield = (GameObject)Resources.Load("Prefab/Shield");
        shieldBar = GameObject.FindGameObjectWithTag("ShieldBar" + playerNum);
        rBody = gameObject.GetComponent<Rigidbody>();
        isGrounded = false;
        projectileTimer = projectileDelay;
        projectileTriggerDown = false;
        shieldPower = 100;
    }

    // Update is called once per frame
    void Update() {
        if (scoreManager == null) scoreManager = GameObject.FindWithTag("ScoreManager");
        if (!scoreManager.GetComponent<ScoreManager>().gameIsOver) {
            projectileTimer += Time.deltaTime;
            //if the player is on the ground
            if (isGrounded) {
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
            //Debug.Log(shieldPower);
            shieldBar.GetComponent<EnergyBar>().SetValue((int)shieldPower);
            if (Input.GetButtonDown("Shield" + playerNum) && shieldPower >= 0 && shieldPower <= 101) {
                shieldInstance = (GameObject)Instantiate(shield);
                shieldInstance.transform.position = gameObject.transform.position;
                shieldInstance.transform.SetParent(gameObject.transform);
            }
            if (Input.GetButton("Shield" + playerNum) && shieldPower > 0) {
                Time.timeScale = .25f;
                shieldPower -= .75f;
            }
            else if (!scoreManager.GetComponent<ScoreManager>().gameIsOver && shieldPower >= -1) {
                Time.timeScale = 1;
                Destroy(shieldInstance);
                if (shieldPower < 100) {
                    shieldPower += 1;
                }

            }

            if (hookInstance != null) {
                //updates line renderer
                playerToHook.SetVertexCount(2);
                playerToHook.SetPosition(0, transform.position);
                playerToHook.SetPosition(1, hookInstance.transform.position);
                // moves player towards hook if hook is attatched to wall
                Vector3 PlayerToHook = hookInstance.transform.position - gameObject.transform.position;
                if (hookInstance.GetComponent<Hook>().hooked) {

                    rBody.AddForce(forceOfHookOnPlayer * (hookInstance.transform.position - gameObject.transform.position).normalized);
                }
            }
            else {
                playerToHook.SetVertexCount(0);
            }
        }
    }

    void FixedUpdate() {
    }

    void OnCollisionEnter(Collision col) {

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


}
