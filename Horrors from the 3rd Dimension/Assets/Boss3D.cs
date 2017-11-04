using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3D : MonoBehaviour {

	GameObject player;
	public int state = -1;
	public float savedTime;
	public float health;
	MeshFilter cubeFilter;
	Mesh cube;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
		cubeFilter = GetComponent<MeshFilter> ();
		cube = cubeFilter.mesh;
		Vector3[] vs = cube.vertices;
		int[] ts = cube.triangles;
		List<Vector3> orderedVerts = new List<Vector3> ();
		for (int i = 0; i < cube.vertexCount; i++) 
		{
			foreach (int t in ts) 
			{
				if (t == i) 
				{
					orderedVerts.Add (vs [i]);
				}
			}
		}
		for (int i = 1; i < cube.vertexCount; i++) 
		{
			CapsuleCollider cc = new CapsuleCollider();
			cc.radius = .1f;
			cc.height = gameObject.transform.localScale.x;

			float x = (vs [i].x - vs [i - 1].x) / 2f;
			float y = (vs [i].y - vs [i - 1].y) / 2f;
			float z = (vs [i].z - vs [i - 1].z) / 2f;
			cc.center = new Vector3 (x, y, z);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (state) {
		case -1:
			if (player.transform.position.z > 45 && player.transform.position.z < 75 && player.transform.position.x > 5 && player.transform.position.x < 40) {
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

	void OnCollisionEnter(Collision c)
	{
		GameObject victim = c.collider.gameObject;
		ContactPoint[] cps = c.contacts;
		List<Vector3> points = new List<Vector3>();
		foreach (ContactPoint p in cps) 
		{
			points.Add (p.normal);
			print (p.point);
		}
		if (victim.tag.CompareTo ("Player") == 0) 
		{
			
		}
	}


}
