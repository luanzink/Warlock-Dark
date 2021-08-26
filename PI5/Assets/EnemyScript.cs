using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public HealthBar healthBar;


    private void Start()
    {
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);   
    }

    private void Update()
    {
        if(health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("DAMAGE TAKEN!");
        healthBar.SetHealth(health, maxHealth);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
