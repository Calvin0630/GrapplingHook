using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour {
    static Image image;
	// Use this for initialization
	void Start () {
        image = gameObject.GetComponent<Image>();
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Vertical;
        image.fillOrigin = (int)Image.OriginVertical.Top;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //takes a float between 0 and 1 and sets the energy accordingly
    public static void SetValue(float energy) {
        if (energy > 1) energy = 1;
        image.fillAmount = energy;
    }
}
