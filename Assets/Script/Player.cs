using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private bool isRun;
    // ======================================================================
    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    private Rigidbody2D rb;
    private bool runBegun;
    // ======================================================================
    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance = 0f;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;



    // ======================================================================START===============================================================
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }



    // ======================================================================UPDATE===============================================================
    void Update()
    {
        AnimatorController();
        CheckCollision();
        CheckInput();
        if (runBegun)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }




    // ======================================================================OTHER FUNCTION===============================================================
    private void AnimatorController()
    {
        isRun = rb.velocity.x != 0;
        anim.SetBool("isRun", isRun);
    }
    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
            runBegun = true;
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
