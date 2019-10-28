using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.GetComponent<CharacterController2D>().isGrounded = true;
        Player.GetComponent<CharacterController2D>().canFlip = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player.GetComponent<CharacterController2D>().isGrounded = true;
        Player.GetComponent<CharacterController2D>().canFlip = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player.GetComponent<CharacterController2D>().isGrounded = false;
    }
}
