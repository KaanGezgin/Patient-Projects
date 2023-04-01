using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour
{
    [Header("Movement")]
    public LayerMask wall;
    public LayerMask groundCheck;
    public float wallRunForce;
    public float wallRunTime;
    private float wallRunTimer;

    [Header("Controls")]
    public float wallDistance;
    public float jumpHeight;
    public float wallClimbSpeed;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform visor;
    private PlayerMovement playerMovement;
    private Rigidbody playerBody;

    [Header("Input")]
    private float horizontal;
    private float vertical;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
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
        wallRight = Physics.Raycast(transform.position, visor.right, out rightWallHit, wallDistance, wall);
        wallLeft = Physics.Raycast(transform.position, -visor.right, out leftWallHit, wallDistance, wall);
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


        if((wallLeft || wallRight) && vertical > 0 && AboveGround())
        {
            if (!playerMovement.wallRunning)
            {
                StartWallRun();
            }
            else
            {
                if (playerMovement.wallRunning)
                {
                    EndWallRun();
                }
            }
        }
    }
    private void StartWallRun()
    {
        playerMovement.wallRunning = true;
    }
    private void EndWallRun()
    {
        playerMovement.wallRunning = false;
    }
    private void WallrunMovement() 
    {
        playerBody.useGravity = false;
        playerBody.velocity = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z);

        Vector3 wall = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wall, transform.up);

        if((visor.forward - wallForward).magnitude > (visor.forward - -wallForward).magnitude)
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
    }
}

