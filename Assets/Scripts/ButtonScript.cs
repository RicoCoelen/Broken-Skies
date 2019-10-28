using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool isShot = false;
    public bool runonce = false;
    Color originalColor;
    public GameObject player;
    public MeshRenderer renderer;
    public GameObject hideGO;
    public GameObject hideGO2;
    public float range = 0;

    private void Update()
    {
        if (isShot == true)
        {
            renderer.material.color = Color.green;
            if (range == 0)
            {
                if (runonce == false)
                {
                    hideGO.SetActive(false);
                    if (hideGO2 != null)
                    {

                        hideGO2.SetActive(true);
                    }
                    runonce = true;
                }
            }

            if (range > 0 && Vector2.Distance(player.transform.position, transform.position) < range)
            {
                if (runonce == false)
                {
                    hideGO.SetActive(false);
                    if (hideGO2 != null)
                    {

                        hideGO2.SetActive(true);
                    }
                    runonce = true;
                }
            }
        }
        else
        {
            renderer.material.color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("EnemyBullet"))
        {
            if (collision.tag == "Bullet")
            {
                isShot = true;
                Destroy(collision.gameObject);
                Debug.Log("A door has opened!");
            }
        }
    }
}
