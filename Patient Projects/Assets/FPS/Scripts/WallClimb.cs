using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour
{
    [Header("Movement")]
    public LayerMask wall;
    public LayerMask groundCheck;
    public float wallClimbSpeed;
    public float wallRunForce;
    public float wallRunTime;
    private float wallRunTimer;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    private bool useGravity;
    private float gravityCounterForce;

    [Header("Controls")]
    [SerializeField] private float wallDistance;
    [SerializeField] private float jumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("References")]
    public Transform face;
    public CameraControls mainCamera;
    private PlayerMovement playerMovement;
    private Rigidbody playerBody;

    [Header("Input")]
    private float horizontal;
    private float vertical;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    public KeyCode jump = KeyCode.Space;
    public bool upwardsRunning;
    public bool downwardsRunning;


    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        CheckForWall();
        StateMachine();
    }
    private void FixedUpdate()
    {
        if (playerMovement.wallRunning)
        {
            WallrunMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, face.right, out rightWallHit, wallDistance, wall);
        wallLeft = Physics.Raycast(transform.position, -face.right, out leftWallHit, wallDistance, wall);
    }
    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, jumpHeight, wall);
    }
    private void StateMachine()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);


        if((wallLeft || wallRight) && vertical > 0 && AboveGround() && !exitingWall)
        {
            if (!playerMovement.wallRunning)
            {
                StartWallRun();
            }

            if(wallRunTime > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }
            
            if(wallRunTimer <= 0 && playerMovement.wallRunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }
            if (Input.GetKeyDown(jump))
            {
                WallJump();
            }
        }
        else if (exitingWall)
        {
            if (playerMovement.wallRunning)
            {
                EndWallRun();
            }

            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }
            if (exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }
        else
        {
            if (playerMovement.wallRunning)
                EndWallRun();
        }
    }
    private void StartWallRun()
    {
        playerMovement.wallRunning = true;
        wallRunTimer = wallRunTime;

        playerBody.velocity = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z);

    }
    private void EndWallRun()
    {
        playerMovement.wallRunning = false;
    }
    private void WallrunMovement() 
    {
        playerBody.useGravity = useGravity;
        //playerBody.velocity = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z);

        Vector3 wall = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wall, transform.up);

        if((face.forward - wallForward).magnitude > (face.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        playerBody.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (upwardsRunning)
        {
            playerBody.velocity = new Vector3(playerBody.velocity.x, wallClimbSpeed, playerBody.velocity.z);
        }
        if (downwardsRunning)
        {
            playerBody.velocity = new Vector3(playerBody.velocity.x, -wallClimbSpeed, playerBody.velocity.z);
        }

        if (!(wallLeft && horizontal > 0) && !(wallRight && horizontal < 0))
        {
            playerBody.AddForce(-wallForward * 100, ForceMode.Force);
        }
        if(useGravity == true)
        {
            playerBody.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        Vector3 wallNormal;
        Vector3 forceToApply;


        wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        playerBody.velocity = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z);
        playerBody.AddForce(forceToApply, ForceMode.Impulse);
    }
}

