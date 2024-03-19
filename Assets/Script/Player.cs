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
    private bool isGrounded;
    [Header("WallCheckInfo")]
    [SerializeField] private float wallCheckDistance = 0f;
    [SerializeField]Transform wallCheckPosition;
    private bool isFacingWall;



    // ======================================================================START===============================================================
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpSpeed = defaultJumpSpeed;
        if (transform.GetChild(0) != null)
        {
            wallCheckPosition = transform.GetChild(0);
        }
    }



    // ======================================================================UPDATE===============================================================
    void Update()
    {
        AnimatorController();
        CheckCollision();
        CheckInput();
        if (playerUnlock)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        if(isFacingWall)
            playerUnlock = false;
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
        isFacingWall = Physics2D.Raycast(wallCheckPosition.position, Vector2.right, wallCheckDistance, whatIsGround);
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
            canDoubleJump = true;
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
        Gizmos.DrawLine(wallCheckPosition.position, new Vector2(wallCheckPosition.position.x + wallCheckDistance, wallCheckPosition.position.y ));
    }
    
}
