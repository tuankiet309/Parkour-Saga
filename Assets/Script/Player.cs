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
    // ======================================================================
    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance = 0f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private bool isFacingWall;
    private bool isGrounded;

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
        if(isGrounded)
        {
            canDoubleJump = true;
        }
        AnimatorController();
        CheckCollision();
        CheckInput();
        if (playerUnlock && !isFacingWall)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }




    // ======================================================================OTHER FUNCTION===============================================================
    private void AnimatorController()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity",rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("canDoubleJump", canDoubleJump);
    }
    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isFacingWall = Physics2D.BoxCast(wallCheck.position, wallCheckSize,0,Vector2.zero,whatIsGround);
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
            playerUnlock = true;
        if (Input.GetButtonDown("Jump") )
        {
            JumpFunction();
        }
    }

    private void JumpFunction()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position,wallCheckSize);
    }
}
