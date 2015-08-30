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
    //string path = "HighScores.xml";

    public HighScores() {
        list = new List<Score>();
    }

    public void AddScore(Score score) {
        list.Add(score);
        list.Sort();
        ScoreManager.Write();
    }

    public override string ToString() {
        string result = "";
        for (int i=0;i<list.Count;i++) {
            result += list[i].ToString() + "\n";
        }
        return result;
    }

}
