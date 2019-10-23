using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundScript : MonoBehaviour
{
    public float distance;
    public bool isGrounded = true;
    public GameObject changeVar;
    private float direction;
    private Vector2 side;
    public LayerMask level;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (changeVar.GetComponent<EnemyScript>().flipped == false)
        {
            direction = transform.position.y - distance;
            side = Vector2.down;
        }
        else
        {
            direction = transform.position.y + distance;
            side = Vector2.up;
        }

        // bottom check
        Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y);

        // raycast to ground
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, side, distance, level);

        if (hit.collider == true)
        {
            changeVar.GetComponent<EnemyScript>().isGrounded = true;
            isGrounded = true;
        }
        else
        {
            changeVar.GetComponent<EnemyScript>().isGrounded = false;
            isGrounded = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // ray cast
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, direction, transform.position.z));
    }
}
