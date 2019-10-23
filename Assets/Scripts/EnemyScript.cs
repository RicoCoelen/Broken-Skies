using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float health = 100f;
    public float speed;
    public float distance;
    public float knockBackForce;
    public GameObject player;
    public GameObject groundDetection;
    public GameObject wallDetection;

    private Animator anim;
    private Rigidbody2D rb;

    // enemy rotation
    public bool facingRight = true;
    public bool flipped = false;
    public bool death = false;
    float yRotation = 0;
    float xRotation = 0;

    // checks
    public bool isGrounded = false;
    public bool isWalled = false;

    // flash red
    public float flashTime;
    Color origionalColor;
    public SpriteRenderer renderer;

    // huidige status
    private State cState;

    // current enemy target
    public GameObject currentTarget = null;

    // State Types
    public enum State
    {
        IDLE = 0,
        PATROLLING = 1,
        CHASING = 2,
        FLEEING = 3
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        origionalColor = renderer.color;

        // always start idle
        cState = State.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        // check state and handle accordingly
        switch (cState)
        {
            case State.IDLE:
                EnemyIdle();
                break;

            case State.PATROLLING:
                EnemyPatrolling();
                break;

            case State.CHASING:
                EnemyChasing();
                break;

            case State.FLEEING:
                EnemyFleeing();
                break;

            default:
                cState = State.IDLE;
                break;
        }

        FlipCorrection();
    }

    void EnemyFleeing()
    {

        if (currentTarget == null)
        {
            cState = State.PATROLLING;
        }
        else
        {
            // move current rigidbody to tracked player
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + -direction * speed * Time.deltaTime);
        }
    }

    void EnemyIdle()
    {
        // todo but just stand still
        cState = State.PATROLLING;
    }

    void EnemyPatrolling()
    {
        if (currentTarget != null)
        {
            cState = State.CHASING;
        }
        else
        {
            if (isGrounded == false || isWalled == true)
            {
                speed *= -1;
                facingRight = !facingRight;
            }
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    void EnemyChasing()
    {
        if (currentTarget == null)
        {
            cState = State.PATROLLING;
        }
        else
        {
            // move current rigidbody to tracked player
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        }
    }

    private void FlipCorrection()
    {
        // check velocity and let enemy stay left after walking
        if (rb.velocity.x > 0 && !facingRight || rb.velocity.x < 0 && facingRight)
        {
            facingRight = !facingRight;
        }

        // rotate x axis
        if (facingRight == true)
        {
            yRotation = 0;
        }
        else
        {
            yRotation = 180;
        }

        // rotate y axis
        if (flipped == true)
        {
            xRotation = 180;
            rb.gravityScale = -1;
        }
        else
        {
            xRotation = 0;
            rb.gravityScale = 1;
        }

        // rotate enemy
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void TakeDamage(float amount)
    {
        // add knockback
        Vector3 moveDirection = transform.position - player.transform.position;
        rb.AddForce(moveDirection.normalized * knockBackForce);

        // remove health
        health -= amount;

        // flash red
        FlashRed();

        // if no health die
        if (health <= 0f)
        {
            Destroy(gameObject);
        }

        // maybe show health above head?
        //healthBar.value = Mathf.Clamp(health, 0, 100f);
    }

    private void FlashRed()
    {
        renderer.color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    private void ResetColor()
    {
        renderer.color = origionalColor;
    }
}
