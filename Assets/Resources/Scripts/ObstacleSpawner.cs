using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObstacleSpawner : MonoBehaviour {
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
    public float worldVelocityX;
    Rigidbody player1RigidBody;
    Rigidbody player2RigidBody;
    Vector3 prevWorldVelocity;
    Vector3 worldVelocity;
    GameObject[] tmp;
    public bool GameOverDetection;
    GameObject FramePiece;
    GameObject frameTemp;
    public float distanceTravelled = 0;
    int numOfPlayers;
    public Text distanceField;
    int distance;
    //the x position of the left-most obstacle
    GameObject lastObstacle;
    float spawnPointX;
    GameObject enemy;
    public float EnemySpawnDelay;
    public float initialEnemySpeed;
    public float deltaEnemySpeed;
    float enemySpawnIndex;

    // Use this for initialization
    void Start() {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        //ternary operator
        numOfPlayers = (player2 == null) ? 1 : 2;
        roof = (GameObject)Resources.Load("Prefab/Roof");
        obstacle = (GameObject)Resources.Load("Prefab/Obstacle");
        FramePiece = (GameObject)Resources.Load("Prefab/GameOverDetector");
        enemy = (GameObject) Resources.Load("Prefab/Enemy");
        //initiates lists
        roofObjects = new List<GameObject>();
        obstacleObjects = new List<GameObject>();
        //finds roof objects in scene, and adds them to the array
        tmp = GameObject.FindGameObjectsWithTag("Wall");
        for (int i = 0; i < tmp.Length; i++) roofObjects.Add(tmp[i]);
        //finds obstacle objects in scene, and adds them to the obstacleList
        tmp = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i < tmp.Length; i++) obstacleObjects.Add(tmp[i]);
        player1RigidBody = player1.GetComponent<Rigidbody>();
        if (player2 != null) player2RigidBody = player2.GetComponent<Rigidbody>();
        cameraSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        cameraWidth = cameraSize.x;
        worldMovePointX = movingPointX * cameraWidth;
        spawnPointX = cameraSize.x + 5;
        MakeFrame(GameOverDetection);
        enemySpawnIndex = 0;
        StartCoroutine(SpawnEnemies(EnemySpawnDelay));
    }

    // Update is called once per frame
    void Update() {
        distanceTravelled -= Time.fixedDeltaTime * worldVelocity.x;
        distance = (int) distanceTravelled;
        distanceField.text = distance + " Metres Travelled";
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

        lastObstacle = obstacleObjects[obstacleObjects.Count - 1];
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
        if (roofObjects[roofObjects.Count - 1].transform.position.x < 3.89) {
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
            if (obstacleObjects[0].transform.position.x < -40) {
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
        if (lastObstacle.transform.position.x + lastObstacle.transform.localScale.x/2 < cameraSize.x) {
            float yTop = Random.Range(-cameraSize.y + 1, 1);
            float yScale = yTop + cameraSize.y;
            GameObject shitVarName = (GameObject)Instantiate(obstacle, new Vector3(spawnPointX, yTop - yScale/2, 0), Quaternion.identity);
            shitVarName.transform.localScale = new Vector3(Random.Range(5, 8), yScale, 1);
            obstacleObjects.Add(shitVarName);
        }
    }

    IEnumerator SpawnEnemies(float delay) {
        yield return new WaitForSeconds(delay);
        GameObject clone = (GameObject) Instantiate(enemy, new Vector3(-1.5f * cameraSize.x, 0, 0), Quaternion.identity);
        Debug.Log(initialEnemySpeed + deltaEnemySpeed * enemySpawnIndex);
        clone.GetComponent<Enemy1>().moveSpeed = initialEnemySpeed + (deltaEnemySpeed * enemySpawnIndex);
        enemySpawnIndex++;
        StartCoroutine(SpawnEnemies(delay));
    }
}
