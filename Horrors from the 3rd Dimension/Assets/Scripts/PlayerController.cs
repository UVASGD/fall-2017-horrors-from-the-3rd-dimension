using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        checkInput();
        
    }
    void checkInput()
    {
        Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.W))
        {
            movement.x += 10.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.x -= 10.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.z += 10.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.z -= 10.0f;
        }
        movement.Normalize();
        transform.Translate(movement*Time.deltaTime*10, Space.World);
        Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement.y -= 120.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement.y += 120.0f;
        }
        transform.Rotate(movement * Time.deltaTime);
    }
}
