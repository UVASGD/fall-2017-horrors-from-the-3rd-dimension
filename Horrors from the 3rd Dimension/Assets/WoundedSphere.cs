using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundedSphere : MonoBehaviour {

	int state = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (state) 
		{
		case 0:
			transform.position = new Vector3 (transform.position.x, transform.position.y-.1f, transform.position.z);
			break;
		case 1:
			transform.position = new Vector3 (transform.position.x, transform.position.y+.1f, transform.position.z);
			break;
		}

		if (transform.position.y < -12) 
		{
			state = 1;
		}

		if (transform.position.y > 0) 
		{
			state = 0;
		}
	}

	void OnCollisionEnter(Collision C)
	{

	}
}
