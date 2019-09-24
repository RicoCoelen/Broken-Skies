﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthScript : MonoBehaviour
{
    public GameObject manager;
    public Slider healthBar;
    public float health = 100f;

    // flash red
    public float flashTime;
    Color origionalColor;
    public GameObject renderer;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = health;
        healthBar.maxValue = health;
        origionalColor = renderer.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        FlashRed();
        if (health <= 0f)
        {
            manager.GetComponent<GameManager>().GameOver();
        }
        healthBar.value = Mathf.Clamp(health, 0, 100f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Kill")
        {
            TakeDamage(100);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.gameObject.tag == "Kill")
        {
            TakeDamage(100);
        }
    }

    void FlashRed()
    {
        renderer.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    void ResetColor()
    {
        renderer.GetComponent<SpriteRenderer>().color = origionalColor;
    }
}
