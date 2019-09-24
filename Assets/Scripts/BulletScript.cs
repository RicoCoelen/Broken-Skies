﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float bulletDamage = 50f;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        EnemyScript Enemy = collision.GetComponent<EnemyScript>();
        if (Enemy != null)
        {
            Enemy.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
