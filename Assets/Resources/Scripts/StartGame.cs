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
        if (!File.Exists(ScoreManager.highScorePath)) {
            using (StreamWriter file = new StreamWriter(File.Create(ScoreManager.highScorePath))) ;
        }

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
        if (GameObject.FindWithTag("Spawn") == null) {
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
