using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float Range;

    [SerializeField]
    private Transform Target;

    [SerializeField]
    private bool Detected = false;
    

    //"Atirador"
    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private Transform shootPoit;
    [SerializeField]
    private float force;
    [SerializeField]
    private LayerMask playerMask;

    float nextTimeToFire = 0f;

    Vector2 Direction;

    [SerializeField]
    private GameObject Gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos = Target.position;
        Direction = targetPos - (Vector2)transform.position;
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction,Range);

        if(rayInfo){
            if (rayInfo.collider.gameObject.CompareTag("Player") || rayInfo.collider.gameObject.CompareTag("Shield"))
            {
                if (Detected == false)
                {
                    Detected = true;
                }
            }
            else
                Detected = false;
        }
        if (Detected == true)
        {
            Gun.transform.right = Direction;
            if(Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject bulletIns = Instantiate(Bullet, shootPoit.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }


}
