using UnityEngine;
using System.Collections;
using System.IO;

public class StartGame : MonoBehaviour {
    GameObject scoreManager;
    GameObject buttonManager;
    GameObject obstacleSpawner;
    GameObject scoreMgrTmp;
    GameObject buttonMgrTmp;
    GameObject spawnerTmp;


    // Use this for initialization
    //called before start
    void Awake() {
        scoreManager = (GameObject)Resources.Load("Prefab/ScoreManager");
        buttonManager = (GameObject)Resources.Load("Prefab/ButtonManager");
        obstacleSpawner = (GameObject)Resources.Load("Prefab/ObstacleSpawner");
        Time.timeScale = 1;

        if (GameObject.FindWithTag("ScoreManager") == null) {
            scoreMgrTmp = Instantiate(scoreManager);
        }
        else {
            scoreMgrTmp = GameObject.FindWithTag("ScoreManager");
        }
        if (GameObject.FindWithTag("ButtonManager") == null) {
            buttonMgrTmp = (GameObject)Instantiate(buttonManager);
        }
        else {
            buttonMgrTmp = GameObject.FindWithTag("ButtonManager");
        }
        if (GameObject.FindWithTag("Spawn") == null && Application.loadedLevelName != "MainScreen") {
            spawnerTmp = (GameObject)Instantiate(obstacleSpawner);
        }
        else {
            spawnerTmp = GameObject.FindWithTag("Spawn");
        }
    }

    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

	}
}
