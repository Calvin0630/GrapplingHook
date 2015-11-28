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
        ObstacleSpawner.distanceTravelled = 0;
        Application.LoadLevel(Application.loadedLevel);
        ScoreManager.gameIsOver = false;
    }

    public void LoadTutorial() {
        //Application.LoadLevel("TutorialGUI");
        Application.LoadLevel("SinglePlayerTutorial");
    }
    public void ExitGame() {
        Application.Quit();
    }

    public void LoadMellow() {
        Application.LoadLevel("SinglePlayerMellow");
    }
    

    public void SubmitName() {
        inField = GameObject.FindWithTag("NameText");
        ScoreManager.highScores.AddScore(new Score( (int)ObstacleSpawner.distanceTravelled, (int) Time.timeSinceLevelLoad, (int)ObstacleSpawner.maxSpeed, inField.GetComponent<Text>().text));
        ObstacleSpawner.distanceTravelled = 0;
        Application.LoadLevel("SinglePlayerGameOver");
    }

}
