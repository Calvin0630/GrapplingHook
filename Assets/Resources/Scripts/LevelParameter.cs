using UnityEngine;
using System.Collections;

public class LevelParameter {
    public bool enemySpawning;
    public bool turretSpawning;
    public float enemySpeed;
    public float enemyDelay;
    public int enemyHealth;
    public float buildingWidth;
    public float buildingMaxHeight;
    public float buildingMinHeight;
    public float buildingGap;
    public float distanceTravelledForNextLevel;

    public LevelParameter(bool enemySpawning, bool turretSpawning, float enemySpeed, float enemyDelay, int enemyHealth, float buildingWidth, float buildingMaxHeight, float buildingMinHeight, float buildingGap, float distanceTravelledForNextLevel) {
        this.enemySpawning = enemySpawning;
        this.turretSpawning = turretSpawning;
        this.enemySpeed = enemySpeed;
        this.enemyDelay = enemyDelay;
        this.enemyHealth = enemyHealth;
        this.buildingWidth = buildingWidth;
        this.buildingMaxHeight = buildingMaxHeight;
        this.buildingMinHeight = buildingMinHeight;
        this.buildingGap = buildingGap;
        this.distanceTravelledForNextLevel = distanceTravelledForNextLevel;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
