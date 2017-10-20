using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3D : MonoBehaviour {

	MeshFilter slice;

	// Use this for initialization
	void Start () 
	{
		slice = GetComponentsInChildren<MeshFilter> ()[1];
		slice.mesh = new Mesh ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnCollisionStay(Collision c)
	{
		ContactPoint[] vects = c.contacts;
		List<Vector3> crossection = new List<Vector3>();
		foreach(ContactPoint v in vects)
		{
			crossection.Add (v.point);
		}
		print (crossection.ToArray().Length);
		slice.mesh.vertices = crossection.ToArray();
	}
}
