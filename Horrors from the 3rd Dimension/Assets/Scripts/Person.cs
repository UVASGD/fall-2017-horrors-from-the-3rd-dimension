using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

	private Vector3 reflected;
	private Rigidbody rb;
	private float spin;
	private Vector3 translate = new Vector3(0.0f, 0.0f, 0.0f);
	private float force;
	private float torque;

	public float health = 0.0f;
	public float maxHealth = 1000.0f;
	public float spinDrag;
	public float spinSpeed;
	public float maxSpeed;
	public float drag; //set drag to 1 for minimal acceleration, 0 for most acceleration
	public bool dirMovement;
	private GameObject obj;

	public bool grounded = true;

	ParticleSystem bloodTrail;

	// Use this for initialization
	void Start ()
	{
		health = maxHealth;
		rb = GetComponent<Rigidbody>();
		spin = 0;
		obj = this.gameObject;

		if(spinDrag == 1)//prevents divide by 0 error
		{
			spinDrag -= 0.00001f;
		}
		if(drag == 1)
		{
			drag -= 0.00001f;
		}
		force = (1 / (1 - drag) - 1) * maxSpeed;
		torque = (1 / (1 - spinDrag) - 1) * spinSpeed;
		bloodTrail = GetComponentInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject hitObject = collision.collider.gameObject;
		print("object" + hitObject);
		if (!hitObject.CompareTag("Damageable"))
		{
			return;
		}
		//calculates angle between player and the object they are colliding with

		float cornerAngle = 90;

		reflected = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
		float collisionAngle = Mathf.Acos(Vector3.Dot(reflected,transform.forward)/reflected.magnitude/transform.forward.magnitude);
		if (collisionAngle > Mathf.PI/2)
		{
			collisionAngle -= Mathf.PI;
			collisionAngle *= -1;
		}//gets angle between surface and center of object with vertex at the collision point in radians.

		Vector3 normal = Vector3.Reflect(rb.velocity,collision.contacts[0].normal) - rb.velocity;
		normal = normal.normalized;

		collisionAngle /= Mathf.PI;
		collisionAngle *= (180 - cornerAngle);//converts to degrees and finds angle between the two surfaces.

		float speed = Mathf.Abs(Vector3.Dot(normal,collision.relativeVelocity));//gets relative speed between two objects
		//(hitObject.GetComponent<EnemyMover>()).health -= collisionAngle * speed;
		// print(hitObject.GetComponent<EnemyMover>().health);
		if (!float.IsNaN(collisionAngle * speed))
		{
			hitObject.GetComponentInParent<EnemyMover>().RecieveDamage(collisionAngle * speed);
		}


		//bloodTrail.startSize = (maxHealth / health) / 10000;
	}
}
