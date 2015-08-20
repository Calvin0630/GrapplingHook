using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button : MonoBehaviour {
    GameObject scoreManager;
    GameObject inField;
    GameObject spawner;

    void Start() {
        Object[] test = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
        for(int i=0;i<test.Length;i++) {
            Debug.Log(test[i].name);
        }
        Debug.Log(test.Length);
    }

    void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }
	public void LoadMultiplayer() {
		Application.LoadLevel ("FirstLevel");
	}

    public void LoadSinglePlayer() {
        Application.LoadLevel("SinglePlayer");
    }

    public void SubmitName() {
        scoreManager = GameObject.Find("ScoreManager(Clone)");
        spawner = GameObject.Find("ObstacleSpawner");
        inField = GameObject.Find("NameText");
        scoreManager.GetComponent<ScoreManager>().AddScore(new Score((int)spawner.GetComponent<ObstacleSpawner>().distanceTravelled, (int) Time.timeSinceLevelLoad, inField.GetComponentInChildren<Text>().text));
        Application.LoadLevel("SinglePlayerGameOver");
    }

}
