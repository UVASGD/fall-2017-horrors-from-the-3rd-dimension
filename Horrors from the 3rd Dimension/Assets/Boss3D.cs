using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3D : MonoBehaviour {

	GameObject player;
	public int state = -1;
	public float savedTime;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (state) {
		case -1:
			if (player.transform.position.z > 45 && player.transform.position.z < 75 && player.transform.position.x > 5 && player.transform.position.z < 40) {
				state = 0;
			}
			break;
		case 0:
			transform.position = new Vector3 (transform.position.x, transform.position.y-.2f, transform.position.z);
			if (transform.position.y <= 0) 
			{
				state = 3;
				savedTime = Time.time;
			}
			break;
		case 1:
			transform.position = new Vector3 (transform.position.x, transform.position.y + .2f, transform.position.z);
			if (transform.position.y >= 5) 
			{
				state = 2;
				savedTime = Time.time;
			}
			break;
		case 2:
			transform.position = new Vector3 (player.transform.position.x, player.transform.position.y+5, player.transform.position.z);
			if (Time.time-savedTime > 7) {
				state = 0;
			}
			break;
		case 3:
			transform.Rotate (new Vector3 (0, 1, 0));
			if (Time.time-savedTime > 7) {
				state = 1;
			}
			break;
		}
	}


}
