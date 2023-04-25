using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*
    [Header("Spaceship")]
    public FlyingTest spaceship;
    public GameObject spaceshipObj;
    */
    [Header("Player Attributes")]
    [SerializeField] Rigidbody playerBody;
    [SerializeField] Transform playerTransform;

    [Header("Movement")]
    [SerializeField] float groundDrag;
    [SerializeField] Transform face;
    Vector3 moveDirection;

    private float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float slideSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] float wallrunSpeed;
    bool walkControl;
    public bool climbing;
    public bool wallRunning;
    public bool crouching;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public bool sliding;
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    [Header("Crouching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;
    float startYScale;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airFriction;
    bool jumpReady;
    bool onSlopeJump;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask isGround;
    public bool grounded;

    [Header("States")]
    [SerializeField] MovementState movementStates;

    [Header("Slope Handling")]
    [SerializeField] RaycastHit slopeControlHit;
    public float sloopAngle;

    [Header("Inputs")]
    float horizontalInput;
    float verticalInput;
    public KeyCode jump = KeyCode.Space;
    public KeyCode sprint = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.C;
    public KeyCode slide = KeyCode.LeftControl;

    [Header("Camera")]
    private Camera mainCamera; 

    public enum MovementState
    {
        walking,
        spriting,
        crouch,
        sliding,
        wallrun,
        air
    }
  
    private void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerBody.freezeRotation = true;
        jumpReady = true;
        startYScale = playerTransform.localScale.y;
    }
    private void Update()
    {
        PlayerInputs();
        GroundCheck();
        SpeedControl();
        StateHandler();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void StateHandler()
    {
        //walkControl = true;

        if (wallRunning)
        {
            movementStates = MovementState.wallrun;
            desiredMoveSpeed = wallrunSpeed;
        }

        else if (sliding)
        {
            movementStates = MovementState.sliding;
            if (OnSlope() && playerBody.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        else if (crouching)
        {
            movementStates = MovementState.crouch;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (grounded == true && Input.GetKey(sprint))
        {
            movementStates = MovementState.spriting;
            desiredMoveSpeed = sprintSpeed;
        }
        else if (grounded == true)
        {
            movementStates = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else if (grounded == false)
        {
            movementStates = MovementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(playerTransform.position, Vector3.down, playerHeight * 0.5f + 0.2f, isGround);

        if (grounded)
        {
            playerBody.drag = groundDrag;
        }
        else
        {
            playerBody.drag = 0;
        }
    }
    private void PlayerInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(crouch) && horizontalInput == 0 && verticalInput == 0)
        {
            playerTransform.localScale = new Vector3(playerTransform.localScale.x, crouchYScale, playerTransform.localScale.z);
            playerBody.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            crouching = true;
        }
        if (Input.GetKeyUp(crouch))
        {
            playerTransform.localScale = new Vector3(playerTransform.localScale.x, startYScale, playerTransform.localScale.z);
            crouching = false;
        }
        if (Input.GetKey(jump) && jumpReady && grounded)
        {
            jumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if(wallRunning == false)
        {
            playerBody.useGravity = !OnSlope();
        }
    }
    
    private IEnumerator SmoothLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;
        float slopeAngle;
        float slopeAngleIncrease;


        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                slopeAngle = Vector3.Angle(Vector3.up, slopeControlHit.normal);
                slopeAngleIncrease = 1 + (slopeAngle / 90f);
                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }
            yield return null;
        }
        moveSpeed = desiredMoveSpeed;
    }
    private void Movement()
    {
        moveDirection = face.forward * verticalInput + face.right * horizontalInput;

        if (OnSlope() && !onSlopeJump)
        {
            playerBody.AddForce(SlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            
            if(playerBody.velocity.y > 0)
            {
                playerBody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
       else if (grounded == true)
        {
            playerBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(grounded == false)
        {
            playerBody.AddForce(moveDirection.normalized * moveSpeed * 10f * airFriction, ForceMode.Force);
        }
        //playerBody.useGravity = !OnSlope();

    }
    private void SpeedControl()
    {
        Vector3 flatSpeedValue;
        Vector3 limitedVelocity;

        if (OnSlope() && !onSlopeJump)
        {
            if (playerBody.velocity.magnitude > moveSpeed)
            {
                playerBody.velocity = playerBody.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            flatSpeedValue = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z);

            if (flatSpeedValue.magnitude > moveSpeed)
            {
                limitedVelocity = flatSpeedValue.normalized * moveSpeed;
                playerBody.velocity = new Vector3(limitedVelocity.x, playerBody.velocity.y, limitedVelocity.z);
            }
        }
    }
    private void Jump()
    {
        onSlopeJump = true;
        playerBody.velocity = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z);
        playerBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        jumpReady = true;
        onSlopeJump = false;

    }
    public bool OnSlope()
    {
        float angle;

        if(Physics.Raycast(transform.position, Vector3.down, out slopeControlHit, playerHeight * 0.5f + 0.3f))
        {
            angle = Vector3.Angle(Vector3.up, slopeControlHit.normal);
            return angle < sloopAngle && angle != 0;
        }
        return false;
    }
    public Vector3 SlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeControlHit.normal).normalized;
    }
  
}
