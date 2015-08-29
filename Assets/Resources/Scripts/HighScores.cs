using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("HighScoreCollection")]
public class HighScores {
    [XmlArray("HighScores")]
    [XmlArrayItem("Score")]
    public List<Score> list;
    static string path = Path.Combine(Application.dataPath, "HighScores.xml");
    XMLManager xmlManager = new XMLManager();
    //string path = "HighScores.xml";

    public HighScores() {
        list = XMLManager.Load().scores;
        AddScore(new Score(0,0,"name"));
    }

    public void AddScore(Score score) {
        list.Add(score);
        list.Sort();
        //if (list.Count > 30) list.RemoveAt(list.Count - 1);
        xmlManager.Save();
    }
}
