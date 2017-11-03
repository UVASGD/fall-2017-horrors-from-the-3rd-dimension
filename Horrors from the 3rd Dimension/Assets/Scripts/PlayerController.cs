using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
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

    void Start () {
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

    // Update is called once per frame
    void Update () {
        
        CheckInput();
        ApplyDrag();
        MoveObject();
        RotateObject();

		if (grounded) {
			KeepObjectOnPlane ();
		}

		if (health < maxHealth) {
			bloodTrail.Emit (1);
			if (health < 0) 
			{
				bloodTrail.startSize = 1000;
				bloodTrail.Emit (2);
				bloodTrail.transform.parent = this.transform.parent;
                Destroy(obj);
			}
		}

		ParticleSystem.MinMaxCurve size = bloodTrail.main.startSize;
		size.constantMin = .1f;
		size.constantMax = 1-health/maxHealth;
		ParticleSystem.MainModule m = bloodTrail.main;
		m.startSize = size;

		if (health < maxHealth - (.05 * maxHealth)) {
			bloodTrail.Emit (1);
		}
    }

    void ApplyDrag()
    {
        translate *= (1 - drag);
        spin *= (1 - spinDrag);
    }

    void KeepObjectOnPlane()
    {
        transform.position = (new Vector3(transform.position.x, 0, transform.position.z));//keeps player on plane
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);//keeps x and z rotation 0
    }

    void RotateObject()
    {
        rb.angularVelocity = new Vector3(0, spin / Time.deltaTime, 0);//controls rotation
    }

    void MoveObject()
    {
        if (!dirMovement)
        {
            rb.velocity = translate / Time.deltaTime;
        }
        else
        {
            ConvertMotion();//changes the up/down motion into motion relative to the object itself, then applies it
        }
    }

    void ConvertMotion()
    {
        float angle = transform.eulerAngles.y;
        angle = angle / 180 * Mathf.PI;
        angle += Mathf.PI/4;
        angle %= 2 * Mathf.PI;

		Vector3 move = new Vector3(Mathf.Sin(angle),rb.velocity.y,Mathf.Cos(angle));
        rb.velocity = move*translate[2]/Time.deltaTime;
    }

    void CheckInput()
    {
        MoveX();
        MoveZ();
        Rotate();
    }

    void MoveX()
    {
        if (!dirMovement)
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))//if moving in more than one direction, slows movement accordingly
                {
                    translate[0] -= force * 0.7071067812f;
                }
                else
                {
                    translate[0] -= force;
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    translate[0] += force * 0.7071067812f;
                }
                else
                {
                    translate[0] += force;
                }
            }
        }
    }

    void MoveZ()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (!dirMovement && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                translate[2] += force * 0.7071067812f;
            }
            else
            {
                translate[2] += force;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (!dirMovement && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                translate[2] -= force * 0.7071067812f;
            }
            else
            {
                translate[2] -= force;
            }
        }
    }

    void Rotate()
    {
        if (!dirMovement)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                spin -= torque;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                spin += torque;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                spin -= torque;
            }
            if (Input.GetKey(KeyCode.D))
            {
                spin += torque;
            }
        }
    }
}