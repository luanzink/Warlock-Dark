using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private int actualHealth;
    private int maxHealth = 100;
    private Rigidbody2D rb;
    [SerializeField]
    private float knockBackX;
    [SerializeField]
    private float knockBackY;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        actualHealth = maxHealth;
        
    }

    private void Update()
    {
        if(actualHealth <= 0)
        {
            Die();
        }
    }

    public void ModifyHealth(int amount)
    {
        actualHealth -= amount;
        float currentHealthPct = (float)actualHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    public void TakeDamage(int damage)
    {
        ModifyHealth(damage);
        rb.AddForce(new Vector2 (knockBackX, knockBackY), ForceMode2D.Impulse);
        //actualHealth -= damage;
        Debug.Log("DAMAGE TAKEN!");
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
