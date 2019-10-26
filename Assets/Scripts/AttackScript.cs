using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AttackScript : MonoBehaviour
{
    private float timeToAttack;
    public float cooldownAttack;

    private float timeToShoot;
    public float cooldownShoot;

    public Transform attackPos;
    public float attackRange;

    public LayerMask Enemies;

    public float DamageStab;

    public Animator anim;

    public GameObject projectile;
    public Transform firePoint;

    public CinemachineVirtualCamera vc;

    // Update is called once per frame
    void Update()
    {
        // melee attack
        if (timeToAttack <= 0)
        {
            if(Input.GetKey(KeyCode.Q))
            {
                anim.SetBool("IsAttacking", true);
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Enemies);
                Debug.Log(enemiesToDamage);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    vc.GetComponent<CinemachineCameraShaker>().ShakeCamera(0.1f);
                    enemiesToDamage[i].GetComponent<EnemyScript>().TakeDamage(DamageStab);
                }         
            }
            timeToAttack = cooldownAttack;
        }
        else
        {
            anim.SetBool("IsAttacking", false);
            timeToAttack -= Time.deltaTime;
        }

        // shooting
        if (timeToShoot <= 0)
        {
            if (Input.GetKey(KeyCode.E))
            {
                anim.SetBool("IsShooting", true);
                vc.GetComponent<CinemachineCameraShaker>().ShakeCamera(0.1f);
                Instantiate(projectile, firePoint.position, firePoint.rotation);
            }
            timeToShoot = cooldownShoot;
        }
        else
        {
            anim.SetBool("IsShooting", false);
            timeToShoot -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
