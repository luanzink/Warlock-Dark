using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask isEnemy;
    public int damage;

    void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                

                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, isEnemy);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyScript>().TakeDamage(damage);
                }
                timeBtwAttack = startTimeBtwAttack;
                //Debug.Log(GetComponent<EnemyScript>().health);
            }
            
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
