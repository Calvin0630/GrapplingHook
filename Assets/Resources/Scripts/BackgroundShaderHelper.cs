using UnityEngine;
using System.Collections;

public class BackgroundShaderHelper : MonoBehaviour {
    Material material;
	// Use this for initialization
	void Start () {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update () {
        material.SetInt("_ScreenWidth", Screen.width);
        material.SetInt("_ScreenHeight", Screen.height);
        material.SetFloat("_DistanceTravelled", BuildingSpawner.distanceTravelled);
    }
}
