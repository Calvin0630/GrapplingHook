using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using System.IO;

public class ScoreManager : MonoBehaviour {
    public static HighScores highScores;
    GameObject scoreBox;
    GameObject highScorePanel;
    GameObject gameOverPanel;
    GameObject nameField;
    GameObject spawner;
    public static string highScorePath = System.IO.Path.Combine(Application.dataPath, "HighScores.xml");
    public bool gameIsOver;
	// Use this for initialization

    //called before start
    void Awake() {
        highScores = new HighScores();
        Read();
        scoreBox = (GameObject)Resources.Load("Prefab/UI/ScoreBox");
        spawner = GameObject.FindWithTag("Spawn");
        DontDestroyOnLoad(gameObject);
        gameIsOver = false;
        gameOverPanel = (GameObject)Resources.Load("Prefab/UI/GameOverPanel");
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
    
    public string ToString() {
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

    public void GameOver() {
        if (!gameIsOver) { 
            Time.timeScale = 0;
            GameObject GG = Instantiate(gameOverPanel);
            GG.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
            GameObject congratsText = GameObject.Find("CongradulationsText");
            spawner = GameObject.Find("ObstacleSpawner");
            congratsText.GetComponent<Text>().text = "You made it " + (int)spawner.GetComponent<ObstacleSpawner>().distanceTravelled + " metres!! GG";
            spawner.GetComponent<ObstacleSpawner>().worldVelocity = Vector3.zero;
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
