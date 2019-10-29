using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyScript : MonoBehaviour
{
    public float health = 100f;
    public float speed;
    public float patrolSpeed;
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

    // checks from somewhere else
    public bool isGrounded = false;
    public bool isWalled = false;
    public GameObject currentTarget = null;

    // flash red
    public float flashTime;
    Color origionalColor;
    public SpriteRenderer renderer;
    public GameObject effect;

    // random speeds
    public float speedMin;
    public float speedMax;
    public float patrolSpeedMin;
    public float patrolSpeedMax;

    // huidige status
    public State cState;

    public Slider healthBar;

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
        speed = Random.Range(speedMin, speedMax);
        patrolSpeed = Random.Range(patrolSpeedMin, patrolSpeedMax);

        healthBar.value = health;
        healthBar.maxValue = health;

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
            anim.SetBool("isRunning", false);
            cState = State.PATROLLING;
        }
        else
        {
            // move current rigidbody to tracked player
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + -direction * speed * Time.deltaTime);
            anim.SetBool("isRunning", true);
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
            anim.SetBool("isWalking", false);
        }
        else
        {
            if (isGrounded == false || isWalled == true)
            {
                speed *= -1;
                patrolSpeed *= -1;
                facingRight = !facingRight;
            }
            anim.SetBool("isWalking", true);
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
        }
    }

    void EnemyChasing()
    {
        if (currentTarget == null)
        {
            cState = State.PATROLLING;
            anim.SetBool("isRunning", false);
        }
        else
        {
            rb.velocity = Vector2.zero;
            // move current rigidbody to tracked player
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            direction.y = 0;

            if (currentTarget.transform.position.x > transform.position.x)
            {
                if (isGrounded == true)
                {
                    rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
                }
                facingRight = true;
                speed = Mathf.Abs(speed);
            }
            else
            {
                if (isGrounded == true)
                {
                    rb.MovePosition(transform.position + -direction * speed * Time.deltaTime);
                }
                facingRight = false;
                speed = -Mathf.Abs(speed);
            }

            if (isGrounded == false)
            {
                anim.SetBool("isRunning", false);
            }
            else
            {
                anim.SetBool("isRunning", true);
            }
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
            healthBar.transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            yRotation = 180;
            healthBar.transform.parent.localRotation = Quaternion.Euler(0, 180, 0);
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
        moveDirection.y = 0;
        rb.AddForce(moveDirection.normalized * knockBackForce);

        // remove health
        health -= amount;

        healthBar.value = Mathf.Clamp(health, 0, 100f);

        // flash red
        FlashRed();

        // if no health die
        if (health <= 0f)
        {
            Instantiate(effect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Kill")
        {
            TakeDamage(100);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Kill")
        {
            TakeDamage(100);
        }
    }
}
