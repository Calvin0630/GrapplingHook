using UnityEngine;
using System.Collections;

public class Score {
    public int distanceTraveled;
    public int timeSurvived;
    string name;

    public Score(int distance, int time, string name) {
        distanceTraveled = distance;
        timeSurvived = time;
    }

    public Score() {
        distanceTraveled = 0;
        timeSurvived = 0;

    }
    public override string ToString() {
        return "   " + this.name + "                  " + this.distanceTraveled + "                       " + this.timeSurvived;
    }

}
