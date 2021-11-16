using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [SerializeField]
    private GameObject escudo;
    public LifeManaManager manager;
    public Transform firePoint;
    public GameObject projectilePrefab;

    //Variáveis de movimento e mecânicas---------------
    [Header("Variaveis")]
    #region
    public float speed = 30;
    public float jumpForce = 6;
    private float moveInput;
    private bool xFacingRight = true;
    private int direcion = 1;
    #endregion



    private float jumpTimeCounter;
    public float jumpTime;
    public bool isJumping;
    private bool canMove = true;

    public Animator anim;
   
    //Física----------------------------
    private Rigidbody2D rb;
    public GameObject groundCheck;
    public bool isGrounded = false;

    public float mana;

    //Ataque
    [Header("Attack")]
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public float attackRange;
    public LayerMask isEnemy;
    public int damage;

    //Checagem na parede:
    [Header("OnWall")]
    public float horizontalJumpForce = 6f;
    public float wallJumpDuration = 0.25f;
    public bool onWall;
    public Vector3 wallOffset;
    public float wallRadius;
    public float maxFallSpeed = -1f;
    public bool jumpFromWall;
    public float jumpFinish;
    public LayerMask wallLayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }


    void FixedUpdate()
    {
        GroundMovement();        
    }


    private void Update()
    {
        AttackButton();
        CuraLife();
        Mana();
        Escudo();
        PhysicsCheck();
        checkCanMove();
        AirMovement();
        Shoot();

        if(Input.GetAxis("Horizontal") != 0)
        {
            anim.SetBool("andando", true);
        }
        else
        {
            anim.SetBool("andando", false);
        }

        if (Input.GetAxis("Horizontal") > 0 && !xFacingRight)
            Flip();
        else if (Input.GetAxis("Horizontal") < 0 && xFacingRight)
            Flip();

    }
    void Mana()
    {
        mana = manager.GetComponent<LifeManaManager>().actualMana;
    }
    public void AttackButton()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Attack();
                timeBtwAttack = startTimeBtwAttack;
                //Debug.Log(GetComponent<EnemyScript>().health);
            }

        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
    public void Attack()
    {
        anim.SetTrigger("attack");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, isEnemy);

        foreach(Collider2D enemy in enemiesToDamage)
        {
            enemy.GetComponent<EnemyScript>().TakeDamage(damage);
        }
    }
    void CuraLife()
    { 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            float lifeActual = manager.GetComponent<LifeManaManager>().actualHealth;
            float x = lifeActual - 100f;
            if (mana > 25f && lifeActual < 100f) {
                manager.GetComponent<LifeManaManager>().ReduceMana(25f);
                manager.GetComponent<LifeManaManager>().Damage(x); 
            }
        }
    }
    void Escudo()
    {
        if (Input.GetKey(KeyCode.E))
        {
            escudo.SetActive(true);
            manager.GetComponent<LifeManaManager>().ReduceMana(0.07f);
            if (mana <= 0f)
                escudo.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.E) && mana < 5f)
        {
            Debug.Log("NOT ENOUGH MANA!");
        }
        else
            escudo.SetActive(false); 
    }

    //Metodo para personagem andar:
    void GroundMovement()
    {
        if (!canMove)
            return;

        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

    }

    // Metodo pulo:
    void AirMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            //isJumping = true;
            //jumpTimeCounter = jumpTime;
            //rb.velocity = Vector2.up * jumpForce;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            anim.SetBool("pulando", true);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            anim.SetBool("pulando", true);
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
                anim.SetBool("pulando", true);
            }
            else
                isJumping = false;
        }

        if(isGrounded == true && rb.velocity.y == 0)
        {
            anim.SetBool("pulando", false);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && onWall && isGrounded == false)
        {
            canMove = false;
            jumpFinish = Time.time + wallJumpDuration;
            isJumping = false;
            jumpFromWall = true;
            Flip();
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(horizontalJumpForce * direcion, jumpForce), ForceMode2D.Impulse);
        }
    }

    //Metodo checagem de movimento:
    void checkCanMove()
    {
        if (jumpFromWall)
        {
            if(Time.time > jumpFinish)
            {
                jumpFromWall = false;
            }
        }

        if(!jumpFromWall && !canMove)
        {
            if(Input.GetAxis("Horizontal") != 0 || isGrounded == true)
            {
                canMove = true;
            }
        }
    }

    //Metodo checagem na parede:
    void PhysicsCheck()
    {
        onWall = false;
        anim.SetBool("wall", false);

        bool rightWall = Physics2D.OverlapCircle(transform.position + new Vector3(wallOffset.x, 0), wallRadius, wallLayer);
        bool LeftWall = Physics2D.OverlapCircle(transform.position + new Vector3(-wallOffset.x, 0), wallRadius, wallLayer);

        if(rightWall || LeftWall)
        {
            
            onWall = true;
        }

        if (onWall)
        {
            anim.SetBool("wall", true);
            if (rb.velocity.y < maxFallSpeed)
            {
              
                rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(wallOffset.x, 0f), wallRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(-wallOffset.x, 0f), wallRadius);


    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    //Metodo para flipagem do personagem:
    private void Flip()
    {
        direcion *= -1;
        xFacingRight = !xFacingRight;

        transform.Rotate(0f, 180f, 0);
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.C) && mana >= 15f)
        {
            manager.ReduceMana(15f);
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void Impulse( float x, float y)
    {
        rb.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            manager.Damage(25);
        }
    }

}
