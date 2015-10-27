using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {
    public static GameObject[] tutorialBoxes;
    static int tutorialBoxIndex;
    static GameObject tutorialBoxInstance;
	// Use this for initialization
	void Start () {
        tutorialBoxIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public static void LoadNextTutorialBox() {
        if (tutorialBoxIndex < tutorialBoxes.Length) {
            if (tutorialBoxInstance == null) Destroy(tutorialBoxInstance);
            tutorialBoxIndex++;
        }
        else {
            print("tutorialBoxIndex is high as fuck");
        }
    }
}
