using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
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
        if (collision.tag == "Enemies")
        {
            EnemyScript Enemy = collision.GetComponent<EnemyScript>();
            if (Enemy != null)
            {
                Enemy.TakeDamage(Random.Range(minDamage, maxDamage));
            }
            Destroy(gameObject);
        }
       
        if(collision.tag == "Boss")
        {
            BossScript Boss = collision.GetComponent<BossScript>();
            if (Boss != null)
            {
                Boss.TakeDamage(Random.Range(minDamage, maxDamage));
            }
            Destroy(gameObject);
        }
    }
}
