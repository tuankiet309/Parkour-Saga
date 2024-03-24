using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    // ======================================================================
    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float defaultJumpSpeed;
    [SerializeField] private float doubleJumpSpeed;
    private float jumpSpeed;
    private Rigidbody2D rb;
    private bool playerUnlock;
    private bool canDoubleJump;
    [Header("Slide Info")]
    [SerializeField] private float slideTimer = 0f;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideCooldown = 0f;
    private float slideCooldownTimer;
    private bool isSlide;
    private float slideTimerCounter = 0f;
    // ======================================================================
    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance = 0f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private float ceilingCheckDistance;
    private bool isFacingWall;
    private bool isGrounded;
    private bool isUnderCeiling;


    // ======================================================================START===============================================================
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpSpeed = defaultJumpSpeed;
    }



    // ======================================================================UPDATE===============================================================
    void Update()
    {
        
        AnimatorController();
        CheckCollision();
        CheckInput();
        CheckForSlide();
        
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        
        if (playerUnlock && !isFacingWall)
            MoveFunction();
    }
    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
            playerUnlock = true;
        if (Input.GetButtonDown("Jump"))
        {
            JumpFunction();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SlideTimerFunction();
        }
    }
    private void CheckForSlide()
    {
        slideTimerCounter -= Time.deltaTime;
        slideCooldownTimer -= Time.deltaTime;
        if (slideTimerCounter < 0f && !isUnderCeiling)
        {
            isSlide = false;
        }
    }

    private void MoveFunction()
    {
        if (isFacingWall)
            return;
        if (isSlide )
        {
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
            
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    

    private void SlideTimerFunction()
    {
        if (rb.velocity!= Vector2.zero && slideCooldownTimer < 0 && isGrounded)
        {
            isSlide = true;
            slideTimerCounter = slideTimer;
            slideCooldownTimer = slideCooldown;
        }
    }

    private void JumpFunction()
    {
        if(isSlide && isUnderCeiling)
        {
            return;
        }
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
        else if (canDoubleJump == true)
        {
            jumpSpeed = doubleJumpSpeed;
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpSpeed = defaultJumpSpeed;
        }
    }


    // ======================================================================OTHER FUNCTION===============================================================
    private void AnimatorController()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSlide", isSlide);
    }
    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isFacingWall = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, whatIsGround);
        isUnderCeiling = Physics2D.Raycast(transform.position,Vector2.up, ceilingCheckDistance, whatIsGround);
    }


    // ======================================================================GIZMOS===============================================================

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawLine(transform.position,new Vector2(transform.position.x,transform.position.y + ceilingCheckDistance));
    }
}
