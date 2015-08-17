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
	// Use this for initialization

	void Start () {
        highScores = new List<Score>();
        scoreBox = (GameObject) Resources.Load("Prefab/UI/ScoreText");
        DontDestroyOnLoad(gameObject);
        
        for (int i=0;i<10;i++) {
            highScores.Add(new Score());
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddScore(Score score) {
        highScores.Add(score);
        highScores.OrderBy(x => x.distanceTraveled).ToList();     
    }

    public string ToString() {
        string result = "";
        for (int i=0;i<highScores.Count;i++) {
            if (highScores[i] != null) {
                result += "distance: " + highScores[i].distanceTraveled + " , " + "time: " + highScores[i].timeSurvived + " , " + "kills: " + highScores[i].enemysKilled + " \n ";
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
}
