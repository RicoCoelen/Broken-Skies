using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    private float timeToAttack;
    public float cooldownAttack;

    public Transform attackPos;
    public float attackRange;

    public LayerMask Enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToAttack <= 0)
        {
            if(Input.GetKey(KeyCode.E))
            {
                timeToAttack = cooldownAttack;
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Enemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {

                }
            }
        }
        else
        {
            timeToAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
