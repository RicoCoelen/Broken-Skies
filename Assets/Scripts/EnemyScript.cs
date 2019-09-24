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
    public Transform groundDetection;

    private Animator anim;
    private Rigidbody2D rb;

    // enemy rotation
    bool facingRight = true;
    public bool flipped = false;
    public bool death = false;
    float yRotation = 0;
    float xRotation = 0;
    bool movingRight;
    private float Range;

    // flash red
    public float flashTime;
    Color origionalColor;
    public SpriteRenderer renderer;

    
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        origionalColor = renderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        // get range player and this object
        Range = Vector2.Distance(transform.position, player.transform.position);
       
        // follow player if close
        if (Range <= 15f)
        {
            Vector2 velocity = new Vector2((transform.position.x - player.transform.position.x) * speed, 0);
            rb.velocity = -velocity;
        }
        else
        {
            // just walk left to right
            if (movingRight == true)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
            }
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

    void FlashRed()
    {
        renderer.color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    void ResetColor()
    {
        renderer.color = origionalColor;
    }
}
