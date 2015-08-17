using UnityEngine;
using System.Collections;

public class HighScoreTable : MonoBehaviour {
    GameObject scoreManager;
	// Use this for initialization
	void Start () {
        scoreManager = GameObject.FindWithTag("ScoreManager");
        if (scoreManager != null) {
            scoreManager.GetComponent<ScoreManager>().PopulateList();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    IEnumerator FillTable(float delay) {
        yield return new WaitForSeconds(delay);
        scoreManager.GetComponent<ScoreManager>().PopulateList();
    }

}
