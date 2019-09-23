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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player.GetComponent<CharacterController2D>().isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Player.GetComponent<CharacterController2D>().isGrounded = false;
    }
}
