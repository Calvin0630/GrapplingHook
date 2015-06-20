using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {
    public GameObject wall;
    public float levelUpDelay;
    public float initialSpawnDelay;
	public float minSpawnDelay;
	float delay;
    public float deltaSpawnDelayPerLevel;
	Vector3 TopRightEdgeOfCamera;
	// Use this for initialization
	void Start () {
		TopRightEdgeOfCamera = -Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0));
		delay = initialSpawnDelay;
		StartCoroutine(SpawnCube());
		StartCoroutine(LevelUp());
	}
	
	// Update is called once per frame
    void Update() {
        if (Time.time < levelUpDelay){

        }
	}

	IEnumerator LevelUp() {
		while (true) {
			yield return new WaitForSeconds(levelUpDelay);
			if (delay > minSpawnDelay) {
				delay -= deltaSpawnDelayPerLevel;
			}
		}
	}

    IEnumerator SpawnCube() {
        while (true) {
			CreateCube ();
            yield return new WaitForSeconds(delay);
        }
    }

    public void CreateCube() {
        float width = Random.Range(5, 10);
		float leftOrRight = Random.Range (0, 100) % 2;
		float xPos = TopRightEdgeOfCamera.x -width/2 - 1;
		if (leftOrRight == 1)
			xPos *= -1;
        GameObject clone = (GameObject) Instantiate(wall, new Vector3(xPos, 2*TopRightEdgeOfCamera.y, 0) , Quaternion.identity);
		clone.transform.localScale = new Vector3 (width, 2, 1);
		clone.GetComponent<Rigidbody> ().velocity = new Vector3 (0, -5, 0);
		Debug.Log (delay);

    }
}
