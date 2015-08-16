using UnityEngine;
using System.Collections;

public class Score {
    public int distanceTraveled;
    public int enemysKilled;
    public int timeSurvived;

    public Score(int distance, int kills, int time) {
        distanceTraveled = distance;
        enemysKilled = kills;
        timeSurvived = time;
    }
    public override string ToString() {
        return "distance: " + this.distanceTraveled + " , " + "time: " + this.timeSurvived + " , " + "kills: " + this.enemysKilled;
    }

}
