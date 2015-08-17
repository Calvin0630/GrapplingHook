using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
    GameObject scoreManager;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        scoreManager = (GameObject) Resources.Load("Prefab/ScoreManager");
        GameObject managerInstance;
        if (GameObject.Find("ScoreManager") == null) managerInstance = (GameObject) Instantiate(scoreManager, Vector3.zero, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
