using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{

    private float timeToAttack;
    public float cooldownAttack;

    public Transform attackPos;
    public float attackRange;

    //
    public LayerMask Player;
    public GameObject PlayerGO;

    // attack
    public float DamageStab;
    public Animator anim;

    // detection
    public float DetectionRange;
    public GameObject currentTarget;


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
            checkMelee();
        }
        else
        {
            anim.SetBool("EnemyAttack", false);
            timeToAttack -= Time.deltaTime;
        }

        // check for shoot and player
        checkForPlayer();
    }

    private void checkMelee()
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

    private void checkForPlayer()
    {
        // get distance
        float dis = Vector3.Distance(transform.parent.position, PlayerGO.transform.position);

        // check if in range
        if (dis < DetectionRange) 
        {
            currentTarget = PlayerGO;
            GetComponentInParent<EnemyScript>().currentTarget = PlayerGO;
        }
        else
        {
            // else make him forget
            currentTarget = null;
            GetComponentInParent<EnemyScript>().currentTarget = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // stab range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

        // detection range
        Gizmos.DrawWireSphere(transform.parent.position, DetectionRange);

        // shoot raycast
    }
}
