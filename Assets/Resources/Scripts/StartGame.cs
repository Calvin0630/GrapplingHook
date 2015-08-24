using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
    GameObject scoreManager;
    GameObject buttonManager;

	// Use this for initialization
	void Start () { 
        scoreManager = (GameObject) Resources.Load("Prefab/ScoreManager");
        buttonManager = (GameObject)Resources.Load("Prefab/ButtonManager");
        Time.timeScale = 1;

        if (GameObject.FindWithTag("ScoreManager") == null) {
            GameObject scoreMgrTmp = (GameObject)Instantiate(scoreManager);
        }
        if (GameObject.FindWithTag("ButtonManager") == null) {
            GameObject buttonMgrTmp = (GameObject)Instantiate(buttonManager);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
