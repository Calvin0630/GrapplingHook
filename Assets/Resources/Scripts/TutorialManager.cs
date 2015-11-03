using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {
    public GameObject[] tutorialBoxes;
    static int tutorialBoxIndex;
    static GameObject tutorialBoxInstance;
    // Use this for initialization
    void Start() {
        ObstacleSpawner.worldMovingIsEnabled = false;
        tutorialBoxIndex = 0;
        tutorialBoxInstance = Instantiate(tutorialBoxes[tutorialBoxIndex]);
        tutorialBoxInstance.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
    }

    // Update is called once per frame
    void Update() {
        if (LoadNextBoxCondition(tutorialBoxIndex)) {
            LoadNextTutorialBox();
        }
    }
    public void LoadNextTutorialBox() {
        if (tutorialBoxIndex < tutorialBoxes.Length) {
            if (tutorialBoxInstance != null) Destroy(tutorialBoxInstance.gameObject);
            tutorialBoxIndex++;
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
                return Input.GetButtonDown("JumpPlayer1") || Input.GetButtonDown("JumpPlayer2");
            case 1:
                return Input.GetButtonDown("HorizontalPlayer1") || Input.GetButtonDown("HorizontalPlayer2");
            case 2:
                return Input.GetMouseButtonUp(0);
            case 3:
                return Input.GetKeyDown(KeyCode.Return);
            default:
                return false;
        }
    }
}