using UnityEngine;
using System.Collections;

public class Score {
    public int distanceTraveled;
    public int enemysKilled;
    public int timeSurvived;

    public Score(int distance, int kills, int time) {
        distanceTraveled = distance;
        timeSurvived = time;
    }

    public Score() {
        distanceTraveled = 0;
        timeSurvived = 0;

    }
    public override string ToString() {
        return "                            " + this.distanceTraveled + "                       " + this.timeSurvived;
    }

}
