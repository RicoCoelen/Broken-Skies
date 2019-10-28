using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorScript : MonoBehaviour
{
    private bool runonce = false;

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<ButtonScript>().isShot == true)
        {
            if (runonce == false)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false); ;
        }
    }
}
