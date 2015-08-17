﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;

public class ScoreManager : MonoBehaviour {
    List<Score> highScores;
    GUIText scoreBox;
	// Use this for initialization

	void Start () {
        highScores = new List<Score>();
        scoreBox = (GUIText)Resources.Load("Prefab/UIBehaviour/ScoreText");
        /*
        for (int i=0;i<10;i++) {
            highScores[i] = new Score(0, 0, 0);
        }*/
        DontDestroyOnLoad(gameObject);
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
    public void PopulateList {
        Debug.Log("Fuck you");
    }
}
