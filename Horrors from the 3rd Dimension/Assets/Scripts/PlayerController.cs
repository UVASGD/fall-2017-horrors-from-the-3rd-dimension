using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private float spin;
    private Vector3 translate = new Vector3(0.0f, 0.0f, 0.0f);
    private float force;
    private float torque;

    public float spinDrag;
    public float spinSpeed;
    public float maxSpeed;
    public float drag; //set drag to 1 for minimal acceleration, 0 for most acceleration
    public bool dirMovement;
    
    void Start () {
        rb = GetComponent<Rigidbody>();
        spin = 0;

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
    }

    // Update is called once per frame
    void Update () {
        checkInput();
        applyDrag();
        moveObject();
        rotateObject();
        keepObjectOnPlane();

        BoxCollider b = this.GetComponent<BoxCollider>();//not used yet
    }

    void applyDrag()
    {
        translate *= (1 - drag);
        spin *= (1 - spinDrag);
    }

    void keepObjectOnPlane()
    {
        transform.position = (new Vector3(transform.position.x, 0, transform.position.z));//keeps player on plane
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);//keeps x and z rotation 0
    }

    void rotateObject()
    {
        rb.angularVelocity = new Vector3(0, spin / Time.deltaTime, 0);//controls rotation
    }

    void moveObject()
    {
        if (!dirMovement)
        {
            rb.velocity = translate / Time.deltaTime;
        }
        else
        {
            convertMotion();//changes the up/down motion into motion relative to the object itself, then applies it
        }
    }

    void convertMotion()
    {
        float angle = transform.eulerAngles.y;
        angle = angle / 180 * Mathf.PI;
        angle += Mathf.PI/4;
        angle %= 2 * Mathf.PI;

        Vector3 move = new Vector3(Mathf.Sin(angle),0,Mathf.Cos(angle));
        rb.velocity = move*translate[2]/Time.deltaTime;
    }

    void checkInput()
    {
        moveX();
        moveZ();
        rotate();
    }

    void moveX()
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

    void moveZ()
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

    void rotate()
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