using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

[Serializable]
public class HighScores {
    [XmlArray("HighScores")]
    [XmlArrayItem(typeof(Score), ElementName = "ScoreElement")]
    public List<Score> list;
    static string path = Path.Combine(Application.dataPath, "HighScores.xml");
    //string path = "HighScores.xml";

    public HighScores() {
        list = new List<Score>();
    }

    public void AddScore(Score score) {
        list.Add(score);
        list.Sort();
        ScoreManager.Write();
    }

}
