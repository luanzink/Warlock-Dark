using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody rb;
    public float counter = 0f;
    private EnemyScript enemy;

    void Start()
    {
        rb.velocity = transform.right * speed;
        enemy = GetComponent<EnemyScript>();
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if(counter >= 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            enemy.TakeDamage(20);
            Destroy(gameObject);
        }
    }
}
