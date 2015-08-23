using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
    GameObject scoreManager;

	// Use this for initialization
	void Start () { 
        scoreManager = (GameObject) Resources.Load("Prefab/ScoreManager");
        Time.timeScale = 1;
        if (GameObject.FindWithTag("ScoreManager") == null) {
            GameObject scoreMgrTmp = (GameObject)Instantiate(scoreManager);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
