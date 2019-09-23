using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    // rigidbody
    Rigidbody2D rb;

    // jump force
    Vector3 jump;
    public float jumpForce = 10.0f;
    public bool isGrounded;

    // speed and movement of rigidbody
    public float speed = 40f;
    float move;

    // flipped character and gravity with sprite
    bool facingRight = true;
    public bool flipped = false;
    float yRotation = 0;
    float xRotation = 0;

    // double click vars
    public float ButtonCooler = 0.5f; // Half a second before reset
    int ButtonCount = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // get input from player
        move = Input.GetAxis("Horizontal");
        
        // move rigidbody accordingly
        rb.velocity = new Vector2(speed * move, rb.velocity.y);

        // check velocity and let player stay left after walking
        if (rb.velocity.x > 0 && !facingRight || rb.velocity.x < 0 && facingRight)
        {
            facingRight = !facingRight;
        }

        // check space for jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // jump
            rb.AddForce(jump * jumpForce, ForceMode2D.Force);

            // flip gravity if double tapped
            if (ButtonCooler > 0 && ButtonCount == 1)
            {
                flipped = !flipped;
            }
            else
            {
                ButtonCooler = 0.5f;
                ButtonCount += 1;
            }
        }

        // reset doubletaptimer
        if (ButtonCooler > 0)
        {
            ButtonCooler -= 1 * Time.deltaTime;
        }
        else
        {
            ButtonCount = 0;
        }

        // flip object to corresponding side
        if (facingRight == true)
        {
            yRotation = 0;
        }
        else
        {
            yRotation = 180;
        }

        // flip gravity 
        if (flipped == true)
        {
            rb.gravityScale = -1;
            jump = new Vector3(0.0f, -jumpForce, 0.0f);
            xRotation = 180;
        }
        else
        {
            rb.gravityScale = 1;
            jump = new Vector3(0.0f, jumpForce, 0.0f);
            xRotation = 0;
        }

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
