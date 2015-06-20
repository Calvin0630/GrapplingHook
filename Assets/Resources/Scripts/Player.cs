using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public GameObject hook;
    public float firePower = 1.0f;
    LineRenderer playerToHook;
    GameObject hookInstance;
    Rigidbody rBody;
    public float forceOfHookOnPlayer;
    public bool hooked;
    float initDistPlayerToHook;
	// Use this for initialization
	void Start () {
        playerToHook = gameObject.GetComponent<LineRenderer>();
        rBody = gameObject.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
        //hook shooting
	    if (Input.GetMouseButtonDown(0)) {
            if (hookInstance != null) Destroy(hookInstance);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 fireDir = (mousePosition - transform.position);
            fireDir = SetZToZero(fireDir);
            fireDir = fireDir.normalized;
            hookInstance = (GameObject)GameObject.Instantiate(hook, transform.position, Quaternion.identity);
            Rigidbody hookRigidBody = hookInstance.GetComponent<Rigidbody> ();
            hookRigidBody.velocity = firePower * fireDir;
        }

        if (Input.GetMouseButtonDown(1)) {
            if (hookInstance != null) Destroy(hookInstance);
        }
        
        
        if (hookInstance != null) {
            //updates line renderer
            if (hooked) {
                initDistPlayerToHook = (hookInstance.transform.position - gameObject.transform.position).magnitude;
                hooked = false;
            }
            playerToHook.SetVertexCount(2);
            playerToHook.SetPosition(0, transform.position);
            playerToHook.SetPosition(1, hookInstance.transform.position);
            // moves player towards hook if hook is attatched to wall
            Vector3 PlayerToHook = hookInstance.transform.position - gameObject.transform.position;
            if (hookInstance.GetComponent<Rigidbody>().velocity == Vector3.zero) {
                float alpha = (hookInstance.transform.position - gameObject.transform.position).magnitude / initDistPlayerToHook;
                forceOfHookOnPlayer = Mathf.SmoothStep(0, 50, alpha);
                rBody.AddForce(forceOfHookOnPlayer * (hookInstance.transform.position - gameObject.transform.position).normalized);
            }
        }
        else  {
            playerToHook.SetVertexCount(0);
        }
        
	}

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Wall") {
            Debug.Log("Game Over!");
        }
    }

    public Vector3 GetUnitVector(Vector3 v) {
        return v / v.magnitude;
    }

    public Vector3 SetZToZero(Vector3 v) {
        return new Vector3(v.x, v.y, 0);
    }
    

}
