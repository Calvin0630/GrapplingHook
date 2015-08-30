using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button : MonoBehaviour {
    GameObject scoreManager;
    GameObject inField;
    GameObject spawner;

    void Start() {
        DontDestroyOnLoad(gameObject);
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

    public void LoadMainMenu() {
        Application.LoadLevel("MainScreen");
    }

    public void LoadOptions() {
        Application.LoadLevel("Options");
    }

    public void Replay() {
        Application.LoadLevel(Application.loadedLevel);
        GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>().gameIsOver = false;
    }

    public void LoadTutorial() {
        Application.LoadLevel("SinglePlayerTutorial");
    }
    public void ExitGame() {
        Application.Quit();
    }

    public void SubmitName() {
        scoreManager = GameObject.FindWithTag("ScoreManager");
        spawner = GameObject.Find("ObstacleSpawner");
        inField = GameObject.Find("NameText");
        ScoreManager.highScores.AddScore(new Score( (int)spawner.GetComponent<ObstacleSpawner>().distanceTravelled, (int) Time.timeSinceLevelLoad, (int)spawner.GetComponent<ObstacleSpawner>().maxSpeed, inField.GetComponentInChildren<Text>().text));
        Application.LoadLevel("SinglePlayerGameOver");
    }

}
