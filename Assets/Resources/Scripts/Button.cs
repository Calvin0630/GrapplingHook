using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button : MonoBehaviour {
    GameObject scoreManager;
    GameObject inField;

    void Start() {
    }
	public void LoadMultiplayer() {
		Application.LoadLevel ("FirstLevel");
	}

    public void LoadSinglePlayer() {
        Application.LoadLevel("SinglePlayer");
    }

    public void SubmitName() {
        scoreManager = GameObject.Find("ScoreManager(Clone)");
        Debug.Log(scoreManager == null);
        inField = GameObject.Find("NameInputField");
        scoreManager.GetComponent<ScoreManager>().AddScore(inField.GetComponentInChildren<Text>().text);
        Application.LoadLevel("SinglePlayerGameOver");
    }

}
