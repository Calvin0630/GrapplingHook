using UnityEngine;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour {
    public GameObject enemy;
    RectTransform healthBar;
    RectTransform healthPrefab;
    RectTransform health;
    int YPos;
    int maxX;
    int minX;

    // Use this for initialization
    void Start () {
        if (enemy == null) Debug.Log("enemy object isn't set in the enemy healthbar class");
        GameObject fuckUnity = (GameObject)(Resources.Load("Prefab/UI/EnemyHealth/Health"));
        healthPrefab = fuckUnity.GetComponent<RectTransform>();
        health = Instantiate(healthPrefab);
        health.transform.parent = gameObject.transform;
        health.anchoredPosition = Vector2.zero;
        healthBar = gameObject.GetComponent<RectTransform>();
        YPos = 0;
        maxX = 0;
        minX = (int)Mathf.Round(healthBar.rect.width);
    }
	
	// Update is called once per frame
	void Update () {
        if (enemy != null) healthBar.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector2 (enemy.transform.position.x, enemy.transform.position.y + .5f));
	}
    
    //x is between 0, and 100. 100 is full health
    public void SetValue(int x) {
        int newXPos = x * minX / 100 - minX;
        Debug.Log(health.gameObject.name);
        health.anchoredPosition = new Vector2(newXPos, YPos);
        Debug.Log(health.anchoredPosition.x);
    }
}
