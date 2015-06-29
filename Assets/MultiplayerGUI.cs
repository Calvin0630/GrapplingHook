using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerGUI : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
	public string score = "Score: 0";
	public Text timeField;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeField.text= "Time: " + Time.time.ToString("f1");
	}




}
