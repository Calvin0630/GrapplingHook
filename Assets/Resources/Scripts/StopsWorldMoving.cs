using UnityEngine;
using System.Collections;
//this scirpt is only here because FUCK UNITY. It should be replaced with OnLevelWasLoaded in StartGame.cs when unity gets their head out of their ass.
//FUCK UNITY
//FUCK UNITY
//FUCK UNITY
//FUCK UNITY
//FUCK UNITY
//FUCK UNITY
//FUCK UNITY
public class StopsWorldMoving : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ObstacleSpawner.worldMovingIsEnabled = false;
	}
	
    void OnDestroy() {
        ObstacleSpawner.worldMovingIsEnabled = true;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
