using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rig;
    CharacterController controller;
    Vector2 finalVelocity;
    Vector2 xVelocity;
    public float xSpeed;

    Vector2 yVelocity;
    public float maxHeight;
    float jumpSpeed;
    public float timeToPeak;

    float gravity;
    int jump;
    public int qtJump;

    //Checagem se está na parede
    public bool onWall;
    public Vector3 wallOffset;
    public float wallRadius;
    public LayerMask wallLayer;
    public float maxFallSpeed = -1;

    //Outros
    public GameObject tiro;
    private bool xFacingRight = true;



    // Start is called before the first frame update
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        gravity = 2 * maxHeight / Mathf.Pow(timeToPeak, 2);
        jumpSpeed = gravity * timeToPeak;
    }

    // Update is called once per frame
    void Update()
    {
        
        Mover();
        PhysicsCheck();
        if (Input.GetAxis("Horizontal") > 0 && !xFacingRight)
            Flip();
        else if (Input.GetAxis("Horizontal") < 0 && xFacingRight)
            Flip();
    }

    void Mover()
    {

        float xInput = Input.GetAxis("Horizontal");

        xVelocity = xSpeed * xInput * Vector2.right;
        yVelocity += gravity * Time.deltaTime * Vector2.down;
        if (controller.isGrounded)
        {
            yVelocity = Vector2.down;
            jump = 0;
        }

        if (Input.GetButtonDown("Jump") && jump < qtJump)
        {
            yVelocity = jumpSpeed * Vector2.up;
            jump += 1;
        }
        

        
        finalVelocity = xVelocity + yVelocity;

        controller.Move(finalVelocity * Time.deltaTime);

        

    }

    void PhysicsCheck()
    {
        onWall = false;

        //bool rightWall = Physics2D.OverlapCircle(transform.position + new Vector3(wallOffset.x, 0f), wallRadius, wallLayer);
        bool rightWall = Physics.CheckSphere(transform.position + new Vector3(wallOffset.x, 0f), wallRadius, wallLayer);
        bool lefttWall = Physics.CheckSphere(transform.position + new Vector3(-wallOffset.x, 0f), wallRadius, wallLayer);

        if (rightWall || lefttWall)
        {
            onWall = true;
        }

        if (onWall)
        {
            if(controller.velocity.y < maxFallSpeed)
            {
                yVelocity = new Vector2(controller.velocity.x, maxFallSpeed);
            }
            if (Input.GetButtonDown("Jump"))
            {
                if (rightWall)
                {
                    yVelocity = new Vector2(-10f, 15f);
                }

                if (lefttWall)
                {
                    yVelocity = new Vector2(10f, 15f);
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(wallOffset.x, 0f), wallRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(-wallOffset.x, 0f), wallRadius);
    }

    private void Flip()
    {
        xFacingRight = !xFacingRight;

        transform.Rotate(0f, 180f, 0);
    }

}

  
