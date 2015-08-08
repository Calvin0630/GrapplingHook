using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerGUI : MonoBehaviour {
	public Text distanceField;
    int distance;
    ObstacleSpawner spawn;
	// Use this for initialization
	void Start () {
        spawn = GameObject.Find("ObstacleSpawner").GetComponent<ObstacleSpawner>();
        //distance = spawn.distanceTravelled;
	}
	
	// Update is called once per frame
	void Update () {
        distance = (int) spawn.distanceTravelled;
        distanceField.text = distance + " Metres Travelled";
	}
}
