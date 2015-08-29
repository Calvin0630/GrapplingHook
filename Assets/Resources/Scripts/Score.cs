using System;
using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("Score")]
public class Score : IComparable {
    [XmlAttribute("name")]
    public string name;
    [XmlAttribute("distanceTravelled")]
    public int distanceTraveled;
    [XmlAttribute("timeSurvived")]
    public int timeSurvived;
    [XmlAttribute("highSpeed")]
    public int highSpeed;

    public Score(int distance, int time, int highSpeed, string name) {
        distanceTraveled = distance;
        timeSurvived = time;
        this.name = name;
        this.highSpeed = highSpeed;
    }
    public Score() { }
    /*
    public Score() {
        distanceTraveled = 0;
        timeSurvived = 0;

    }*/
    public override string ToString() {
        return " " + "name: " + this.name + " distance:  " + this.distanceTraveled + " time " + this.timeSurvived + " MaxSpeed: " + highSpeed;
    }

    int IComparable.CompareTo(object obj) {
        Score score2 = (Score)obj;
        Score score1 = (Score)this;
        if (score1.distanceTraveled < score2.distanceTraveled) return 1;
        else if (score1.distanceTraveled > score2.distanceTraveled) return -1;
        else return 0;
    }

}
