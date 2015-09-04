using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObstacleSpawner : MonoBehaviour {
    //gameover detection, and moving players
    public bool isActive;
    GameObject player1;
    GameObject player2;
    GameObject obstacle;
    GameObject roof;
    List<GameObject> roofObjects;
    List<GameObject> obstacleObjects;
    // is in [-1, 1]
    public float movingPointX;
    //top left corner of world screen
    Vector3 cameraSize;
    float cameraWidth;
    float worldMovePointX;
    GameObject farthestPlayer;
    public static float worldVelocityX;
    public static float maxSpeed = 0;
    Rigidbody player1RigidBody;
    Rigidbody player2RigidBody;
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
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        //ternary operator
        numOfPlayers = (player2 == null) ? 1 : 2;
        roof = (GameObject)Resources.Load("Prefab/Roof");
        obstacle = (GameObject)Resources.Load("Prefab/Obstacle");
        FramePiece = (GameObject)Resources.Load("Prefab/GameOverDetector");
        chaserPrefab = (GameObject) Resources.Load("Prefab/Chaser");
        turretPrefab = (GameObject)Resources.Load("Prefab/Turret");
        //initiates lists
        roofObjects = new List<GameObject>();
        obstacleObjects = new List<GameObject>();
        //finds roof objects in scene, and adds them to the array
        tmp = GameObject.FindGameObjectsWithTag("Wall");
        for (int i = 0; i < tmp.Length; i++) roofObjects.Add(tmp[i]);
        if (roofObjects.Count > 0) roofObjects.Sort((p1, p2) => p1.transform.position.x.CompareTo(p2.transform.position.x));
        //finds obstacle objects in scene, and adds them to the obstacleList
        tmp = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i < tmp.Length; i++) obstacleObjects.Add(tmp[i]);
        if(obstacleObjects.Count > 0) obstacleObjects.Sort((p1, p2) => p1.transform.position.x.CompareTo(p2.transform.position.x));
        if (player1 != null) player1RigidBody = player1.GetComponent<Rigidbody>();
        if (player2 != null) player2RigidBody = player2.GetComponent<Rigidbody>();
        cameraSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        cameraWidth = cameraSize.x;
        worldMovePointX = movingPointX * cameraWidth;
        MakeFrame(GameOverDetection);
        distanceField = GameObject.FindWithTag("DistanceText");
        //for debugging
        levelIndex = 5;
        levels = new LevelParameter[] {
            //turretSpawning, turretSpawnProbability, chaserSpawning, chaserSpeed, chaserSpawnDelay, chaserHealth
            //buildingWidth, buildingMaxHeight, buildingMinHeight, buildingGap
            new LevelParameter(false, 2, false, 1, 1, 1, 3, -2, -4, 3, 100),
            new LevelParameter(false, 2, true, 3, 4, 2, 3, -2, -4, 3, 500),
            new LevelParameter(false, 2, true, 3, 3, 3, 4, 0, -4, 3, 1000),
            new LevelParameter(false, 2, true, 4, 3, 3, 3, 0, -4, 4, 2000),
            new LevelParameter(false, 2, true, 4, 2, 2, 4, 1, -3, 5, 3000),
            //for debugging purposes
            new LevelParameter(true, 2, false, 1, 1, 1, 3, -2, -4, 3, 10000), 
        };
        StartCoroutine(SpawnChasers(levels[levelIndex].chaserDelay));
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        if (distanceTravelled > levels[levelIndex].distanceTravelledForNextLevel && levelIndex < levels.Length-1) {
            levelIndex++;
        }
        distanceTravelled -= Time.deltaTime * worldVelocity.x;
        distance = (int)distanceTravelled;
        if (worldVelocityX > maxSpeed) maxSpeed = worldVelocityX;
        //Debug.Log(distanceField == null);
        distanceField.GetComponent<Text>().text = distance + " Metres Travelled";
        if (numOfPlayers == 1) FindWorldVelocity1Player();
        else if (numOfPlayers == 2) FindWorldVelocity2Player();
        MoveWorld();
        if (isActive) {
            //moves player1
            if (player1.transform.position.x > worldMovePointX) player1RigidBody.velocity = player1RigidBody.velocity + worldVelocity - prevWorldVelocity;
            //moves player2 
            if (player2 != null && player2.transform.position.x > worldMovePointX) player2RigidBody.velocity = player2RigidBody.velocity + worldVelocity - prevWorldVelocity;
            prevWorldVelocity = worldVelocity;
        }
        MakeRoofObjects();
        DeleteRoofObjects();
        DeleteObstacleObjects();
        MakeFloorObjects();
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

    GameObject FindFarthestPlayer2Player() {
        if (player2 != null && player1.transform.position.x > player2.transform.position.x) {
            return player1;
        }
        else return player2;
    }

    void OnLevelWasLoaded(int level) {
        if (level == 2) {
            levelIndex = 0;
            distanceTravelled = 0;
        }
    }

    //find world velocity
    void FindWorldVelocity1Player() {
        if (player1.transform.position.x > worldMovePointX) {
            farthestPlayer = player1;
            worldVelocityX = CalculateWorldVelocity(farthestPlayer.transform.position.x);
            worldVelocity = new Vector3(-worldVelocityX, 0, 0);
            //Debug.Log (worldObjects.Length);
        }
        else {
            worldVelocity = Vector3.zero;
        }
    }

    void FindWorldVelocity2Player() {
        if (player2 != null && (player1.transform.position.x > worldMovePointX || player2.transform.position.x > worldMovePointX)) {
            farthestPlayer = FindFarthestPlayer2Player();
            worldVelocityX = CalculateWorldVelocity(farthestPlayer.transform.position.x);
            worldVelocity = new Vector3(-worldVelocityX, 0, 0);
            //Debug.Log (worldObjects.Length);
        }
        else {
            worldVelocity = Vector3.zero;
        }
    }

    //calculates world velocity given the farthest players position
    float CalculateWorldVelocity(float position) {
        return Mathf.Pow(2, .9f * (position + 2));
    }

    void MakeRoofObjects() {
        if (roofObjects.Count == 0) {
            GameObject clone = (GameObject)Instantiate(roof, new Vector3(0, 5, 0), Quaternion.identity);
            clone.GetComponent<Rigidbody>().velocity = new Vector3(-worldVelocityX, 0, 0);
            clone.transform.localScale = new Vector3(20, 2, 1);
            roofObjects.Add(clone);
        }
        else if (roofObjects[roofObjects.Count - 1].transform.position.x < 3.89) {
            //make new roof object w/ left edge @ right edge of camera 
            GameObject clone = (GameObject)Instantiate(roof, new Vector3(18.89f, 5, 0), Quaternion.identity);
            clone.GetComponent<Rigidbody>().velocity = new Vector3(-worldVelocityX, 0, 0);
            clone.transform.localScale = new Vector3(20, 2, 1);
            roofObjects.Add(clone);
        }
    }

    void DeleteRoofObjects() {
        if (roofObjects[0].transform.position.x < -40) {
            Destroy(roofObjects[0]);
            roofObjects.RemoveAt(0);
        }
    }

    void DeleteObstacleObjects() {
        if (obstacleObjects.Count > 0) {
            if (obstacleObjects[0].transform.position.x + obstacleObjects[0].transform.localScale.x/2 < -cameraSize.x) {
                Destroy(obstacleObjects[0]);
                obstacleObjects.RemoveAt(0);
            }
        }
    }

    void MoveWorld() {
        //moves obstacles
        for (int i = 0; i < obstacleObjects.Count; i++) {
            if (obstacleObjects[i] != null) {
                obstacleObjects[i].GetComponent<Rigidbody>().velocity = worldVelocity;
            }
        }
        //moves roofs
        for (int i = 0; i < roofObjects.Count; i++) {
            if (roofObjects[i] != null) {
                roofObjects[i].GetComponent<Rigidbody>().velocity = worldVelocity;
            }
        }
    }

    void MakeFloorObjects() {
        if (obstacleObjects.Count == 0) {
            //Debug.Log("no Obstacles in List. Wat do?");
        }
        else if (obstacleObjects[obstacleObjects.Count - 1].transform.position.x + obstacleObjects[obstacleObjects.Count - 1].transform.localScale.x/2 < cameraSize.x) {
            //spawns obstacle
            float yTop = Random.Range(levels[levelIndex].buildingMinHeight, levels[levelIndex].buildingMaxHeight);
            float yScale = yTop + 2 * cameraSize.y;
            GameObject shitVarName = (GameObject)Instantiate(obstacle, new Vector3(cameraSize.x + levels[levelIndex].buildingGap, yTop - yScale/2, 0), Quaternion.identity);
            shitVarName.transform.localScale = new Vector3(levels[levelIndex].buildingWidth, yScale, 1);
            obstacleObjects.Add(shitVarName);
            //spawns turret
            if (Random.Range(0, 1000) % levels[levelIndex].turretSpawnProbability == 0) {
                GameObject turretClone = (GameObject)Instantiate(turretPrefab, new Vector3(shitVarName.transform.position.x, yTop + turretPrefab.transform.lossyScale.y / 2, 0), Quaternion.identity);
                turretClone.GetComponent<Turret>().initialHealth = 3;
                turretClone.transform.parent = shitVarName.transform;
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
