using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
    GameObject scoreManager;

	// Use this for initialization
	void Start () { 
        scoreManager = (GameObject) Resources.Load("Prefab/ScoreManager");
        GameObject managerInstance;
        if (GameObject.Find("ScoreManager") == null) managerInstance = (GameObject) Instantiate(scoreManager, Vector3.zero, Quaternion.identity);
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
