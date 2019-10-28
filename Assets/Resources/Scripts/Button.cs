using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour {
    GameObject scoreManager;
    GameObject inField;
    GameObject spawner;

    void Start() {

    }

    void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }
	public void LoadMultiplayer() {
		SceneManager.LoadScene ("FirstLevel");
	}

    public void LoadSinglePlayer() {
        SceneManager.LoadScene("SinglePlayer");
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("MainScreen");
    }

    public void LoadOptions() {
        SceneManager.LoadScene("Options");
    }
    public void LoadControls() {
        SceneManager.LoadScene("Controls");
    }

    public void Replay() {
        ObstacleSpawner.distanceTravelled = 0;
        SceneManager.LoadScene(Application.loadedLevel);
        ScoreManager.gameIsOver = false;
    }

    public void LoadTutorial() {
        //SceneManager.LoadScene("TutorialGUI");
        SceneManager.LoadScene("SinglePlayerTutorial");
    }
    public void ExitGame() {
        Application.Quit();
    }

    public void LoadMellow() {
        SceneManager.LoadScene("SinglePlayerMellow");
    }
    

    public void SubmitName() {
        inField = GameObject.FindWithTag("NameText");
        ScoreManager.highScores.AddScore(new Score( (int)ObstacleSpawner.distanceTravelled, (int) Time.timeSinceLevelLoad, (int)ObstacleSpawner.maxSpeed, inField.GetComponent<Text>().text));
        ObstacleSpawner.distanceTravelled = 0;
        SceneManager.LoadScene("SinglePlayerGameOver");
    }

}
