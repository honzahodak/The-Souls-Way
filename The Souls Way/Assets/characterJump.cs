using UnityEngine;
using UnityEngine.InputSystem;

//This script handles moving the character on the Y axis, for jumping and gravity

public class characterJump : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] public Rigidbody2D rb;
    private characterGround ground;
    [HideInInspector] public Vector2 velocity;
    private characterJuice juice;
   // [SerializeField] movementLimiter moveLimit;

    [Header("Jumping Stats")]
    [SerializeField, Range(2f, 5.5f)][Tooltip("Maximum jump height")] public float jumpHeight = 7.3f;
    [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier to apply when coming down")] public float downwardMovementMultiplier = 6.17f;
    [SerializeField, Range(0, 2)][Tooltip("How many times can you jump in the air?")] public int maxAirJumps = 0;

    [Header("Options")]
    [SerializeField][Tooltip("The fastest speed the character can fall")] public float fallSpeedLimit;
    [SerializeField, Range(0f, 0.3f)][Tooltip("How long should coyote time last?")] public float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)][Tooltip("How far from ground should we cache jump?")] public float jumpBuffer = 0.15f;
    public float defaultGravity = 10f;

    [Header("Calculations")]
    private float jumpForce;
    private float gravMultiplier;

    [Header("Current State")]
    public int remainingAirJumps;
    private bool desiredJump;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
   // private bool pressingJump;
    public bool onGround;
    public bool inJump;
    private bool inJumpProtect;

    void Awake()
    {
        //Find the character's Rigidbody and ground detection and juice scripts
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<characterGround>();
        juice = GetComponentInChildren<characterJuice>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            desiredJump = true;
          //  pressingJump = true;
        }
        if (context.canceled)
        {
           // pressingJump = false;
        }
    }
    void Update()
    {
        onGround = ground.GetOnGround();

        if (onGround)
        {
            remainingAirJumps = maxAirJumps;
            if (!inJumpProtect)
                inJump = false;
            else
                inJumpProtect = false;
        }
        if (desiredJump)
        {
            jumpBufferCounter += Time.deltaTime;
                //If time exceeds the jump buffer, turn off "desireJump"
            if (jumpBufferCounter > jumpBuffer)
            {
                desiredJump = false;
                jumpBufferCounter = 0;
            }
        }

        //If stepped off the edge
        if (!inJump && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
           // print(coyoteTimeCounter);
        }
        else
        {
            //Reset it when touch the ground, or jump
            coyoteTimeCounter = 0;
        }
    }
    private void FixedUpdate()
    {
        
        //Keep trying to do a jump, for as long as desiredJump is true
        if (desiredJump)
        {
            DoAJump();
            return;
        }
        //Clamp the Y variable within the bounds of the speed limit, for the terminal velocity
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -fallSpeedLimit, 100));
        calculateGravity();
    }

    private void calculateGravity()
    {
        if(rb.velocity.y > 0.01f)
        {
            gravMultiplier = 1f;
        }
        else if (rb.velocity.y < -0.01f)
        {
            if (onGround || !inJump)
            {
                gravMultiplier = 1f;
            }
            else
            {
                gravMultiplier = downwardMovementMultiplier;
            }
        }
        //Else not moving vertically at all
        else
        {
            onGround = true;
            gravMultiplier = 1f;
        }
        rb.gravityScale = defaultGravity * gravMultiplier;
    }

    private void DoAJump()
    {
        //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
        if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime) || remainingAirJumps >= 1)
        {
            if(!onGround && !(coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime))
            { //if not onGroung and not used coyote time, must have used doublejump
                remainingAirJumps--;
            }
            inJump = true; inJumpProtect = true;
            onGround = false;
            desiredJump = false;

            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.gravityScale = defaultGravity;
            rb.AddForce(Vector2.up * jumpHeight * defaultGravity, ForceMode2D.Impulse);

            if (juice != null)
            {
                //Apply the jumping effects on the juice script
                juice.jumpEffects();
            }
        }
        if (jumpBuffer == 0)
        {
            //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
            desiredJump = false;
        }
    }
    public void bounceUp(float bounceAmount)
    {
        //Used by the springy pad
        rb.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
    }
}