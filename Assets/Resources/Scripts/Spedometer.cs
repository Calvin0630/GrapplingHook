using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spedometer : MonoBehaviour {
    Text speedText;
    Image image;
    float maxSpeed = 50;
    float playerSpeed;


	// Use this for initialization
	void Start () {
        image = gameObject.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        playerSpeed = ObstacleSpawner.worldVelocityX;
        if (playerSpeed > maxSpeed) {
            image.fillAmount=1;
            //now do a cool particle effect
        }
        else {
            image.fillAmount = playerSpeed / maxSpeed ;
        } 
        //print(playerSpeed / maxSpeed);
	}
}
