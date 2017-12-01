﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//This is rudimentary and will probably not be used because I don't really know what I'm doing but hopefully I learn something.

public class EnemyMover : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 translate = new Vector3();
    private Vector3 direction = new Vector3();
    private GameObject player;
    private GameObject text;
    private float force;
    private float targetAngle;
    private float distanceToPlayer;
    private bool inRangeOfPlayer;
    private int selector;
    private System.Random r = new System.Random();
    private string[] foundDialogOptions = new string[] {"Stop right there!", "Hey, get back here!", "Hey you! Get back here!", "I’ll send you to the coroner!", "Finally! Something to do besides wander around! I call dibs on killing ‘im!", "I’ll destroy you all! Oh. There’s just one of you. Still, I’ll destroy ALL of you…"};
    private string[] escapeDialogOptions = new string[] { "Running away won’t make you any less vulnerable!", "Ugh.. I really need to get back into shape…", "Where’d he go?" };

    public Text dialog;
	public float maxHealth = 500.0f;
	public float health;
    public float cornerPos;
    public int numCorners;
    public float maxSpeed;
    public float drag;

	ParticleSystem bloodTrail;
    
	// Use this for initialization
	void Start () {
        health = maxHealth;
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        text = GameObject.Find("Text");
        force = (1 / (1 - drag) - 1) * maxSpeed;
		bloodTrail = GetComponentInChildren<ParticleSystem> ();
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.collider.gameObject;
        if (!hitObject.CompareTag("Player"))
        {
            return;
        }
        //calculates angle between player and the object they are colliding with

        float cornerAngle = 90;

        Vector3 reflected = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
        float collisionAngle = Mathf.Acos(Vector3.Dot(reflected, transform.forward) / reflected.magnitude / transform.forward.magnitude);
        if (collisionAngle > Mathf.PI / 2)
        {
            collisionAngle -= Mathf.PI;
            collisionAngle *= -1;
        }//gets angle between surface and center of object with vertex at the collision point in radians.

        Vector3 normal = Vector3.Reflect(rb.velocity, collision.contacts[0].normal) - rb.velocity;
        normal = normal.normalized;

        collisionAngle /= Mathf.PI;
        collisionAngle *= (180 - cornerAngle);//converts to degrees and finds angle between the two surfaces.

        float speed = Mathf.Abs(Vector3.Dot(normal, collision.relativeVelocity));//gets relative speed between two objects
        (hitObject.GetComponent<PlayerController>()).health -= collisionAngle * speed;
        ((HealthBar)hitObject.GetComponentInChildren<HealthBar>()).UpdateHealth();
    }

    public void RecieveDamage(float damage)
    {
        health -= damage;
        print("health" + health);
        if (health <= 0)
        {
			bloodTrail.startSize = 100;
			bloodTrail.Emit (2);
			bloodTrail.transform.parent = this.transform.parent;
            Destroy(this.gameObject);
        }

		//bloodTrail.startSize = (maxHealth / health) / 10000;
		ParticleSystem.MinMaxCurve size = bloodTrail.main.startSize;
		size.constantMin = .1f;
		size.constantMax = 1-health/maxHealth;
		ParticleSystem.MainModule m = bloodTrail.main;
		m.startSize = size;
    }

    // Update is called once per frame
    void Update () {
        locateTarget();
        if (distanceToPlayer < 10)
        {
            if (!inRangeOfPlayer) {
                dialog = text.GetComponent<Text>();
                selector = r.Next(0,foundDialogOptions.Length);
                dialog.text = foundDialogOptions[selector];
            }
            moveObject();
            rotateObject();
            inRangeOfPlayer = true;
        }
        else
        {
            if (inRangeOfPlayer)
            {
                dialog = text.GetComponent<Text>();
                selector = r.Next(0,escapeDialogOptions.Length);
                dialog.text = escapeDialogOptions[selector];
            }
            inRangeOfPlayer = false;
        }
        applyDrag();
        keepObjectOnPlane();

		if (health < maxHealth - (.05 * maxHealth)) {
			bloodTrail.Emit (1);
		}
	}

    void locateTarget()
    {
		if (player == null) {
			Application.LoadLevel (Application.loadedLevel);
		}

        direction = player.transform.position - transform.position;
        distanceToPlayer = direction.magnitude;
        direction.Normalize();
        targetAngle = Mathf.Acos(direction[0]);//in radians
        if (direction[2] < 0)
        {
            targetAngle *= -1;
            targetAngle += 2 * Mathf.PI;
        }
        targetAngle /= Mathf.PI / 180;
        targetAngle = 360 - targetAngle;
        if (distanceToPlayer > 4)
        {
            targetAngle += 45;
        }
        //print(targetAngle);
    }

    void rotateObject()
    {
        rb.angularVelocity = new Vector3(0, 0.005f * findMinDif() / Time.deltaTime, 0);//controls rotation
    }

    float findMinDif()//this mess should work for any shape with evenly spaced corners
    {
        targetAngle -= cornerPos;
        while (Mathf.Abs(targetAngle) > 360 / numCorners / 2)
        {
            targetAngle -= 360 / numCorners;
        }
        targetAngle -= transform.eulerAngles.y;
        while (targetAngle < -180 / numCorners)
        {
            targetAngle += 360 / numCorners;
        }
           
        return targetAngle;
    }

    void moveObject()
    {
        translate += direction;
        rb.velocity = force * translate / Time.deltaTime;
    }

    void applyDrag()
    {
        translate *= (1 - drag);
    }

    void keepObjectOnPlane()
    {
        transform.position = (new Vector3(transform.position.x, 0, transform.position.z));//keeps object on plane
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);//keeps x and z rotation 0
    }
}
