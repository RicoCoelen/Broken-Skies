using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterController2D : MonoBehaviour
{
    // rigidbody
    Rigidbody2D rb;
    // animator
    public Animator animator;
    // camera
    public GameObject camera;
    public CinemachineVirtualCamera vc;
    public CinemachineTargetGroup vct;

    // jump force
    Vector3 jump;
    public float jumpForce = 10.0f;
    public bool isGrounded = false;
    public bool canFlip = false;

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

    // attacking
    public bool isAttacking = false;
    public bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vc = camera.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        // get input from player
        move = Input.GetAxis("Horizontal");

        // check space for jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // jump
            rb.AddForce(jump * jumpForce, ForceMode2D.Force);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canFlip)
            {
                // flip gravity if double tapped
                if (ButtonCooler < 0.5f && ButtonCount == 1)
                {
                    flipped = !flipped;
                    rb.velocity = Vector2.zero;
                    canFlip = false;
                }
                else
                {
                    ButtonCooler = 0.5f;
                    ButtonCount += 1;
                }
            }
        }

        // shooting
        if (Input.GetKeyDown(KeyCode.E))
        {
            isShooting = true;
            if (vc.gameObject.activeSelf == true)
            {
                vc.GetComponent<CinemachineCameraShaker>().ShakeCamera(0.1f);
            }
            else
            {
                vct.GetComponent<CinemachineCameraShaker>().ShakeCamera(0.1f);
            }
        }
        else
        {
            isShooting = false;
        }
    }

    private void FixedUpdate()
    {
        // move rigidbody accordingly
        rb.velocity = new Vector2(speed * move, rb.velocity.y);

        // check velocity and let player stay left after walking
        if (rb.velocity.x > 0 && !facingRight || rb.velocity.x < 0 && facingRight)
        {
            facingRight = !facingRight;
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
            vc.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.35f;
        }
        else
        {
            rb.gravityScale = 1;
            jump = new Vector3(0.0f, jumpForce, 0.0f);
            xRotation = 0;
            vc.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.65f;
        }

        // rotate player
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

        // set animatior vars
        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetBool("IsGrounded", isGrounded);
    }
}
