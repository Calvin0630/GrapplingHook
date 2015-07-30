using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public string playerNum;
    public float movementSpeed;
    public GameObject hook;
    public float jumpForce;
    public float firePower;
    LineRenderer playerToHook;
    GameObject hookInstance;
    Rigidbody rBody;
    public bool isGrounded;
	public bool isJumping;
    public float forceOfHookOnPlayer;
	public bool hasJetpack;
	// Use this for initialization
	void Start () {
        playerToHook = gameObject.GetComponent<LineRenderer>();
        rBody = gameObject.GetComponent<Rigidbody>();
        isGrounded = false;
	}

	// Update is called once per frame
	void Update () {
        //if (playerNum == "Player1") Debug.Log("Velocity: " + rBody.velocity.x + " isGrounded: " + isGrounded);

		//if the player is on the ground
        if (isGrounded) {
            rBody.velocity = new Vector3(Input.GetAxis("Horizontal" + playerNum) * movementSpeed, rBody.velocity.y, 0);
			// jumping controls
			if (Input.GetAxis("Jump" + playerNum) > .7f) {
				rBody.AddForce(new Vector3(0, jumpForce, 0));
			}
        }

        //hook shooting
	    if (Input.GetAxis("Fire" + playerNum) >.7f) {
            if (hookInstance != null) Destroy(hookInstance);
            Vector3 fireDir;
            if (playerNum == "Player1") fireDir = FindFireDirMouse();

            else if (playerNum == "Player2") fireDir = FindFireDirJoystick();
            else {
                fireDir = Vector3.zero;
                Debug.Log("fireDir is zero, check player.cs");
            }
            hookInstance = (GameObject)GameObject.Instantiate(hook, transform.position, Quaternion.identity);
            Rigidbody hookRigidBody = hookInstance.GetComponent<Rigidbody> ();
            hookRigidBody.velocity = firePower * fireDir;
        }

        if (Input.GetAxis("CancelShot" + playerNum) > .7f) {
            if (hookInstance != null) Destroy(hookInstance);
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
        else  {
            playerToHook.SetVertexCount(0);
        }

        //Jetpack controls
		if (hasJetpack && Input.GetAxis ("PowerUp" + playerNum) > .7f) {
			Debug.Log("PowerUp!");
			gameObject.GetComponent<Rigidbody>().AddForce();
		}
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
