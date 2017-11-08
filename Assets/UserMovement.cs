using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;
    public float speed;
    public float movement;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        movement = Input.GetAxis("Horizontal");

        if (movement < 0)
        {
            rigidBody.velocity = new Vector2(movement * speed, rigidBody.velocity.y);
        }
        else if(movement > 0)
        {
            rigidBody.velocity = new Vector2(movement * speed, rigidBody.velocity.y);
        }

        else if (movement == 0)
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

        movement = Input.GetAxis("Vertical");

        if (movement < 0)
        {
            rigidBody.velocity = new Vector2( rigidBody.velocity.x, movement * speed);
        }
        else if (movement > 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, movement * speed);
        }

        else if (movement == 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,0);
        }

    }
}
