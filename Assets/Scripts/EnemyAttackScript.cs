using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    // detection
    public float DetectionRange;
    public GameObject currentTarget;
    public LayerMask Player;
    public GameObject PlayerGO;
    Vector3 direction;

    // attack
    public Transform attackPos;
    public float attackRange;
    public float DamageStab;
    public Animator anim;
    private float timeToAttack;
    public float cooldownAttack;

    // shoot
    public float shootRange;
    public GameObject projectile;
    private float timeToShoot;
    public float cooldownShoot;

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
        
        // check if player is left or right in sights
        if (GetComponentInParent<EnemyScript>().facingRight == true)
        {
             direction = Vector2.right;
            direction.z = 0;
        }
        else
        {
            direction = Vector2.left;
            direction.z = 0;
        }

        // raycast to ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shootRange, Player);
        
        if (hit.collider == true)
        {
            GetComponentInParent<EnemyScript>().currentTarget = PlayerGO;
            TryShoot();
        }
    }

    private void TryShoot()
    {
        float r = Random.Range(1,11);

        if(r < 2)
        {
            // melee attack
            if (timeToShoot <= 0)
            {
                anim.SetBool("EnemyShoot", true);
                Instantiate(projectile, transform.position, transform.rotation);
                timeToShoot = cooldownShoot;
            }
            else
            {
                anim.SetBool("EnemyShoot", false);
                timeToShoot -= Time.deltaTime;
            }
        }
        else
        {
            anim.SetBool("EnemyShoot", false);
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
        Gizmos.DrawLine(transform.position, transform.position +  new Vector3(direction.x * shootRange, 0, 0));
    }
}
