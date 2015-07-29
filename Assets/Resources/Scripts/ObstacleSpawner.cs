using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour {
    public bool isActive;
    public GameObject player1;
    public GameObject player2;
    public GameObject obstacle;
    public GameObject roof;
    public List<GameObject> roofObjects;
    public List<GameObject> obstacleObjects;
    // is in [-1, 1]
    public float movingPointX;
    float cameraWidth;
    float worldMovePointX;
    GameObject farthestPlayer;
    float worldVelocityX;
    Rigidbody player1RigidBody;
    Rigidbody player2RigidBody;
    Vector3 prevWorldVelocity;
    Vector3 worldVelocity;

    // Use this for initialization
    void Start() {
        player1RigidBody = player1.GetComponent<Rigidbody>();
        player2RigidBody = player2.GetComponent<Rigidbody>();
        cameraWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
        worldMovePointX = movingPointX * cameraWidth;
    }

    // Update is called once per frame
    void Update() {
        if (player1.transform.position.x > worldMovePointX || player2.transform.position.x > worldMovePointX) {
            farthestPlayer = FindFarthestPlayer();
			Debug.Log (farthestPlayer.ToString());
            worldVelocityX = FindWorldVelocity(farthestPlayer.transform.position.x);
            worldVelocity = new Vector3(-worldVelocityX, 0, 0);
            //Debug.Log (worldObjects.Length);
        }
        else {
            worldVelocity = Vector3.zero;
        }  
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

    }

    GameObject FindFarthestPlayer() {
        if (player1.transform.position.x > player2.transform.position.x) {
            return player1;
        }
        else return player2;
    }

    float FindWorldVelocity(float position) {
        //return .8f * Mathf.Pow((0.32193f * (position - 4.445f)), 5) + 6;
        return Mathf.Pow(2, .9f * (position - 1));
        //Debug.Log(position);
        //return position;
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
