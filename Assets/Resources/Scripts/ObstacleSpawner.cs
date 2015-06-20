using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {
    public GameObject wall;
    public float levelUpDelay;
    public float initialSpawnDelay;
    public float deltaSpawnDelayPerLevel;
	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnCube(5));
	}
	
	// Update is called once per frame
    void Update() {
        if (Time.time < levelUpDelay){

        }
	}

    IEnumerator SpawnCube(float delay) {
        while (true) {
            Debug.Log("Hello");
            yield return new WaitForSeconds(delay);
        }
    }

    public void CreateCube() {
        float width = Random.Range(2, 5);
        GameObject clone = (GameObject)Instantiate(wall, new Vector3(width, 10, 0) , Quaternion.identity);
    }
}
