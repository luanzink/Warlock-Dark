using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody rb;
    public float counter = 0f;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if(counter >= 2f)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        EnemyScript enemy = other.GetComponent<EnemyScript>();
        if(enemy != null)
        {
            enemy.TakeDamage(20);
            Destroy(gameObject);
        }
        //Destroy(gameObject);
    }
}
