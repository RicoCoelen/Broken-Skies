using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{

    private float timeToAttack;
    public float cooldownAttack;

    public Transform attackPos;
    public float attackRange;

    public LayerMask Player;

    public float DamageStab;

    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // melee attack
        if (timeToAttack <= 0)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Player);
            
            if (enemiesToDamage.Length > 0)
            {
                anim.SetBool("EnemyAttack", true);
                
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i].gameObject.CompareTag("Player"))
                    {
                        enemiesToDamage[i].GetComponent<HealthScript>().TakeDamage(DamageStab);
                    }
                }
            }
            timeToAttack = cooldownAttack;
        }
        else
        {
            anim.SetBool("EnemyAttack", false);
            timeToAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
