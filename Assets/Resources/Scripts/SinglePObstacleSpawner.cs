using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SinglePObstacleSpawner : MonoBehaviour {
	public bool isActive;
	GameObject player1;
	GameObject obstacle;
	GameObject roof;
	public List<GameObject> roofObjects;
	public List<GameObject> obstacleObjects;
	// is in [-1, 1]
	public float movingPointX;
	float cameraWidth;
	float worldMovePointX;
	GameObject farthestPlayer;
	float worldVelocityX;
	Rigidbody player1RigidBody;
	Vector3 prevWorldVelocity;
    Vector3 worldVelocity;
    GameObject[] tmp;
	
	// Use this for initialization
    void Start() {
        player1 = GameObject.Find("Player1");
		roof = (GameObject) Resources.Load ("Prefab/Roof");
		obstacle = (GameObject) Resources.Load ("Prefab/Obstacle");
		player1RigidBody = player1.GetComponent<Rigidbody>();
		cameraWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
		worldMovePointX = movingPointX * cameraWidth;
        //initiates lists
        roofObjects = new List<GameObject>();
        obstacleObjects = new List<GameObject>();
        //finds roof objects in scene, and adds them to the array
        tmp = GameObject.FindGameObjectsWithTag("Roof");
        for (int i = 0; i < tmp.Length; i++) roofObjects.Add(tmp[i]);
        //finds obstacle objects in scene, and adds them to the obstacleList, and sorts them
        tmp = GameObject.FindGameObjectsWithTag("Obstacle");
		for (int i = 0; i < tmp.Length; i++) obstacleObjects.Add(tmp[i]);
		obstacleObjects.Sort ((x,y) => x.transform.position.x.CompareTo(y.transform.position.x));
	}
	
	// Update is called once per frame
	void Update() {
		FindWorldVelocity ();
		if (isActive) {
			MoveWorld ();
			//moves player1
			if (player1.transform.position.x > worldMovePointX) player1RigidBody.velocity = player1RigidBody.velocity + worldVelocity - prevWorldVelocity;
			prevWorldVelocity = worldVelocity;
		}
		MakeRoofObjects ();
		DeleteRoofObjects ();
		DeleteObstacleObjects ();
	}
	


	//find world velocity
	void FindWorldVelocity() {
		if (player1.transform.position.x > worldMovePointX) {
			farthestPlayer = player1;
			worldVelocityX = CalculateWorldVelocity(farthestPlayer.transform.position.x);
			worldVelocity = new Vector3(-worldVelocityX, 0, 0);
			//Debug.Log (worldObjects.Length);
		}
		else {
			worldVelocity = Vector3.zero;
		}  
	}
	
	//calculates world velocity given the farthest players position
	float CalculateWorldVelocity(float position) {
		return Mathf.Pow(2, .9f * (position + 5));
	}
	
	void MakeRoofObjects() {
		if (roofObjects[roofObjects.Count - 1].transform.position.x < 3.89) {
			//make new roof object w/ left edge @ right edge of camera 
			GameObject clone = (GameObject)Instantiate(roof, new Vector3(18.89f, 5, 0), Quaternion.identity);
			clone.GetComponent<Rigidbody>().velocity = new Vector3(-worldVelocityX, 0, 0);
			clone.transform.localScale = new Vector3(20, 2, 1);
			roofObjects.Add(clone);
		}
	}
	
	void DeleteRoofObjects() {
		if (roofObjects [0].transform.position.x < -40) {
			Destroy (roofObjects [0]);
			roofObjects.RemoveAt (0);
		}
	}
	void DeleteObstacleObjects() {
		if (obstacleObjects.Count > 0) {
			if (obstacleObjects [0].transform.position.x < -40) {
				Destroy (obstacleObjects [0]);
				obstacleObjects.RemoveAt (0);
			}
		}
	}
	
	void MoveWorld() {
		//moves obstacles
		for (int i = 0; i < obstacleObjects.Count; i++) {
			if (obstacleObjects[i] != null) {
				obstacleObjects[i].GetComponent<Rigidbody>().velocity = worldVelocity;
			}
		}
		//moves roofs
		for (int i = 0; i < roofObjects.Count; i++) {
			if (roofObjects[i] != null) {
				roofObjects[i].GetComponent<Rigidbody>().velocity = worldVelocity;
			}
		}
	}
}
