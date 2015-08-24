using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {
    int XPos;
    int maxY;
    int minY;
    RectTransform[] tmp;
    RectTransform background;
    RectTransform energy;
	// Use this for initialization
	void Start () {
        tmp = gameObject.GetComponentsInChildren<RectTransform>();
        foreach(RectTransform rec in tmp) {
            if (rec.gameObject.name == "Background") background = rec;
            else if (rec.gameObject.name == "Energy") energy = rec;
        }
        XPos = 0;
        maxY = 0;
        minY = (int) Mathf.Round(background.rect.height);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    //takes a value from 0-100 and sets the health location accordingly
    public void SetValue(int x) {
        int newYPos = x * minY / 100 - minY;
        energy.anchoredPosition = new Vector2(XPos, newYPos);
    }
}
