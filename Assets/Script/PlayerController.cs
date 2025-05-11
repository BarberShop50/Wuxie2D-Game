using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public float restoreTime;
    public float climbSpeed;

    private Rigidbody2D myRigibody;// Start is called before the first frame update
    private Animator myAnim;
    private BoxCollider2D myFeet;
    private bool isGround;
    private bool canDoubleJump;
    private bool isOneWayPlatform;

    private bool isLadder;
    private bool isClimbing;
    private bool isJumping;
    private bool isFallling;
    private bool isDoubleJumping;
    private bool isDoubleFalling;
    private float playerGravity;
    void Start()
    {
        myRigibody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        playerGravity = myRigibody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController .isGameAlive ) 
        {
            CheckAirStatus();
            Run();
            Flip();
            Jump();
            CheckGrounded();
            SwichAnimationg();
            OneWayPlatformCheck();
            CheckLadder();
            Climb();
           



        }




    }
    void CheckGrounded() 
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))||
            myFeet .IsTouchingLayers(LayerMask .GetMask ("MovingPlatform"))||
            myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        isOneWayPlatform = myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));   
        Debug.Log(isGround);
    }
    void CheckLadder() 
    {
        isLadder = myFeet.IsTouchingLayers(LayerMask .GetMask("Ladder"));

    }
    void Flip() 
    {
        bool playerHasXAxisSpeed = Mathf.Abs(myRigibody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if (myRigibody.velocity.x > 0.1f) 
            {
                transform.localRotation = Quaternion.Euler(0,0,0);
            }
            if (myRigibody.velocity.x < -0.1f) 
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    void Run() 
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigibody.velocity.y);
        myRigibody.velocity = playerVel;
        bool playerHasXAxisSpeed = Mathf.Abs(myRigibody.velocity.x) > Mathf.Epsilon;
        myAnim.SetBool("Run", playerHasXAxisSpeed);
    }
    void Jump() 
    {
        if (Input.GetButtonDown("Jump")) 
        {
            if (isGround)
            {
                myAnim.SetBool("Jump", true);
                Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
                myRigibody.velocity = Vector2.up * jumpVel;
                canDoubleJump = true;
            }
            else 
            {
                if (canDoubleJump)
                {
                    myAnim.SetBool("DoubleJump", true);
                    Vector2 doubleJumpVel = new Vector2(0.0f, doubleJumpSpeed);
                    myRigibody.velocity = Vector2.up * doubleJumpVel;
                    canDoubleJump = false;
                }

            }
           
        }

    }
    void Climb() 
    {
        if (isLadder)
        {
            float moveY = Input.GetAxis("Vertical");
            if (moveY > 0.5f || moveY < -0.5f)
            {
                myAnim.SetBool("Climbing", true);
                myRigibody.gravityScale = 0.0f;

                myRigibody.velocity = new Vector2(myRigibody.velocity.x, moveY * climbSpeed);
            }
            else
            {
                if (isDoubleFalling || isJumping || isFallling || isDoubleJumping)
                {
                    myAnim.SetBool("Climbing", false);

                }
                else
                {
                    myAnim.SetBool("Climbing", false);
                    myRigibody.velocity = new Vector2(myRigibody.velocity.x, 0.0f);

                }


            }


        }
        else 
        {
            myAnim.SetBool("Climbing", false);
            myRigibody.gravityScale = playerGravity;
        }

    
    
    }
  
    void SwichAnimationg() 
    {
        myAnim.SetBool("Idle", false);
        if (myAnim.GetBool("Jump"))
        {
            if (myRigibody.velocity.y < 0.0f)
            {
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("Fall", true);
            }
        }
        else if (isGround)
        {
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("Idle", true);
        }
        if (myAnim.GetBool("DoubleJump"))
        {
            if (myRigibody.velocity.y < 0.0f)
            {
                myAnim.SetBool("DoubleJump", false);
                myAnim.SetBool("DoubleFall", true);
            }
        }
        else if (isGround)
        {
            myAnim.SetBool("DoubleFall", false);
            myAnim.SetBool("Idle", true);
        }
      

    }
    void OneWayPlatformCheck()
    {
        if (isGround && gameObject.layer != LayerMask.NameToLayer("Player")) 
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
        float moveY = Input.GetAxis("Vertical");
        if (isOneWayPlatform && moveY < -0.1f)
        {
            gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
            Invoke("RestorePlayerLayer", restoreTime);
        }


    }
    void RestorePlayerLayer() 
    {
        if (!isGround && gameObject.layer != LayerMask.NameToLayer("Player")) 
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
    void CheckAirStatus() 
    {
        isJumping = myAnim.GetBool("Jump");
        isFallling = myAnim.GetBool("Fall");
        isDoubleJumping = myAnim.GetBool("DoubleJump");
        isDoubleFalling = myAnim.GetBool("DoubleFall");
        isClimbing = myAnim.GetBool("Climbing");


    }
}
