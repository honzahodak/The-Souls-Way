using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    // [SerializeField] movementLimiter moveLimit;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    characterGround ground;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)] public float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] public float acceleration = 52f;

    [Header("Calculations")]
    private float directionX;
    private Vector2 desiredVelocity;
    [HideInInspector]public Vector2 velocity;


    [Header("Current State")]
    public bool onGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ground = GetComponent<characterGround>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        directionX = inputVector.x;
    }

    private void Update()
    {
        if (directionX != 0)
        {
            sr.flipX = directionX > 0 ? false : true;
           // transform.localScale = new Vector3(directionX > 0 ? 1 : -1, 1, 1);
        }
        desiredVelocity = new Vector2(directionX, 0f) * maxSpeed;
    }
    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = rb.velocity;

        run();
    }
    private void run()
    {   
        float maxSpeedChange = acceleration * Time.deltaTime; ;
        if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x))
        {
            velocity.x = 0f;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        }
        else
        {
            //If they match, accelerate
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        }
        rb.velocity = velocity;
    }
}
