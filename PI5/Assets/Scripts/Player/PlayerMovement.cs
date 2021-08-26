using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [SerializeField]
    private GameObject escudo;
    public LifeManaManager manager;

    public float speed = 30;
    public float jumpForce = 6;
    private float moveInput;
    private bool xFacingRight = true;
    private int direcion = 1;
    //public float fallMultiplier = 2.5f;
    //public float lowJumpMultiplier = 2f;
    private float jumpTimeCounter;
    public float jumpTime;
    public bool isJumping;
    private bool canMove = true;

    public Animator anim;
   
    
    private Rigidbody2D rb;

    public GameObject groundCheck;
    public bool isGrounded = false;

    public float mana;

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
        CuraLife();
        Mana();
        Escudo();
        AirMovement();
        PhysicsCheck();
        checkCanMove();

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
            rb.velocity = Vector2.up * jumpForce;

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

        bool rightWall = Physics2D.OverlapCircle(transform.position + new Vector3(wallOffset.x, 0), wallRadius, wallLayer);
        bool LeftWall = Physics2D.OverlapCircle(transform.position + new Vector3(-wallOffset.x, 0), wallRadius, wallLayer);

        if(rightWall || LeftWall)
        {
            onWall = true;
        }

        if (onWall)
        {
            if(rb.velocity.y < maxFallSpeed)
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

    //Metodo para flipagem do personagem:
    private void Flip()
    {
        direcion *= -1;
        xFacingRight = !xFacingRight;

        transform.Rotate(0f, 180f, 0);
    }

}
