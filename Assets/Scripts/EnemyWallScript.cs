using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallScript : MonoBehaviour
{
    public float distance;
    public bool isWalled = false;
    public GameObject changeVar;
    private float direction;
    private Vector2 side;
    public LayerMask level;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (changeVar.GetComponent<EnemyScript>().facingRight == true)
        {
            direction = transform.position.x + distance;
            side = Vector2.right;
        }
        else
        {
            direction = transform.position.x - distance;
            side = Vector2.left;
        }

        // bottom check
        Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y);

        // raycast to wall
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, side, distance, level);

        if (hit.collider == true)
        {
            changeVar.GetComponent<EnemyScript>().isWalled = true;
            isWalled = true;
        }
        else
        {
            changeVar.GetComponent<EnemyScript>().isWalled = false;
            isWalled = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // ray cast
        Gizmos.DrawLine(transform.position, new Vector3(direction, transform.position.y, transform.position.z));
    }
}
