using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spedometer : MonoBehaviour {
    Image image;
    float maxSpeed = 75;
    float playerSpeed;


	// Use this for initialization
	void Start () {
        if (ScoreManager.highScores.list.Count > 0 && ScoreManager.highScores.list[0].highSpeed > 25) {
            maxSpeed = ScoreManager.highScores.list[0].highSpeed;
        }
        else maxSpeed = 40;
        image = gameObject.GetComponent<Image>();
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Radial180;
        image.fillOrigin = (int) Image.OriginVertical.Bottom;
    }
	
	// Update is called once per frame
	void Update () {
        if (ObstacleSpawner.worldVelocityX > maxSpeed) playerSpeed = 75;
        else playerSpeed = ObstacleSpawner.worldVelocityX;
        print(playerSpeed / maxSpeed * 180);
        image.fillAmount = playerSpeed / maxSpeed ;
	}
}
