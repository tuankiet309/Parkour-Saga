using System;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isDead;
    [HideInInspector]public bool playerUnlock;
    [HideInInspector] public bool extraLife;
    // ======================================================================
    [Header("KnockBack Info")]
    [SerializeField] private Vector3 knockBackDir;
    private bool isKnock;
    private bool canBeKnock =true;
    [Header("Move Info")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float moveSpeed;
    [Space]
    [SerializeField] private float milestoneIncreaser;
    private float speedMilestone;
    private float defaultMovespeed;
    private float defaultMilestoneIncreaser;
    [Header("Jump Info")]
    [SerializeField] private float defaultJumpSpeed;
    [SerializeField] private float doubleJumpSpeed;
    private float jumpSpeed;
    private bool canDoubleJump;
    [Header("Slide Info")]
    [SerializeField] private float slideTimer = 0f;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideCooldown = 0f;
    private float defaultSlideSpeed;
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
        sr= GetComponent<SpriteRenderer>();
        jumpSpeed = defaultJumpSpeed;
        speedMilestone = milestoneIncreaser;
        defaultMilestoneIncreaser = milestoneIncreaser;
        defaultMovespeed = moveSpeed;
        defaultSlideSpeed = slideSpeed;
    }



    // ======================================================================UPDATE===============================================================
    void Update()
    {
        if (isDead)
            return;
        AnimatorController();
        CheckCollision();
        extraLife = moveSpeed >= maxSpeed;
        if (isKnock)
            return;
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (playerUnlock && !isFacingWall && !isDead)
            SetUpMovement();

        CheckInput();
        CheckForSlide();
        CheckForLedge();
        SpeedController();
        
    }
    public void Damage()
    {
        if (extraLife)
        {
            Knockback();
            SpeedReset();
        }
        else StartCoroutine(Die());
    }
    private IEnumerator Die()
    {
        if (isDead != true)
        {
            AudioManager.Instance.PlaySFX(4);
            isDead = true;
            canBeKnock = false;
            rb.velocity = knockBackDir;
            anim.SetBool("isDead", true);
            Time.timeScale = 0.6f;
            yield return new WaitForSeconds(.5f);
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(1f);
            GameManager.Instance.GameEnded();
        }
    }

    private IEnumerator Invincibility()
    {
        Color origin = sr.color;
        Color darker = new Color(sr.color.r, sr.color.g, sr.color.b,.5f);
        canBeKnock = false;
        for(int i=1;i<6;i++)
        {
            sr.color = darker;
            yield return new WaitForSeconds(i/8f);
            sr.color = origin;
            yield return new WaitForSeconds(i/8f);
        }
        
        canBeKnock = true;
    }
    private void Knockback()
    {
        if (!canBeKnock)
            return;
        isKnock = true;
        rb.velocity = knockBackDir;
        StartCoroutine(Invincibility());
    }
    private void CancelKnock() => isKnock = false;
    private void SpeedReset()
    {
        moveSpeed = defaultMovespeed;
        milestoneIncreaser = defaultMilestoneIncreaser;
        slideSpeed = defaultSlideSpeed;
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
        
        /*if (Input.GetButtonDown("Fire2"))
            playerUnlock = true;*/
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

    private void SetUpMovement()
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
        if (rb.velocity!= Vector2.zero && slideCooldownTimer < 0 && isGrounded )
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
            AudioManager.Instance.PlaySFX(1);
        }
        else if (canDoubleJump == true)
        {
            jumpSpeed = doubleJumpSpeed;
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpSpeed = defaultJumpSpeed;
            AudioManager.Instance.PlaySFX(2);

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
