using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    // [SerializeField] movementLimiter moveLimit;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private characterGround ground;
    private characterJump jump;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)] public float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] public float acceleration = 52f;
    [SerializeField, Range(0f, 500f)] public float deceleration = 52f;
    [SerializeField, Range(0f, 20f)] public float wallSlidingSpeed = 52f;

    [Header("Wall Jump")]
   // private bool isWallJumping;
   // private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(8, 16);
    private int lastWallTouched = 0;
    private int currentWall = 0;

    [Header("Other")]
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private LayerMask wallLayer;

    [Header("Calculations")]
    private float directionX;
    private Vector2 desiredVelocity;
    [HideInInspector] public Vector2 velocity;

    [Header("Current State")]
    public bool onGround;
    private bool movingInput;
    private bool moving;
    private bool pressingKey;
    public bool isWallSliding;

    [SerializeField] private bool WallJumpUnlocked = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ground = GetComponent<characterGround>();
        jump = GetComponent<characterJump>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pressingKey = true;
        }
        if (context.canceled)
        {
            pressingKey = false;
        }
        Vector2 inputVector = context.ReadValue<Vector2>();
        directionX = inputVector.x;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jump.desiredJump = true;
            
            if(wallJumpCounter > 0 && WallJumpUnlocked)
            {
                if(lastWallTouched == currentWall)
                {
                    return;
                }
                wallJumpCounter = 0;
                rb.velocity = new Vector2(wallJumpForce.x * currentWall * -1f, wallJumpForce.y);
                lastWallTouched = currentWall;
                Invoke(nameof(StopWallJump), wallJumpDuration);
                Invoke(nameof(ResetWallJump), 3f);
            }
        }
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (directionX != 0)
        {
            sr.flipX = directionX > 0 ? false : true;
            jump.lookingRight = !sr.flipX;
        }
        desiredVelocity = new Vector2(directionX, 0f) * maxSpeed;
        WallSlide();
        WallJump();
    }
    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = rb.velocity;
        run();
    }
    private void run()
    {
        float maxSpeedChange = acceleration * Time.deltaTime;
        if (pressingKey)
        {
            if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x))
            {//start accelerating on the other direction
                velocity.x = 0f;
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            }
            else
            {
                //If they match, accelerate
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            }
        }
        else
        {//decelerate
            maxSpeedChange = deceleration * Time.deltaTime;
            desiredVelocity = Vector2.zero;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        }
        rb.velocity = velocity;
    }
    private bool IsOnWall()
    {
        if(Physics2D.OverlapCircle(leftWallCheck.position, 0.2f, wallLayer))
        {
            currentWall = -1;
            return true;
        }
        if (Physics2D.OverlapCircle(rightWallCheck.position, 0.2f, wallLayer))
        {
            currentWall = 1;
            return true;
        }
        return false;
    }
    private void WallSlide()
    {
        if (IsOnWall() && !onGround && velocity.y < 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            wallJumpCounter = wallJumpTime;
            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }
    }
    private void ResetWallJump()
    {
        lastWallTouched = 0;
    }
    private void StopWallJump()
    {
       // isWallJumping = false;
        wallJumpCounter = 0;
    }
    public void UnlockWallJump()
    {
        WallJumpUnlocked = true;
    }
}