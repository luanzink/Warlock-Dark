using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerDamage : MonoBehaviour
{
    public GameObject generalManager;
    public float valueX = 3f;
    public float valueY = 6f;
    public float damage = 5f;
   

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.layer == 9)
        {
            Debug.Log("Indentificou Damage");
            col.gameObject.GetComponent<PlayerMovement>().Impulse(valueX, valueY);
            generalManager.GetComponent<LifeManaManager>().Damage(damage);

            
        }
    }
}
