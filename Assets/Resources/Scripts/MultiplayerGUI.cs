using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerGUI : MonoBehaviour {
	public Text distanceField;
    int distance;
    BuildingSpawner spawn;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        distance = (int) BuildingSpawner.distanceTravelled;
        distanceField.text = distance + " Metres Travelled";
	}
}
