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
    public float lifestealAmount;

    public LayerMask Enemies;

    public float DamageStab;

    public Animator anim;

    public GameObject projectile;
    public Transform firePoint;

    public GameObject muzzleflash;

    public CinemachineVirtualCamera vc;
    public CinemachineVirtualCamera vct;

    // Update is called once per frame
    void FixedUpdate()
    {
        // melee attack
        if (timeToAttack <= 0)
        {
            if(Input.GetKey(KeyCode.Mouse1))
            {
                anim.SetBool("IsAttacking", true);
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Enemies);
                Debug.Log(enemiesToDamage);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (vc.gameObject.activeSelf == true)
                    {
                        vc.GetComponent<CinemachineCameraShaker>().ShakeCamera(0.1f);
                    }
                    else
                    {
                        vct.GetComponent<CinemachineCameraShaker>().ShakeCamera(0.1f);
                    }
                    enemiesToDamage[i].GetComponent<EnemyScript>().TakeDamage(DamageStab);
                    GetComponentInParent<HealthScript>().GiveHealth(lifestealAmount);
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
            if (Input.GetKey(KeyCode.Mouse0))
            {
                anim.SetBool("IsShooting", true);

                if (vc.gameObject.activeSelf == true)
                {
                    vc.GetComponent<CinemachineCameraShaker>().ShakeCamera(0.1f);
                }
                else 
                {
                    vct.GetComponent<CinemachineCameraShaker>().ShakeCamera(0.1f);
                }
                
                Instantiate(projectile, firePoint.position, firePoint.rotation);
                GameObject temp = Instantiate(muzzleflash, firePoint.position, firePoint.rotation);
                temp.transform.Rotate(0, 90, 0);
                temp.transform.parent = transform.parent;
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
