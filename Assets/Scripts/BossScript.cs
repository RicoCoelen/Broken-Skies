﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class BossScript : MonoBehaviour
{
    // health
    public Slider healthBar;
    public float startHealth = 10000f;
    public float health;

    // flash red
    public float flashTime;
    Color origionalColor;
    public SpriteRenderer renderer;

    // attack
    public GameObject bullet;
    public float accuracy;
    private float direction;
    public Transform shootPoint;
    public Animator anim;
    public float maxRange;
    public int chance;

    // huidige status
    public State cState;

    public enum State
    {
        WAITING = 0,
        TRIGGERED = 1,
        STAGE1 = 2,
        STAGE2 = 3
    }

    // cameras
    public CinemachineVirtualCamera vc;
    public CinemachineTargetGroup vct;

    // player
    public GameObject Player;

    // doors
    public bool runonce = false;
    public GameObject closeDoor;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.value = health;
        healthBar.maxValue = health;
        origionalColor = renderer.color;

        // always start idle
        cState = State.WAITING;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (cState)
        {
            case State.WAITING:
                // do nothing
                break;

            case State.TRIGGERED:
                BossTriggered();
                break;

            case State.STAGE1:
                Stage1();
                break;

            case State.STAGE2:
                Stage2();
                break;

            default:
                cState = State.WAITING;
                break;
        }
    }

    void BossTriggered()
    {
        vc.gameObject.SetActive(false);
        vct.gameObject.SetActive(true);
        closeDoor.gameObject.SetActive(true);
        healthBar.transform.parent.gameObject.SetActive(true);
        anim.SetBool("isIdle", true);
        anim.SetBool("EnemyShoot", false);

        cState = State.STAGE1;
    }

    void Stage1()
    {
        // bullet rotation
        direction += accuracy;

        if (direction > maxRange || direction < 0)
        {
            direction = Random.Range(0, maxRange);
        }

        Quaternion rot = Quaternion.Euler(shootPoint.rotation.x, shootPoint.rotation.y, direction + 90);

        // random chance
        float r = Random.Range(0, chance);

        if (r < 1)
        {
            Instantiate(bullet, shootPoint.position, rot);
        }

        // anim  and point shoot
        anim.SetBool("isIdle", false);
        anim.SetBool("EnemyShoot", true);
    }

    void Stage2()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("EnemyShoot", true);
    }

    public void TakeDamage(float amount)
    {
        // remove health
        health -= amount;

        healthBar.value = Mathf.Clamp(health, 0, startHealth);

        // flash red
        FlashRed();

        // if no health die
        if (health <= 0f)
        {
            GetComponent<GameManager>().PlayNext();
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
        if (collision.gameObject.tag == "Player")
        {
            if (runonce == false)
            {
                cState = State.TRIGGERED;
                runonce = true;
            }
        }
    }

}
