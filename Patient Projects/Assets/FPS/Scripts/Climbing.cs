using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform visor;
    public Rigidbody playerBody;
    public PlayerMovement playerMovement;
    public LayerMask climbable;

    [Header("Climb Movement")]
    public float climbSpeed;
    public float climbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Controll Variables")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private void Update()
    {
        WallCheck();
        StateMachine();
        if (climbing)
        {
            ClimbingMovement();
        }
    }
    private void StateMachine()
    {
        if(wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if(!climbing && climbTimer > 0)
            {
                StartClimbing();
            }
            if(climbTimer > 0)
            {
                climbTimer -= Time.deltaTime;
            }
            if(climbTimer < 0)
            {
                EndClimbing();
            }
        }
        else
        {
            if (climbing)
            {
                EndClimbing();
            }
        }
    }
    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, visor.forward, out frontWallHit, detectionLength, climbable);
        wallLookAngle = Vector3.Angle(visor.forward, -frontWallHit.normal);
        if (playerMovement.grounded)
        {
            climbTimer = climbTime;
        }
    }
    private void StartClimbing()
    {
        climbing = true;

    }
    private void ClimbingMovement()
    {
        playerBody.velocity = new Vector3(playerBody.velocity.x, climbSpeed, playerBody.velocity.z);

    }
    private void EndClimbing()
    {
        climbing = false;

    }


}
