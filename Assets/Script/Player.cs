using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    // ======================================================================
    [Header("KnockBack Info")]
    [SerializeField] private Vector3 knockBackDir;
    private bool isKnock;
    [Header("Speed Info")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [Space]
    [SerializeField] private float milestoneIncreaser;
    private float speedMilestone;
    private float defaultMovespeed;
    private float defaultMilestoneIncreaser;
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
    [HideInInspector] public bool ledgeDetected;
    [Header("LedgeClimb Info")]
    [SerializeField] private Vector2 offSet1;
    [SerializeField] private Vector2 offSet2;
    private Vector2 climbBeginPosition;
    private Vector2 climbEndPosition;

    private bool canGrabLedge = true;
    private bool canClimb;


    // ======================================================================START===============================================================
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpSpeed = defaultJumpSpeed;
        speedMilestone = milestoneIncreaser;
        defaultMilestoneIncreaser = milestoneIncreaser;
        defaultMovespeed = moveSpeed;
    }



    // ======================================================================UPDATE===============================================================
    void Update()
    {
        Debug.Log(isFacingWall);
        
        AnimatorController();
        CheckCollision();

        if (isKnock)
            return;
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (playerUnlock && !isFacingWall)
            MoveFunction();
        CheckInput();
        CheckForSlide();
        CheckForLedge();
        SpeedController();
        
    }
    private void Knockback()
    {
        isKnock = true;
        rb.velocity = knockBackDir;
    }
    private void CancelKnock() => isKnock = false;
    private void SpeedReset()
    {
        moveSpeed = defaultMovespeed;
        milestoneIncreaser = defaultMilestoneIncreaser;
    }
    private void SpeedController()
    {
        if (moveSpeed == maxSpeed)
            return;
        if(transform.position.x > speedMilestone)
        {
            speedMilestone = speedMilestone + milestoneIncreaser;
            moveSpeed *= speedMultiplier;
            slideSpeed *= speedMultiplier;
            milestoneIncreaser*=speedMultiplier;
            if(moveSpeed> maxSpeed)
                moveSpeed = maxSpeed;
        }
    }
    private void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Knockback();
        }
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
    private void CheckForLedge()
    {
        if(ledgeDetected && canGrabLedge)
        {
            rb.gravityScale = 0;
            canGrabLedge = false;
            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            climbBeginPosition = ledgePosition + offSet1;
            climbEndPosition = ledgePosition + offSet2;
            canClimb = true;
        }
        if(canClimb)
            transform.position = climbBeginPosition;
    }
    private void LegdeClimbOver()
    {
        canClimb = false;
        transform.position = climbEndPosition;
        rb.gravityScale = 5;
        Invoke("AllowLedgeGrab", 0.1f);
    }
    private void AllowLedgeGrab() => canGrabLedge = true;
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
        {
            
            SpeedReset();
            return;
        }
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
        if(rb.velocity.y<-20)
        {
            anim.SetBool("canRoll", true);
        }
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSlide", isSlide);
        anim.SetBool("canClimb", canClimb);
        anim.SetBool("isKnock", isKnock);
    }
    private void RollFinish() => anim.SetBool("canRoll", false);
    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isFacingWall = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero,0, whatIsGround);
        isUnderCeiling = Physics2D.Raycast(transform.position,Vector2.up, ceilingCheckDistance, whatIsGround);
        Debug.Log(ledgeDetected);
    }


    // ======================================================================GIZMOS===============================================================

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawLine(transform.position,new Vector2(transform.position.x,transform.position.y + ceilingCheckDistance));
    }
}
