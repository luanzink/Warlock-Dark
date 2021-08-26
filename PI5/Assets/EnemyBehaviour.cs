using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region Variáveis Públicas
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastLength;
    public float attackDistance;
    public float moveSpeed;
    public float timer; //Cooldown de ataque.
    #endregion

    #region Variáveis Privadas
    private RaycastHit2D hit;
    private GameObject target;
    //private Animator anim;
    private float distance; //Distancia entre player e enemy
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float intTimer;
    #endregion

    private void Awake()
    {
        intTimer = timer;
    }


    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, rayCastMask);
            Debug.Log(hit);
            RaycastDebugger();
        }   


        if(hit.collider != null)
        {
            EnemyLogic();
        }
        else if(hit.collider == null)
        {
            Debug.Log("a");
            inRange = false;
        }
        if (inRange == false)
        {
            StopAttack();
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance > attackDistance)
        {
            Move();
            //StopAttack();
        }
        else if (attackDistance >= distance /*&& cooling == false*/)
            Attack();
        if (cooling)
        {
            //anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        timer = intTimer;
        attackMode = true;
        //animation
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            target = other.gameObject;
            inRange = true;
            Move();
        }
    }

    void RaycastDebugger()
    {
        if(distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
        }
        else if (attackDistance < distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
        }
    }
}
