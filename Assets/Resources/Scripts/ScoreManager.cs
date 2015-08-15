using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    Score[] highScores;
	// Use this for initialization

	void Start () {
        highScores = new Score[10];
        for (int i=0;i<10;i++) {
            highScores[i] = new Score(0, 0, 0);
        }
        //DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddScore(Score score) {
        for (int i=0;i<10;i++) {
            /*
            if (highScores[i] == null) {
                highScores[i] = score;
                break;
            }*/
            if (score.distanceTraveled > highScores[i].distanceTraveled) {
                Score tmp = highScores[i];
                highScores[i] = score;
                for (int j= i+1;j<9;j++) {
                    highScores[j+1] = highScores[j];
                }
                highScores[i + 1] = tmp;
                break;
            }

        }
    }
    public string ToString() {
        string result = "";
        for (int i=0;i<10;i++) {
            if (highScores[i] != null) {
                result += "distance: " + highScores[i].distanceTraveled + " , " + "time: " + highScores[i].timeSurvived + " , " + "kills: " + highScores[i].enemysKilled + " \n ";
            }
        }
        return result;
    }
}
