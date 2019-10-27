using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{

    public Slider healthBar;
    public float startHealth = 10000f;
    public float health;

    // flash red
    public float flashTime;
    Color origionalColor;
    public SpriteRenderer renderer;

    // huidige status
    public State cState;

    public enum State
    {
        WAITING = 0,
        TRIGGERED = 1,
        STAGE1 = 2,
        STAGE2 = 3
    }

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
    void Update()
    {
        
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

}
