using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rise : MonoBehaviour {
    private Rigidbody rb;
    private Mesh crossSection;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        crossSection = new Mesh();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x,transform.position.y + .5f * Time.deltaTime, transform.position.z);

        ((MeshFilter)GetComponentInChildren<MeshFilter>()).mesh = crossSection;
        //((MeshCollider)GetComponentInChildren<MeshCollider>()) = crossSection;
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] intersections = collision.contacts;
        List<Vector3> verts = new List<Vector3>();

        foreach(ContactPoint c in intersections)
        {
            verts.Add(c.point);
        }

        crossSection.SetVertices(verts);


    }
}
