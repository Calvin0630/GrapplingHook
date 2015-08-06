using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiPObstacleSpawner : MonoBehaviour {
    public bool isActive;
    GameObject player1;
    GameObject player2;
    GameObject obstacle;
    GameObject roof;
    List<GameObject> roofObjects;
    List<GameObject> obstacleObjects;
    // is in [-1, 1]
    public float movingPointX;
    //top left corner of world screen
    Vector3 cameraSize;
    float cameraWidth;
    float worldMovePointX;
    GameObject farthestPlayer;
    float worldVelocityX;
    Rigidbody player1RigidBody;
    Rigidbody player2RigidBody;
    Vector3 prevWorldVelocity;
    Vector3 worldVelocity;
    GameObject[] tmp;
    GameObject FramePiece;
	float distanceTraveled = 0;

    // Use this for initialization
    void Start() {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
		roof = (GameObject) Resources.Load ("Prefab/Roof");
		obstacle = (GameObject) Resources.Load ("Prefab/Obstacle");
        //initiates lists
        roofObjects = new List<GameObject>();
        obstacleObjects = new List<GameObject>();
        //finds roof objects in scene, and adds them to the array
        tmp = GameObject.FindGameObjectsWithTag("Roof");
        for (int i = 0; i < tmp.Length; i++) roofObjects.Add(tmp[i]);
        //finds obstacle objects in scene, and adds them to the obstacleList
        tmp = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i < tmp.Length; i++) obstacleObjects.Add(tmp[i]);
        player1RigidBody = player1.GetComponent<Rigidbody>();
        player2RigidBody = player2.GetComponent<Rigidbody>();
        cameraSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        cameraWidth = cameraSize.x;
        worldMovePointX = movingPointX * cameraWidth;
        MakeFrame();
    }

    // Update is called once per frame
    void Update() {
		distanceTraveled += Time.fixedDeltaTime * worldVelocity.x;
		FindWorldVelocity ();
        if (isActive) {
			MoveWorld ();
            //moves player1
			if (player1.transform.position.x > worldMovePointX) player1RigidBody.velocity = player1RigidBody.velocity + worldVelocity - prevWorldVelocity;
            //moves player2
			if (player2.transform.position.x > worldMovePointX) player2RigidBody.velocity = player2RigidBody.velocity + worldVelocity - prevWorldVelocity;
            prevWorldVelocity = worldVelocity;
        }
		MakeRoofObjects ();
		DeleteRoofObjects ();
		DeleteObstacleObjects ();

    }

    //makes the GameOver frame
    void MakeFrame() {

    }

    GameObject FindFarthestPlayer() {
        if (player1.transform.position.x > player2.transform.position.x) {
            return player1;
        }
        else return player2;
    }

	//find world velocity
	void FindWorldVelocity() {
		if (player1.transform.position.x > worldMovePointX || player2.transform.position.x > worldMovePointX) {
			farthestPlayer = FindFarthestPlayer();
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
		if (roofObjects[0].transform.position.x < -40) {
			Destroy(roofObjects[0]);
			roofObjects.RemoveAt(0);
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
