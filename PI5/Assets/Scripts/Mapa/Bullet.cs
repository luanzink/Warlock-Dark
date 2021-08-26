using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField]
    float damage = 5f;
    [SerializeField]
    GameObject generalManager;
    // Start is called before the first frame update
    void Start()
    {
        generalManager = GameObject.FindWithTag("Mag");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(this.gameObject);
        if(col.gameObject.tag == "Player")
        {
            generalManager.GetComponent<LifeManaManager>().Damage(damage);
            Debug.Log("Dano");
        }
    }*/

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(this.gameObject);
        if (col.gameObject.tag == "Player")
        {
            generalManager.GetComponent<LifeManaManager>().Damage(damage);
            Debug.Log("Dano");
        }
    }
}
