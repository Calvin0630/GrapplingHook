using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour {
	public GameObject hook;
	public float firePower = 1.0f;
	LineRenderer playerToHook;
	GameObject hookInstance;
	Rigidbody rBody;
	public float forceOfHookOnPlayer;
	bool hooked;
	float initDistPlayerToHook;
	Vector3 LStick;
	// Use this for initialization
	void Start () {
		playerToHook = gameObject.GetComponent<LineRenderer>();
		rBody = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		LStick = new Vector3 (Input.GetAxis ("LStickX"), -Input.GetAxis ("LStickY"), 0);
		Debug.Log (LStick);
		//hook shooting
		if (Input.GetAxis("RT") > .7f && LStick.magnitude > .7f) {
			if (hookInstance != null) Destroy(hookInstance);
			Vector3 fireDir = LStick.normalized;
			hookInstance = (GameObject)GameObject.Instantiate(hook, transform.position, Quaternion.identity);
			Rigidbody hookRigidBody = hookInstance.GetComponent<Rigidbody> ();
			hookRigidBody.velocity = firePower * fireDir;
		}
		
		if (Input.GetAxis("LT") > .7f) {
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
		
	}
	
	public Vector3 GetUnitVector(Vector3 v) {
		return v / v.magnitude;
	}
	
	public Vector3 SetZToZero(Vector3 v) {
		return new Vector3(v.x, v.y, 0);
	}
	
	
}