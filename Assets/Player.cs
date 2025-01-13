using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Movement Configuration")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    
    private float xInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
    }

    private void HandleCollision()
    {
        /*
        This function detects the ground via Raycast. To check the value for the ground distance, we
        used the OnDrawGizmos() to draw a line from the origin to the distance in y.
        */

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump() => rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);
    }

    private void HandleAnimations()
    {
        anim.SetFloat("linearVelocityX", rb.linearVelocityX);
        anim.SetFloat("linearVelocityY", rb.linearVelocityY);
        anim.SetBool("isGrounded", isGrounded);
    }
}
