using UnityEngine;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour {
    public GameObject enemy;
    RectTransform healthBar;
    RectTransform healthPrefab;
    RectTransform health;
    RectTransform thisRectTransform;
    int YPos;
    int maxX;
    int minX;

    // Use this for initialization
    void Start () {
        if (enemy == null) Debug.Log("enemy object isn't set in the enemy healthbar class");
        GameObject fuckUnity = (GameObject)(Resources.Load("Prefab/UI/EnemyHealth/Health"));
        //GetComponent<RectTransform>().localScale = Camera.main.WorldToViewportPoint(new Vector3(2, 2, 2));
        healthPrefab = fuckUnity.GetComponent<RectTransform>();
        health = Instantiate(healthPrefab);
        health.transform.parent = gameObject.transform;
        health.anchoredPosition = Vector2.zero;
        thisRectTransform = gameObject.GetComponent<RectTransform>();
        //sets the red part relative to player
        thisRectTransform.localScale = new Vector3(enemy.transform.lossyScale.x, enemy.transform.lossyScale.x, enemy.transform.lossyScale.x) * 1;
        //sets the green part to == red part
        health.localScale = new Vector2(thisRectTransform.rect.width, thisRectTransform.rect.height);
        YPos = 0;
        maxX = 0;
        minX = (int)Mathf.Round(thisRectTransform.rect.width);
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(thisRectTransform.localScale);
        if (enemy != null) thisRectTransform.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector2 (enemy.transform.position.x, enemy.transform.position.y + .5f));
	}
    
    //x is between 0, and 100. 100 is full health
    public void SetValue(int x) {
        int newXPos = x * minX / 100 - minX;
        health.anchoredPosition = new Vector2(newXPos, YPos);
    }
}
