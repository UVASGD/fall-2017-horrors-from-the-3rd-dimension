using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    public RectTransform dimension;
    public float maxLength;
    // Use this for initialization
    void Start () {
        dimension = (RectTransform)GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHealth()
    {
        float maxHealth = GetComponentInParent<PlayerController>().maxHealth;
        float health = GetComponentInParent<PlayerController>().health;
        if (health < 0)
        {
            health = 0;
        }
        dimension.localScale = new Vector3(maxLength * health / maxHealth, dimension.localScale.y, dimension.localScale.z);
        //dimension.localPosition = new Vector3(-50 * dimension.localScale.x, dimension.localPosition.y, dimension.localPosition.z);
    }
}
