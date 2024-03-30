//using System;
//using System.Collections;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{

//    [Header("Assignable")]
//    public Transform orientation;

//    //Other
//    private Rigidbody rb;

//    [Header("Movement")]
//    public float moveSpeed = 4500;
//    public float maxSpeed = 20;
//    public bool grounded;
//    public LayerMask whatIsGround;

//    public float counterMovement = 0.175f;
//    private float threshold = 0.01f;
//    public float maxSlopeAngle = 35f;

//    [Header("Sprinting")]
//    [SerializeField] float maxWalkSpeed = 4f;
//    [SerializeField] float maxSprintSpeed = 6f;
//    [SerializeField] float acceleration = 10f;

//    [Header("Crouch & Slide")]
//    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
//    private Vector3 playerScale;
//    public float slideForce = 400;
//    public float slideCounterMovement = 0.2f;
//    public bool sliding;

//    [Header("Jumping")]
//    private bool readyToJump = true;
//    private bool canDoubleJump;
//    private float jumpCooldown = 0.25f;
//    public float jumpForce = 550f;


//    //Input
//    float x, y;
//    public bool jumping, sprinting, crouching;


//    [Header("Keybinds")]
//    [SerializeField] KeyCode jumpKey = KeyCode.Space;
//    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
//    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

//    //Sliding
//    private Vector3 normalVector = Vector3.up;
//    private Vector3 wallNormalVector;

//    private bool canMove = true; //If player is not hitted
//    private bool isStuned = false;
//    private bool wasStuned = false; //If player was stunned before get stunned another time
//    private float pushForce;
//    private Vector3 pushDir;

//    public Vector3 checkPoint;
//    private bool slide = false;
//    public float gravity = 10.0f;

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    void Start()
//    {
//        playerScale = transform.localScale;
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }


//    private void FixedUpdate()
//    {
//        Movement();
//    }

//    private void Update()
//    {
//        MyInput();
//        ControlSpeed();
//    }

//    /// <summary>
//    /// Find user input. Should put this in its own class but im lazy
//    /// </summary>
//    private void MyInput()
//    {
//        x = Input.GetAxisRaw("Horizontal");
//        y = Input.GetAxisRaw("Vertical");
//        jumping = Input.GetButton("Jump");
//        crouching = Input.GetKey(KeyCode.LeftControl);

//        //Crouching
//        if (Input.GetKeyDown(crouchKey))
//            StartCrouch();
//        if (Input.GetKeyUp(crouchKey))
//            StopCrouch();
//    }

//    private void StartCrouch()
//    {
//        transform.localScale = crouchScale;
//        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
//        if (rb.velocity.magnitude > 0.5f)
//        {
//            if (grounded)
//            {
//                rb.AddForce(orientation.transform.forward * slideForce);
//            }
//        }
//    }

//    private void StopCrouch()
//    {
//        transform.localScale = playerScale;
//        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
//    }

//    private void Movement()
//    {
//        //Extra gravity
//        rb.AddForce(Vector3.down * Time.deltaTime * 10);

//        //Find actual velocity relative to where player is looking
//        Vector2 mag = FindVelRelativeToLook();
//        float xMag = mag.x, yMag = mag.y;

//        //Counteract sliding and sloppy movement
//        CounterMovement(x, y, mag);

//        //If holding jump && ready to jump, then jump
//        if (readyToJump && jumping) Jump();

//        //Set max speed
//        float maxSpeed = this.maxSpeed;

//        //If sliding down a ramp, add force down so player stays grounded and also builds speed
//        if (crouching && grounded && readyToJump)
//        {
//            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
//            return;
//        }

//        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
//        if (x > 0 && xMag > maxSpeed) x = 0;
//        if (x < 0 && xMag < -maxSpeed) x = 0;
//        if (y > 0 && yMag > maxSpeed) y = 0;
//        if (y < 0 && yMag < -maxSpeed) y = 0;

//        //Some multipliers
//        float multiplier = 1f, multiplierV = 1f;

//        // Movement in air
//        if (!grounded)
//        {
//            multiplier = 0.5f;
//            multiplierV = 0.5f;
//        }

//        // Movement while sliding
//        if (grounded && crouching) multiplierV = 0f;

//        //Apply forces to move player
//        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
//        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
//    }

//    void ControlSpeed()
//    {
//        if (Input.GetKey(sprintKey) && grounded)
//        {
//            maxSpeed = Mathf.Lerp(maxSpeed, maxSprintSpeed, acceleration * Time.deltaTime);
//        }
//        else
//        {
//            maxSpeed = Mathf.Lerp(maxSpeed, maxWalkSpeed, acceleration * Time.deltaTime);
//        }
//    }


//    private void Jump()
//    {
//        if (grounded && readyToJump)
//        {
//            readyToJump = false;
//            canDoubleJump = true;

//            //Add jump forces
//            rb.AddForce(Vector2.up * jumpForce * 1.5f);
//            rb.AddForce(normalVector * jumpForce * 0.5f);

//            //If jumping while falling, reset y velocity.
//            Vector3 vel = rb.velocity;
//            if (rb.velocity.y < 0.5f)
//                rb.velocity = new Vector3(vel.x, 0, vel.z);
//            else if (rb.velocity.y > 0)
//                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

//            Invoke(nameof(ResetJump), jumpCooldown);
//        }
//    }

//    private void ResetJump()
//    {
//        readyToJump = true;
//        canDoubleJump = false;
//    }

//    private float desiredX;


//    private void CounterMovement(float x, float y, Vector2 mag)
//    {
//        if (!grounded || jumping) return;

//        //Slow down sliding
//        if (crouching)
//        {
//            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
//            return;
//        }

//        //Counter movement
//        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
//        {
//            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
//        }
//        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
//        {
//            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
//        }

//        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
//        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
//        {
//            float fallspeed = rb.velocity.y;
//            Vector3 n = rb.velocity.normalized * maxSpeed;
//            rb.velocity = new Vector3(n.x, fallspeed, n.z);
//        }
//    }

//    /// <summary>
//    /// Find the velocity relative to where the player is looking
//    /// Useful for vectors calculations regarding movement and limiting movement
//    /// </summary>
//    /// <returns></returns>
//    public Vector2 FindVelRelativeToLook()
//    {
//        float lookAngle = orientation.transform.eulerAngles.y;
//        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

//        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
//        float v = 90 - u;

//        float magnitue = rb.velocity.magnitude;
//        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
//        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

//        return new Vector2(xMag, yMag);
//    }

//    private bool IsFloor(Vector3 v)
//    {
//        float angle = Vector3.Angle(Vector3.up, v);
//        return angle < maxSlopeAngle;
//    }

//    private bool cancellingGrounded;

//    /// <summary>
//    /// Handle ground detection
//    /// </summary>
//    private void OnCollisionStay(Collision other)
//    {
//        //Make sure we are only checking for walkable layers
//        int layer = other.gameObject.layer;
//        if (whatIsGround != (whatIsGround | (1 << layer))) return;

//        //Iterate through every collision in a physics update
//        for (int i = 0; i < other.contactCount; i++)
//        {
//            Vector3 normal = other.contacts[i].normal;
//            //FLOOR
//            if (IsFloor(normal))
//            {
//                grounded = true;
//                cancellingGrounded = false;
//                normalVector = normal;
//                CancelInvoke(nameof(StopGrounded));
//            }
//        }

//        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
//        float delay = 3f;
//        if (!cancellingGrounded)
//        {
//            cancellingGrounded = true;
//            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
//        }
//    }

//    private void StopGrounded()
//    {
//        grounded = false;
//    }

//    public void HitPlayer(Vector3 velocityF, float time)
//    {
//        rb.velocity = velocityF;

//        pushForce = velocityF.magnitude;
//        pushDir = Vector3.Normalize(velocityF);
//        StartCoroutine(Decrease(velocityF.magnitude, time));
//    }

//    private IEnumerator Decrease(float value, float duration)
//    {
//        if (isStuned)
//            wasStuned = true;
//        isStuned = true;
//        canMove = false;

//        float delta = 0;
//        delta = value / duration;

//        for (float t = 0; t < duration; t += Time.deltaTime)
//        {
//            yield return null;
//            if (!slide) //Reduce the force if the ground isnt slide
//            {
//                pushForce = pushForce - Time.deltaTime * delta;
//                pushForce = pushForce < 0 ? 0 : pushForce;
//                //Debug.Log(pushForce);
//            }
//            rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0)); //Add gravity
//        }

//        if (wasStuned)
//        {
//            wasStuned = false;
//        }
//        else
//        {
//            isStuned = false;
//            canMove = true;
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float maxYSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool jumping;
    public bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        dashing,
        air
    }

    public bool dashing;

    private bool canMove = true;
  
    private bool isStuned = false;
    private bool wasStuned = false; //If player was stunned before get stunned another time
    private float pushForce;
    private Vector3 pushDir;

    public Vector3 checkPoint;
    private bool slide = false;
    public float gravity = 10.0f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

     
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;
    private void StateHandler()
    {
        // Mode - Dashing
        if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }

        // Mode - Crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;

            if (desiredMoveSpeed < sprintSpeed)
                desiredMoveSpeed = walkSpeed;
            else
                desiredMoveSpeed = sprintSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void MovePlayer()
    {
        if (state == MovementState.dashing) return;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        // limit y vel
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public void HitPlayer(Vector3 velocityF, float time)
    {
        rb.velocity = velocityF;

        pushForce = velocityF.magnitude;
        pushDir = Vector3.Normalize(velocityF);
        StartCoroutine(Decrease(velocityF.magnitude, time));
    }

    private IEnumerator Decrease(float value, float duration)
    {
        if (isStuned)
            wasStuned = true;
        isStuned = true;
        canMove = false;

        float delta = 0;
        delta = value / duration;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;
            if (!slide) //Reduce the force if the ground isnt slide
            {
                pushForce = pushForce - Time.deltaTime * delta;
                pushForce = pushForce < 0 ? 0 : pushForce;
                //Debug.Log(pushForce);
            }
            rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0)); //Add gravity
        }

        if (wasStuned)
        {
            wasStuned = false;
        }
        else
        {
            isStuned = false;
            canMove = true;
        }
    }
}
