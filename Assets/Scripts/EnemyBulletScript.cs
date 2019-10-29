using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float minDamage;
    public float maxDamage;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Kill")
        {
            // nothing
        }
        else
        {
            Debug.Log(collision.name);
            HealthScript Player = collision.GetComponent<HealthScript>();
            if (Player != null)
            {
                Player.TakeDamage(Random.Range(minDamage, maxDamage));
            }
            Destroy(gameObject);
        }
    }
}
