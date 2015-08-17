using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;

public class ScoreManager : MonoBehaviour {
    List<Score> highScores;
    GameObject scoreBox;
    GameObject highScorePanel;
    GameObject gameOverPanel;
    GameObject nameField;
    GameObject spawner;
	// Use this for initialization

	void Start () {
        highScores = new List<Score>();
        scoreBox = (GameObject) Resources.Load("Prefab/UI/ScoreText");
        spawner = GameObject.FindWithTag("Spawn");
        DontDestroyOnLoad(gameObject);
        /*
        for (int i=0;i<10;i++) {
            highScores.Add(new Score());
        }
        */
        gameOverPanel = (GameObject) Resources.Load("Prefab/UI/GameOverPanel");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddScore(Score score) {
        highScores.Add(score);
        highScores.OrderBy(x => -x.distanceTraveled).ToList();     
    }

    public void AddScore(string name) {
        spawner = GameObject.Find("ObstacleSpawner");
        highScores.Add(new Score((int)spawner.GetComponent<ObstacleSpawner>().distanceTravelled, (int)Time.time, name));
        Debug.Log("Hello");
        highScores.OrderBy(x => -x.distanceTraveled).ToList();
    }

    public string ToString() {
        string result = "";
        for (int i=0;i<highScores.Count;i++) {
            if (highScores[i] != null) {
                result += "distance: " + highScores[i].distanceTraveled + " , " + "time: " + highScores[i].timeSurvived + " \n ";
            }
        }
        return result;
    }
    public void PopulateList() {
        highScorePanel = GameObject.FindWithTag("HighScoreList");
        for (int i = 0; i < highScores.Count; i++) {
            if (highScores[i] != null) {
                GameObject scoreText = Instantiate(scoreBox);
                scoreText.GetComponent<Text>().text = " " + (i + 1) + ": " + highScores[i].ToString();
                scoreText.GetComponent<RectTransform>().SetParent(highScorePanel.transform);
                scoreText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }

    

    public void GameOver() {
        Time.timeScale = 0;
        GameObject GG = Instantiate(gameOverPanel);
        GG.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
        //GG.GetComponent<RectTransform>().
        nameField = GameObject.FindWithTag("NameField");
    }

}
