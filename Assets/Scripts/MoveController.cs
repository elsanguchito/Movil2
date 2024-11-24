using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    [Header("Configuraciones del jugador")]
    [SerializeField] private LayerMask collisionLayers;
    private Rigidbody2D rb;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public LayerMask slideWallLayer;
    public Transform wallCheckPoint;
    public Transform groundCheckPoint;
    public Transform cornerCheckPoint;
    public Rigidbody2D platformRb;
    public ParticleController particleController;

    [Header("Ajustes de movimiento")]
    [SerializeField] private int speed = 10;
    [SerializeField, Range(1, 10)] private float acceleration = 5f;
    [SerializeField] private float cornerBounceForce = 5f;
    private float speedMultiplier;
    private Vector2 relativeTransform;
    private bool btnPressed;
    private float cornerBounceCooldown = 0.2f;
    private float lastBounceTime = -1f;

    [Header("Ajustes de salto")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpMultiplier = 1.5f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float fastFallMultiplier = 2f;
    private float coyoteTimeCounter;
    private bool isJumping = false;

    [Header("Detección de plataformas y paredes")]
    public bool isOnPlatform;
    private bool isGrounded;
    private bool isCorner;

    [Header("Ajustes de Wall Slide")]
    private bool isWallSliding = false;
    [SerializeField] private float wallSlideSpeed = 1.5f;
    [SerializeField] private float wallSlideDuration = 1f;
    private float wallSlideTimer = 0f;

    [Header("Ajustes de Wall Jump")]
    [SerializeField] private float wallJumpForceX = 7f;
    [SerializeField] private float wallJumpForceY = 7f;
    private bool isWallJumping = false;
    private int wallDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UpdateRelativeTransform();
    }

    private void FixedUpdate()
    {
        HandleCornerCheck();
        HandleGroundCheck();
        HandleWallDetection();
        HandleMovement();
        ApplyFastFall();
        ApplySlideWall();
    }

    private void HandleMovement()
    {

        if (isWallSliding)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        UpdateSpeedMultiplier();

        float targetSpeed = speed * speedMultiplier * relativeTransform.x;
        if (isOnPlatform)
        {
            rb.velocity = new Vector2(targetSpeed + platformRb.velocity.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
        }
    }

    private void UpdateSpeedMultiplier()
    {
        if (btnPressed && speedMultiplier < 1)
        {
            speedMultiplier += Time.deltaTime * acceleration;
        }
        else if (!btnPressed && speedMultiplier > 0)
        {
            speedMultiplier -= Time.deltaTime * acceleration;
            if (speedMultiplier < 0) speedMultiplier = 0;
        }
    }

    private void HandleWallDetection()
    {
        bool isOnWall = Physics2D.OverlapBox(wallCheckPoint.position, new Vector2(0.1f, 0.5f), 0, wallLayer);

        if (isOnWall)
        {
            Flip();
            return;
        }

        bool isOnSlideWall = Physics2D.OverlapBox(wallCheckPoint.position, new Vector2(0.1f, 0.5f), 0, slideWallLayer);

        if (isOnSlideWall)
        {
            if (!isWallSliding)
            {
                isWallSliding = true;
                wallSlideTimer = wallSlideDuration;
            }
        }

        if (isWallSliding)
        {
            wallSlideTimer -= Time.fixedDeltaTime;

            if (wallSlideTimer <= 0 || isGrounded || !isOnSlideWall)
            {
                isWallSliding = false;
                Flip();
            }
        }
    }

    private void HandleGroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, new Vector2(0.6f, 0.1f), 0, collisionLayers);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            isJumping = false;
            rb.gravityScale = 1f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleCornerCheck()
    {
        isCorner = Physics2D.OverlapBox(cornerCheckPoint.position, new Vector2(0.723f, 0.713f), 0, collisionLayers);

        if (isCorner)
        {
            Vector2 currentVelocity = rb.velocity;

            if (currentVelocity.magnitude < 0.1f && Time.time - lastBounceTime > cornerBounceCooldown)
            {
                lastBounceTime = Time.time;
                float horizontalBounce = transform.localScale.x > 0 ? -1 : 1;
                rb.velocity = new Vector2(-horizontalBounce * cornerBounceForce, cornerBounceForce);
            }
        }
    }

    private IEnumerator EnableMovementAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
    }

    private void ApplySlideWall()
    {
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    private void ApplyFastFall()
    {
        if (rb.velocity.y < 0 && !isGrounded)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fastFallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public void Move(InputAction.CallbackContext value)
    { 
        if (value.started) btnPressed = true;
        else if (value.canceled) btnPressed = false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isWallSliding && context.performed)
        {
            isWallJumping = true;
            isWallSliding = false;

            wallDirection = transform.localScale.x > 0 ? 1 : -1;

            rb.velocity = new Vector2(-wallDirection * wallJumpForceX, wallJumpForceY);
            isJumping = true;

            Flip();
            return;
        }

        if (isWallJumping && context.started && isJumping)
        {
            rb.velocity += Vector2.up * jumpMultiplier * Time.deltaTime;
        }

        if (context.performed && !isJumping && (isGrounded || (coyoteTimeCounter < coyoteTime && coyoteTimeCounter > 0)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            coyoteTimeCounter = 0;
        }

        if (context.started && isJumping && !isGrounded)
        {
                rb.velocity += Vector2.up * jumpMultiplier * Time.deltaTime;
        }

        if (context.canceled && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            isJumping = false;

            if (isWallJumping) isWallJumping = false;
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        UpdateRelativeTransform();
    }

    private void UpdateRelativeTransform()
    {
        relativeTransform = transform.InverseTransformVector(Vector2.one);
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {  
            rb.gravityScale = 0.4f;

        }
        else if (context.canceled)
        {
            rb.gravityScale = 1f;
        }
    }

    private void Update()
    {
    }
}
