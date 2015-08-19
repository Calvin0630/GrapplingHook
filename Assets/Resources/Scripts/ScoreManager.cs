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
        scoreBox = (GameObject) Resources.Load("Prefab/UI/ScoreBox");
        spawner = GameObject.FindWithTag("Spawn");
        DontDestroyOnLoad(gameObject);
        
        
        gameOverPanel = (GameObject) Resources.Load("Prefab/UI/GameOverPanel");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddScore(Score score) {
        highScores.Add(score);
        highScores.Sort(); 
    }
    /*
    public void AddScore(string name) {
        spawner = GameObject.Find("ObstacleSpawner");
        highScores.Add(new Score((int)spawner.GetComponent<ObstacleSpawner>().distanceTravelled, (int)Time.time, name));
        highScores.OrderByDescending(x => x.distanceTraveled).ToList();
    }
    */
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
                GameObject scoreItem = Instantiate(scoreBox);
                //fils text fields in the highscore box
                Text[] fields = scoreItem.GetComponentsInChildren<Text>();
                foreach (Text text in fields) {
                    if (text.name == "Place") text.text = (i+1) + ":";
                    else if (text.name == "Name") text.text = highScores[i].name;
                    else if (text.name == "Distance") text.text = highScores[i].distanceTraveled + " m";
                    else if (text.name == "Time") text.text = "" + highScores[i].timeSurvived + " s";
                }
                //scoreItem.GetComponent<Text>().text = " " + (i + 1) + ": " + highScores[i].ToString();
                scoreItem.GetComponent<RectTransform>().SetParent(highScorePanel.transform, false);
                //scoreItem.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void GameOver() {
        Time.timeScale = 0;
        GameObject GG = Instantiate(gameOverPanel);
        GG.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
        GameObject congratsText = GameObject.Find("CongradulationsText");
        spawner = GameObject.Find("ObstacleSpawner");
        congratsText.GetComponent<Text>().text = "You made it " + (int)spawner.GetComponent<ObstacleSpawner>().distanceTravelled + " metres!! GG";

        spawner.GetComponent<ObstacleSpawner>().worldVelocity = Vector3.zero;
        //GG.GetComponent<RectTransform>().
        //nameField = GameObject.FindWithTag("NameField");
    }

}
