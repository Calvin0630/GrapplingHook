using System;
using UnityEngine;
using System.Collections;

public class Score :IComparable{
    public int distanceTraveled;
    public int timeSurvived;
    public string name;

    public Score(int distance, int time, string name) {
        distanceTraveled = distance;
        timeSurvived = time;
        this.name = name;
    }

    public Score() {
        distanceTraveled = 0;
        timeSurvived = 0;

    }
    public override string ToString() {
        return " " + this.name + "   " + this.distanceTraveled + "  " + this.timeSurvived;
    }

    int IComparable.CompareTo(object obj) {
        Score score2 = (Score)obj;
        Score score1 = (Score)this;
        if (score1.distanceTraveled < score2.distanceTraveled) return 1;
        else if (score1.distanceTraveled > score2.distanceTraveled) return -1;
        else return 0;
    }

}
