using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
    public GameObject obstacle;
    public GameObject roof;
	public GameObject[] worldObjects;
	// is in [-1, 1]
	public float movingPointX;
	float cameraWidth;
	float worldMovePointX;
    GameObject farthestPlayer;
    float worldVelocityX;

    // Use this for initialization
	void Start () {
		cameraWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
		worldMovePointX = movingPointX * cameraWidth;
	}
	
	// Update is called once per frame
    void Update() {
		if (player1.transform.position.x > worldMovePointX || player2.transform.position.x > worldMovePointX) {
            farthestPlayer = FindFarthestPlayer();
			worldVelocityX = FindWorldVelocity(farthestPlayer.transform.position.x);
			//Debug.Log (worldObjects.Length);
			for (int i=0; i<worldObjects.Length;i++) {
				if (worldObjects[i] != null) {
					worldObjects[i].GetComponent<Rigidbody>().velocity = new Vector3(-worldVelocityX, 0, 0);
				}
			}
		}

        if (worldObjects[0].transform.position.x < -1.1) {
            //make new roof object w/ left edge @ right edge of camera 
			Debug.Log ("Its less");
            GameObject clone = (GameObject)Instantiate(roof, new Vector3(18.89f, 5, 0), Quaternion.identity);
			clone.GetComponent<Rigidbody>().velocity = new Vector3(-worldVelocityX, 0, 0);
            worldObjects[1] = clone;
        }
        
	}

    GameObject FindFarthestPlayer() {
        if (player1.transform.position.x > player2.transform.position.x) {
            return player1;
        }
        else return player2;
    }
    
	float FindWorldVelocity(float position) {
		return Mathf.Pow ((0.38667f * (position - 4.445f)), 5) + 15;
	}
}
