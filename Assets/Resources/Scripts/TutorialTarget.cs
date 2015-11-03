using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TutorialTarget : MonoBehaviour {
    public static bool mouseIsOver;
	// Use this for initialization
	void Start () {
        mouseIsOver = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (EventSystem.current.IsPointerOverGameObject()) {
            mouseIsOver = true;
        }
    }
    
}
