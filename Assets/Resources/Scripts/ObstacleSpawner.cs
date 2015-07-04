using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {
    public GameObject wall;
	public bool isActive;
    public float levelUpDelay;
    public float initialSpawnDelay;
	public float minSpawnDelay;
	float delay;
    public float deltaSpawnDelayPerLevel;
    public float ySpeed;
    public float yScale;
	Vector3 TopRightEdgeOfCamera;
	float widthOfValley = 13.28f;
	float centerOfValleyX = -1.25f;
	// Use this for initialization
	void Start () {
		TopRightEdgeOfCamera = -Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0));
		delay = initialSpawnDelay;
		if (isActive) {
			StartCoroutine (SpawnCube ());
			StartCoroutine (LevelUp ());
		}
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
        float width = Random.Range(4, 8);
		float leftOrRight = Random.Range (1, 10) % 2;
		float xPos = widthOfValley/2 -width/2;
		if (leftOrRight == 1)
			xPos *= -1;
		xPos += centerOfValleyX;
        GameObject clone = (GameObject) Instantiate(wall, new Vector3(xPos, 2*TopRightEdgeOfCamera.y, 0) , Quaternion.identity);
		clone.transform.localScale = new Vector3 (width, yScale, 1);
		clone.GetComponent<Rigidbody> ().velocity = new Vector3 (0, -ySpeed, 0);

    }
}
