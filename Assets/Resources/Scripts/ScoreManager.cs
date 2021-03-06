﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {
    public static HighScores highScores;
    GameObject scoreBox;
    GameObject highScorePanel;
    static GameObject gameOverPanelPrefab;
    GameObject nameField;
    GameObject spawner;
    public static string highScorePath;
    public static bool gameIsOver;
	// Use this for initialization

    //called before start
    void Awake() {
        highScorePath = System.IO.Path.Combine(Application.dataPath, "HighScores.xml");
        if (!File.Exists(ScoreManager.highScorePath)) {
            using (StreamWriter file = new StreamWriter(File.Create(ScoreManager.highScorePath))) {
                Debug.Log("making new XML file.");
                string fileContents = "<?xml version = \"1.0\" ?>\n<HighScores xmlns:xsd = \"http://www.w3.org/2001/XMLSchema\" xmlns:xsi = \"http://www.w3.org/2001/XMLSchema-instance\">\n\t<HighScores>\n\t\t<ScoreElement name=\"bush_did_911\" distanceTravelled=\"420\" timeSurvived=\"69\" highSpeed=\"42\"/>\n\t</HighScores>\n</HighScores>";
                //string fileContents = "<? xml version = \"1.0\" ?>\n< HighScores xmlns:xsd = \"http://www.w3.org/2001/XMLSchema\" xmlns: xsi = \"http://www.w3.org/2001/XMLSchema-instance\" >\n\n</ HighScores >";
                file.Write(fileContents);
                //File.AppendAllText(highScorePath, fileContents);
            }
        }
        highScores = new HighScores();
        Read();
        scoreBox = (GameObject)Resources.Load("Prefab/UI/ScoreBox");
        spawner = GameObject.FindWithTag("Spawn");
        gameIsOver = false;
        gameOverPanelPrefab = (GameObject)Resources.Load("Prefab/UI/GameOverPanel");
        Write();
    }

	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnLevelWasLoaded(int level) {
        if (level == 2) {
            gameIsOver = false;
        }
    }
    
    public override string ToString() {
        string result = "";
        for (int i=0;i<highScores.list.Count;i++) {
            if (highScores.list[i] != null) {
                result += "distance: " + highScores.list[i].distanceTraveled + " , " + "time: " + highScores.list[i].timeSurvived + " \n ";
            }
        }
        return result;
    }
    public void PopulateList() {
        //should get code to read from file
        highScorePanel = GameObject.FindWithTag("HighScoreList");
        for (int i = 0; i < highScores.list.Count; i++) {
            if (highScores.list[i] != null) {
                GameObject scoreItem = Instantiate(scoreBox);
                //fils text fields in the highscore box
                Text[] fields = scoreItem.GetComponentsInChildren<Text>();
                foreach (Text text in fields) {
                    if (text.name == "Place") text.text = (i + 1) + ":";
                    else if (text.name == "Name") text.text = highScores.list[i].name;
                    else if (text.name == "Distance") text.text = highScores.list[i].distanceTraveled + " m";
                    else if (text.name == "Time") text.text = "" + highScores.list[i].timeSurvived + " s";
                    else if (text.name == "Speed") text.text = highScores.list[i].highSpeed + " m/s";
                }
                //scoreItem.GetComponent<Text>().text = " " + (i + 1) + ": " + highScores.list[i].ToString();
                scoreItem.GetComponent<RectTransform>().SetParent(highScorePanel.transform, false);
                //scoreItem.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public static void GameOver() {
        if (!gameIsOver) { 
            Time.timeScale = 0;
            GameObject gameOverPanel;
            if (SceneManager.GetActiveScene().name == "SinglePlayerMellow") gameOverPanel = (GameObject) Instantiate((GameObject) Resources.Load("Prefab/UI/GameOverPanelMellow"));
            else gameOverPanel = Instantiate(gameOverPanelPrefab);
            gameOverPanel.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
            GameObject congratsText = GameObject.Find("CongradulationsText");
            if (SceneManager.GetActiveScene().name != "SinglePlayerMellow") {
                GameObject scoreInfo = GameObject.FindWithTag("ScoreInfo");
                foreach (Text text in scoreInfo.GetComponentsInChildren<Text>()) {
                    if (text.gameObject.name == "Distance") text.text = (int)ObstacleSpawner.distanceTravelled + " m";
                    else if (text.gameObject.name == "Time") text.text = (int)Time.timeSinceLevelLoad + " s";
                    else if (text.gameObject.name == "MaxSpeed") text.text = (int)ObstacleSpawner.maxSpeed + " m/s";
                }
                congratsText.GetComponent<Text>().text = "You made it " + (int)ObstacleSpawner.distanceTravelled + " metres!! GG";
            }
            ObstacleSpawner.worldVelocity = Vector3.zero;
            gameIsOver = true;
        }
        //GG.GetComponent<RectTransform>().
        //nameField = GameObject.FindWithTag("NameField");
    }
    //xml shit
    public static void Write() {
        using (FileStream fs = new FileStream(highScorePath, FileMode.Create)) {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(HighScores));
            x.Serialize(fs, highScores);
            fs.Close();
        }
    }
    //public void AddScore(Score score)
    //xml shit
    public void Read() {
        try {
            using (StreamReader reader = new StreamReader(highScorePath)) {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(HighScores));
                highScores = (HighScores)x.Deserialize(reader);
            }
        }
        catch(System.Xml.XmlException e) {
            highScores.list = new List<Score>();
            Write();
        }
    }

}
