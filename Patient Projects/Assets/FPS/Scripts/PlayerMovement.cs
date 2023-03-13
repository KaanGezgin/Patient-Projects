using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] Rigidbody playerBody;
    [SerializeField] Transform playerTransform;

    [Header("Movement")]
    Vector3 moveDirection;
    [SerializeField] float groundDrag;
    [SerializeField] Transform visor;
    private float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    bool walkControl;

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
    [SerializeField] private bool grounded;

    [Header("States")]
    [SerializeField] MovementState movementStates;

    [Header("Slope Handling")]
    [SerializeField] RaycastHit slopeControlHit;
    public float sloopAngle;




    [Header("Inputs")]
    float horizontalInput;
    float verticalInput;
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode crouch;
    public KeyCode slide;

    public enum MovementState
    {
        walking,
        spriting,
        crouch,
        air
    }
    private void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerBody.freezeRotation = true;
        jumpReady = true;
        startYScale = playerTransform.localScale.y;
        walkControl = true;
    
    }
    private void Update()
    {
        PlayerInputs();
        GroundCheck();
        SpeedControl();
    }
    private void FixedUpdate()
    {
        StateHandler();
        Movement();
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
        jump = KeyCode.Space;
        sprint = KeyCode.LeftShift;
        crouch = KeyCode.C;
        slide = KeyCode.LeftControl;

        if (Input.GetKeyDown(crouch))
        {
            playerTransform.localScale = new Vector3(playerTransform.localScale.x, crouchYScale, playerTransform.localScale.z);
            playerBody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouch))
        {
            playerTransform.localScale = new Vector3(playerTransform.localScale.x, startYScale, playerTransform.localScale.z);

        }

        
    }
    private void StateHandler()
    {
        walkControl = true;

        if (Input.GetKey(crouch))
        {
            movementStates = MovementState.crouch;
            moveSpeed = crouchSpeed;
            walkControl = false;

        }
        if (grounded == true && Input.GetKey(sprint))
        {
            movementStates = MovementState.spriting;
            moveSpeed = sprintSpeed;
            walkControl = false;
        }
        if (grounded == true && walkControl == true)
        {
            movementStates = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        if(grounded == false)
        {
            movementStates = MovementState.air;
        }
    }
    private void Movement()
    {
        moveDirection = visor.forward * verticalInput + visor.right * horizontalInput;

        if (OnSlope() && !onSlopeJump)
        {
            playerBody.AddForce(SlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            
            if(playerBody.velocity.y > 0)
            {
                playerBody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        playerBody.useGravity = !OnSlope();

        if (grounded == true)
        {
            playerBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(grounded == false)
        {
            playerBody.AddForce(moveDirection.normalized * moveSpeed * 10f * airFriction, ForceMode.Force);
        }
        if (Input.GetKey(jump) && jumpReady && grounded)
        {
            jumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
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
