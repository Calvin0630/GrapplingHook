using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class BuildingSpawner : MonoBehaviour {
    public static bool worldMovingIsEnabled = true;
    //gameover detection, and moving players
    GameObject player;
    GameObject Building;
    GameObject floorPrefab;
    List<GameObject> floorObjects;
    List<GameObject> BuildingObjects;
    // is in [-1, 1]
    public float movingPointX;
    //top left corner of world screen
    Vector3 cameraSize;
    float cameraWidth;
    float worldMovePointX;
    GameObject farthestPlayer;
    public static float worldVelocityX;
    public static float maxSpeed = 0;
    Rigidbody playerRigidBody;
    Vector3 prevWorldVelocity;
    public static Vector3 worldVelocity;
    GameObject[] tmp;
    public bool GameOverDetection;
    GameObject FramePiece;
    GameObject frameTemp;
    public static float distanceTravelled = 0;
    int numOfPlayers;
    GameObject distanceField;
    int distance;
    GameObject chaserPrefab;
    GameObject turretPrefab;
    public LevelParameter[] levels;
    public int levelIndex;
    public bool enemySpawning;
    public float turretFrequency;

    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player1");
        floorPrefab = (GameObject)Resources.Load("Prefab/Floor");
        Building = (GameObject)Resources.Load("Prefab/Building");
        FramePiece = (GameObject)Resources.Load("Prefab/GameOverDetector");
        chaserPrefab = (GameObject)Resources.Load("Prefab/Enemies/Chaser");
        turretPrefab = (GameObject)Resources.Load("Prefab/Enemies/Turret");
        //initiates lists
        floorObjects = new List<GameObject>();
        BuildingObjects = new List<GameObject>();
        //finds floor objects in scene, and adds them to the array
        tmp = GameObject.FindGameObjectsWithTag("Floor");
        for (int i = 0; i < tmp.Length; i++) floorObjects.Add(tmp[i]);
        if (floorObjects.Count > 0) floorObjects.Sort((p1, p2) => p1.transform.position.x.CompareTo(p2.transform.position.x));
        //finds Building objects in scene, and adds them to the BuildingList
        tmp = GameObject.FindGameObjectsWithTag("Building");
        for (int i = 0; i < tmp.Length; i++) BuildingObjects.Add(tmp[i]);
        if (BuildingObjects.Count > 0) BuildingObjects.Sort((p1, p2) => p1.transform.position.x.CompareTo(p2.transform.position.x));
        if (player != null) playerRigidBody = player.GetComponent<Rigidbody>();
        cameraSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        cameraWidth = cameraSize.x;
        worldMovePointX = movingPointX * cameraWidth;
        MakeFrame(GameOverDetection);
        distanceField = GameObject.FindWithTag("DistanceText");
        //for debugging
        levelIndex = 0;
        levels = new LevelParameter[] {
            //turretSpawning, turretSpawnProbability, chaserSpawning, chaserSpeed, chaserSpawnDelay, chaserHealth
            //buildingWidth, buildingMaxHeight, buildingMinHeight, buildingGap
            new LevelParameter(false, 2, false, 1, 1, 1, 3, -2, -4, 3, 100),
            new LevelParameter(false, 6, true, 3, 4, 2, 3, -2, -4, 3, 500),
            new LevelParameter(false, 4, true, 3, 3, 3, 4, 0, -4, 3, 1000),
            new LevelParameter(true, 4, true, 4, 3, 3, 3, 0, -4, 4, 2000),
            new LevelParameter(true, 2, true, 4, 2, 2, 4, 1, -3, 5, 3000),
            //for debugging purposes
            new LevelParameter(true, 2, false, 1, 1, 1, 3, -2, -4, 3, 10000),
        };
        StartCoroutine(SpawnChasers(levels[levelIndex].chaserDelay));
    }

    // Update is called once per frame
    void Update() {
        distanceTravelled -= Time.deltaTime * worldVelocity.x;

    }

    void FixedUpdate() {
        if (distanceTravelled > levels[levelIndex].distanceTravelledForNextLevel && levelIndex < levels.Length - 1) {
            levelIndex++;
        }
        distance = (int)distanceTravelled;
        //calculates maxSpeed for score
        if (worldVelocityX > maxSpeed) maxSpeed = worldVelocityX;
        distanceField.GetComponent<Text>().text = distance + " Metres Travelled";
        FindWorldVelocity();
        if (worldMovingIsEnabled) {
            //moves player
            playerRigidBody.velocity = playerRigidBody.velocity + worldVelocity - prevWorldVelocity;

            prevWorldVelocity = worldVelocity;
            MoveWorld();
        }
        MakeFloorObjects();
        DeleteFloorObjects();
        DeleteBuildingObjects();
        //MakeBuildingObjects();
    }

    //makes the GameOver frame
    void MakeFrame(bool GameOverDetection) {
        frameTemp = (GameObject)Instantiate(FramePiece, new Vector3(0, 2 * cameraSize.y, 0), Quaternion.identity);
        frameTemp.transform.localScale = new Vector3(4 * cameraSize.x, 2, 2);
        frameTemp.GetComponent<GameOverDetector>().isActive = GameOverDetection;
        frameTemp = (GameObject)Instantiate(FramePiece, new Vector3(0, -2 * cameraSize.y, 0), Quaternion.identity);
        frameTemp.transform.localScale = new Vector3(4 * cameraSize.x, 2, 2);
        frameTemp.GetComponent<GameOverDetector>().isActive = GameOverDetection;
        frameTemp = (GameObject)Instantiate(FramePiece, new Vector3(2 * cameraSize.x, 0, 0), Quaternion.identity);
        frameTemp.transform.localScale = new Vector3(2, 4 * cameraSize.y + 2, 2);
        frameTemp.GetComponent<GameOverDetector>().isActive = GameOverDetection;
        frameTemp = (GameObject)Instantiate(FramePiece, new Vector3(-2 * cameraSize.x, 0, 0), Quaternion.identity);
        frameTemp.transform.localScale = new Vector3(2, 4 * cameraSize.y + 2, 2);
        frameTemp.GetComponent<GameOverDetector>().isActive = GameOverDetection;
    }

    void OnLevelWasLoaded(int level) {
        if (level == 2) {
            levelIndex = 0;
            distanceTravelled = 0;
        }
    }

    //find world velocity
    void FindWorldVelocity() {
        if ( worldMovingIsEnabled) {
            worldVelocity = CalculateWorldVelocity(player.transform.position);
            //Debug.Log (worldObjects.Length);
        }
        else {
            worldVelocity = Vector3.zero;
        }
    }

    //calculates world velocity given the farthest players position
    Vector3 CalculateWorldVelocity(Vector3 playerPos) {
        Vector3 center = new Vector3(0,-5,0);
        float distanceFromCenter = (center-playerPos).magnitude;
        return (center-playerPos).normalized*Mathf.Pow(2,distanceFromCenter);
    }

    void MakeFloorObjects() {

        //if there are no floor objects in scene
        if (floorObjects.Count == 0) {
            //create floor objects in middle of scene
            GameObject clone = (GameObject)Instantiate(floorPrefab, new Vector3(0, -5, 0), Quaternion.identity);
            clone.GetComponent<Rigidbody>().velocity = new Vector3(-worldVelocityX, 0, 0);
            clone.transform.localScale = new Vector3(20, 2, 1);
            floorObjects.Add(clone);
        }
        //checks position of rightmost object
        else if (floorObjects[floorObjects.Count - 1].transform.position.x < 4) {
            //make new floor object @ right edge of camera 
            GameObject clone = (GameObject)Instantiate(floorPrefab, new Vector3(18.89f, -5, 0), Quaternion.identity);
            clone.GetComponent<Rigidbody>().velocity = new Vector3(-worldVelocityX, 0, 0);
            clone.transform.localScale = new Vector3(20, 2, 1);
            //adds object to end of list
            floorObjects.Add(clone);
        }

        //checks position of leftmost object
        else if (floorObjects[0].transform.position.x > -4) {
            //make new floor object @ left edge of camera
            GameObject clone = (GameObject)Instantiate(floorPrefab, new Vector3(-18.89f, -5, 0), Quaternion.identity);
            clone.GetComponent<Rigidbody>().velocity = new Vector3(-worldVelocityX, 0, 0);
            clone.transform.localScale = new Vector3(20, 2, 1);
            //adds object to start of list
            floorObjects.Insert(0, clone);
        }
    }

    void DeleteFloorObjects() {
        //checks leftmost object
        if (floorObjects[0].transform.position.x < -30) {
            Destroy(floorObjects[0]);
            floorObjects.RemoveAt(0);
        }
        //checks rightmost object
        if (floorObjects[floorObjects.Count-1].transform.position.x > 30) {
            Destroy(floorObjects[floorObjects.Count-1]);
            floorObjects.RemoveAt(floorObjects.Count-1);
        }
    }

    void DeleteBuildingObjects() {
        if (BuildingObjects.Count > 0) {
            if (BuildingObjects[0].transform.position.x + BuildingObjects[0].transform.localScale.x / 2 < -cameraSize.x) {
                Destroy(BuildingObjects[0]);
                BuildingObjects.RemoveAt(0);
            }
        }
    }

    void MoveWorld() {
        //moves Buildings
        for (int i = 0; i < BuildingObjects.Count; i++) {
            if (BuildingObjects[i] != null) {
                BuildingObjects[i].GetComponent<Rigidbody>().velocity = worldVelocity;
            }
        }
        //moves floors
        for (int i = 0; i < floorObjects.Count; i++) {
            if (floorObjects[i] != null) {
                floorObjects[i].GetComponent<Rigidbody>().velocity = worldVelocity;
            }
        }
    }

    void MakeBuildingObjects() {
        if (BuildingObjects.Count == 0) {
            //Debug.Log("no Buildings in List. Wat do?");
        }
        else if (BuildingObjects[BuildingObjects.Count - 1].transform.position.x + BuildingObjects[BuildingObjects.Count - 1].transform.localScale.x / 2 < cameraSize.x) {
            //spawns Building
            float yTop = Random.Range(levels[levelIndex].buildingMinHeight, levels[levelIndex].buildingMaxHeight);
            float yScale = yTop + 2 * cameraSize.y;
            GameObject shitVarName = (GameObject)Instantiate(Building, new Vector3(cameraSize.x + levels[levelIndex].buildingGap, yTop - yScale / 2, 0), Quaternion.identity);
            shitVarName.transform.localScale = new Vector3(levels[levelIndex].buildingWidth, yScale, 1);
            BuildingObjects.Add(shitVarName);
            //spawns turret
            if (levels[levelIndex].turretSpawning && enemySpawning) {
                if (Random.Range(0, 1000) % levels[levelIndex].turretSpawnProbability == 0) {
                    GameObject turretClone = (GameObject)Instantiate(turretPrefab, new Vector3(shitVarName.transform.position.x, yTop + turretPrefab.transform.lossyScale.y / 2, 0), Quaternion.identity);
                    turretClone.GetComponent<Turret>().initialHealth = 3;
                    turretClone.transform.parent = shitVarName.transform;
                }
            }
        }
    }

    IEnumerator SpawnChasers(float delay) {
        yield return new WaitForSeconds(delay);
        if (levels[levelIndex].chaserSpawning && enemySpawning) {
            GameObject clone = (GameObject)Instantiate(chaserPrefab, new Vector3(-1.1f * cameraSize.x, 0, 0), Quaternion.identity);
            clone.GetComponent<Chaser>().moveSpeed = levels[levelIndex].chaserSpeed;
            clone.GetComponent<Chaser>().initialHealth = levels[levelIndex].chaserHealth;
        }
        StartCoroutine(SpawnChasers(levels[levelIndex].chaserDelay));
    }
}
