using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
	public GameObject[] obstacles;
	GameObject farthestPlayer;
	// is in [-1, 1]
	public float movingPointX;
	float cameraWidth;
	float worldMovePointX;

    // Use this for initialization
	void Start () {
		cameraWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
		worldMovePointX = movingPointX * cameraWidth;
	}
	
	// Update is called once per frame
    void Update() {
		if (player1.transform.position.x > worldMovePointX || player2.transform.position.x > worldMovePointX) {
			
		}
        
	}
    
}
