using UnityEngine;
using System.Collections;

public class LevelParameter {
    public bool enemySpawning;
    public float enemySpeed;
    public float enemyDelay;
    public int enemyHealth;
    public float buildingWidth;
    public float buildingMaxHeight;
    public float buildingMinHeight;
    public float buildingGap;
    public float distanceTravelledForNextLevel;

    public LevelParameter(bool spawning, float speed, float delay, int health, float bWidth, float bMaxHeight, float bMinHeight, float bGap, float distanceForNextLevel) {
        enemySpawning = spawning;
        enemySpeed = speed;
        enemyDelay = delay;
        enemyHealth = health;
        buildingWidth = bWidth;
        buildingMaxHeight = bMaxHeight;
        buildingMinHeight = bMinHeight;
        buildingGap = bGap;
        distanceTravelledForNextLevel = distanceForNextLevel;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
