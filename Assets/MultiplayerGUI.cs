using UnityEngine;
using UnintyEngine.UI;
using System.Collections;

public class MultiplayerGUI : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
	public string score = "Score: 0";
	public GUIText currentScore;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		score = Time.time.ToString();
	}



	void OnGUI() {
		score = GUI.TextField(new Rect(10, 10, 200, 20), score, 25);
	}
}
