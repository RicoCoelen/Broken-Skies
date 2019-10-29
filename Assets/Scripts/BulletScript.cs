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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Boss":
                BossScript Boss = collision.gameObject.GetComponent<BossScript>();
                if (Boss != null)
                {
                    Boss.TakeDamage(Random.Range(minDamage, maxDamage));
                }
                Destroy(gameObject);
                break;
            case "Enemies":
                EnemyScript Enemy = collision.gameObject.GetComponent<EnemyScript>();
                if (Enemy != null)
                {
                    Enemy.TakeDamage(Random.Range(minDamage, maxDamage));
                }
                Destroy(gameObject);
                break;

            case "Kill":

                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
