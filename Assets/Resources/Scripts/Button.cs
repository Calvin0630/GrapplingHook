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
        ScoreManager.gameIsOver = false;
    }

    public void LoadTutorial() {
        Application.LoadLevel("SinglePlayerTutorial");
    }
    public void ExitGame() {
        Application.Quit();
    }

    public void SubmitName() {
        inField = GameObject.FindWithTag("NameText");
        ScoreManager.highScores.AddScore(new Score( (int)ObstacleSpawner.distanceTravelled, (int) Time.timeSinceLevelLoad, (int)ObstacleSpawner.maxSpeed, inField.GetComponent<Text>().text));
        Application.LoadLevel("SinglePlayerGameOver");
    }

}
