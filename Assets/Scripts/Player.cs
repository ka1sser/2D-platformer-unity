using System;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private Animator anim;

    // Movement Configuration
    [Header("Movement Configuration")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float doubleJumpForce = 8f;
    
    private bool canDoubleJump;
    private bool isGrounded;
    private bool isAirborne;

    [Header("Buffer & Coyote Jump")]
    [SerializeField] private float bufferJumpWindow = 0.25f;
    private float bufferJumpActivated = -1;
    [SerializeField] private float coyoteJumpWindow = 0.5f;
    private float coyoteJumpActivated = -1;

    // Wall Interactions
    [Header("Wall Interactions")]
    [SerializeField] private float wallJumpDuration = 0.6f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(5f, 10f);
    private bool isWallJumping;
    private bool isWallDetected;

    // Knockback
    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 1f;
    [SerializeField] private Vector2 knockbackPower = new Vector2(5f, 5f);
    private bool isKnocked;
    private bool canBeKnocked = true;

    // Collision Info
    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("VFX")]
    [SerializeField] private GameObject deathVfx;

    // Player State
    private float xInput;
    private float yInput;
    private bool facingRight = true;
    private int facingDir = 1;

    // ---------- Unity Lifecycle Methods ----------
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isKnocked) return;

        HandleInput();
        HandleFlip();
        HandleMovement();
        HandleWallSlide();
        UpdateAirborneStatus();
        HandleCollision();
        HandleAnimations();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
    }

    // ---------- Input Handling ----------
    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
            RequestBufferJump();
        }
    }

    private void JumpButton()
    {
        bool coyoteJumpAvailable = Time.time < coyoteJumpActivated + coyoteJumpWindow;

        if (isGrounded || coyoteJumpAvailable)
        {
            Jump();
        }
        else if (isWallDetected && !isGrounded)
        {
            WallJump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }

        CancelCoyoteJump();
    }

    // ---------- Movement ----------
    private void HandleMovement()
    {
        if (isWallDetected || isWallJumping) return;

        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleFlip()
    {
        if ((xInput < 0 && facingRight) || (xInput > 0 && !facingRight))
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    // ---------- Jumping ----------
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void DoubleJump()
    {
        isWallJumping = false;
        canDoubleJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
    }

    private void WallJump()
    {
        canDoubleJump = true;
        rb.linearVelocity = new Vector2(wallJumpForce.x * -facingDir, wallJumpForce.y);
        
        Flip();
        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
    }

    private IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;
        canDoubleJump = true;

        yield return new WaitForSeconds(wallJumpDuration);

        isWallJumping = false;
    }

    private void AttemptBufferJump()
    {
        // When the user spam jump button and the player character is about to land to the ground but not yet
        if (Time.time < bufferJumpActivated + bufferJumpWindow)
        {
            bufferJumpActivated = Time.time - 1;
            Jump();
        }
    }

    // ---------- Airborne Status ----------
    private void UpdateAirborneStatus()
    {
        if (isGrounded && isAirborne) HandleLanding();
        else if (!isGrounded && !isAirborne) BecomeAirborne();
    }

    private void BecomeAirborne()
    {
        isAirborne = true;

        if (rb.linearVelocityY < 0)
        {
            ActivateCoyoteJump();
        }
    }

    private void HandleLanding()
    {
        isAirborne = false;
        canDoubleJump = true;

        AttemptBufferJump();
    }

    private void RequestBufferJump()
    {
        if (isAirborne)
        {
            bufferJumpActivated = Time.time;
        }
    }

    #region Coyote Jump
    private void ActivateCoyoteJump()
    // Jumping at the edge of a platform like in Super Mario reaching for the flag
    {
        coyoteJumpActivated = Time.time;
    }

    private void CancelCoyoteJump() => coyoteJumpActivated = Time.time - 1;
    #endregion
    
    // ---------- Wall Slide ----------
    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.linearVelocity.y < 0;
        if (!canWallSlide) return;

        float slideModifier = yInput < 0 ? 1 : 0.75f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * slideModifier);
    }

    // ---------- Knockback ----------
    public void KnockBack()
    {
        if (!canBeKnocked) return;

        StartCoroutine(KnockbackRoutine());
        anim.SetTrigger("knockback");
        rb.linearVelocity = new Vector2(knockbackPower.x * -facingDir, knockbackPower.y);
    }

    public void Die()
    {
        GameObject newdeathVfx = Instantiate(deathVfx, transform.position, UnityEngine.Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator KnockbackRoutine()
    {
        canBeKnocked = false;
        isKnocked = true;

        yield return new WaitForSeconds(knockbackDuration);

        canBeKnocked = true;
        isKnocked = false;
    }

    // ---------- Collision ----------
    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    // ---------- Animations ----------
    private void HandleAnimations()
    {
        anim.SetFloat("linearVelocityX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("linearVelocityY", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallDetected", isWallDetected);
    }
}