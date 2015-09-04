using UnityEngine;
using System.Collections;

public class LevelParameter {
    public bool chaserSpawning;
    public bool turretSpawning;
    public float turretSpawnProbability;
    public float chaserSpeed;
    public float chaserDelay;
    public int chaserHealth;
    public float buildingWidth;
    public float buildingMaxHeight;
    public float buildingMinHeight;
    public float buildingGap;
    public float distanceTravelledForNextLevel;

    public LevelParameter(bool turretSpawning, float turretSpawnProbability, bool chaserSpawning, float chaserSpeed, float chaserDelay, int chaserHealth, float buildingWidth, float buildingMaxHeight, float buildingMinHeight, float buildingGap, float distanceTravelledForNextLevel) {
        this.chaserSpawning = chaserSpawning;
        this.turretSpawning = turretSpawning;
        this.turretSpawnProbability = turretSpawnProbability;
        this.chaserSpeed = chaserSpeed;
        this.chaserDelay = chaserDelay;
        this.chaserHealth = chaserHealth;
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
