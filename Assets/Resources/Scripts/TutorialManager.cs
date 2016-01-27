using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
    public float boxLoadDelay;
    public GameObject[] tutorialBoxes;
    static int tutorialBoxIndex;
    static GameObject tutorialBoxInstance;
    bool inTransition = false;
    GameObject framePiece;
    Vector3 cameraSize;
    GameObject enemy;
    GameObject idlerPrefab;
    GameObject turretPrefab;
    // Use this for initialization
    void Start() {
        cameraSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        framePiece = (GameObject)Resources.Load("Prefab/TutorialFrame");
        idlerPrefab = (GameObject)Resources.Load("Prefab/Enemies/Idler");
        turretPrefab = (GameObject)Resources.Load("Prefab/Enemies/Turret");
        MakeFrame();
        BuildingSpawner.worldMovingIsEnabled = false;
        tutorialBoxIndex = 0;
        tutorialBoxInstance = Instantiate(tutorialBoxes[tutorialBoxIndex]);
        tutorialBoxInstance.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
    }
    
    // Update is called once per frame
    void Update() {
        if (LoadNextBoxCondition(tutorialBoxIndex) && !inTransition) {
            StartCoroutine(LoadNextTutorialBox());
        }
    }

    public IEnumerator LoadNextTutorialBox() {
        if (tutorialBoxIndex < tutorialBoxes.Length) {
            if (tutorialBoxInstance != null) Destroy(tutorialBoxInstance.gameObject);

            if (tutorialBoxIndex == 2) {
                //spawn idler
                enemy = (GameObject)Instantiate(idlerPrefab);
                enemy.GetComponent<Idler>().initialHealth = 5;
            }
            else if (tutorialBoxIndex == 3) {
                //load a turret and shit
            }
            else if (tutorialBoxIndex == 5) {
                print("tutorial is over");
                //SceneManager.LoadScene("MainScreen");
            }
            inTransition = true;
            yield return new WaitForSeconds(boxLoadDelay);
            inTransition = false;
            tutorialBoxIndex++;
            //loads next box
            tutorialBoxInstance = Instantiate(tutorialBoxes[tutorialBoxIndex]);
            tutorialBoxInstance.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);

        }
        else {
            print("tutorialBoxIndex is high as fuck");
        }
    }

    //
    public bool LoadNextBoxCondition(int index) {

        switch (index) {
            case 0:
                return Input.GetButtonDown("Jump");
            case 1:
                return Input.GetButtonDown("HorizontalPlayer1") || Input.GetButtonDown("HorizontalPlayer2");
            case 2:
                return Input.GetMouseButtonUp(0);
            case 3:
                return Input.GetKeyDown(KeyCode.Return);
            case 4:
                return enemy == null;
            default:
                return false;
        }
    }

    //makes the GameOver frame
    void MakeFrame() {
        GameObject frameTemp;
        Vector3 framePieceScale = framePiece.transform.localScale;
        frameTemp = (GameObject)Instantiate(framePiece);
        frameTemp.transform.localScale = new Vector3(4 * cameraSize.x, 2, 2);
        frameTemp.transform.position = new Vector3(0, cameraSize.y + frameTemp.transform.localScale.y/2, 0);

        frameTemp = (GameObject)Instantiate(framePiece);
        frameTemp.transform.localScale = new Vector3(4 * cameraSize.x, 2, 2);
        frameTemp.transform.position = new Vector3(0, -(cameraSize.y + frameTemp.transform.localScale.y/2), 0);

        frameTemp = (GameObject)Instantiate(framePiece);
        frameTemp.transform.localScale = new Vector3(2, 4 * cameraSize.y + 2, 2);
        frameTemp.transform.position = new Vector3(cameraSize.x + frameTemp.transform.localScale.x/2, 0, 0);

        frameTemp = (GameObject)Instantiate(framePiece);
        frameTemp.transform.localScale = new Vector3(2, 4 * cameraSize.y + 2, 2);
        frameTemp.transform.position = new Vector3(-(cameraSize.x + frameTemp.transform.localScale.x/2), 0, 0);
    }
}