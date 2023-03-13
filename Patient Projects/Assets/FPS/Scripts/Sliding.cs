using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform visor;
    public Transform playerTransform;
    private Rigidbody playerBody;
    private PlayerMovement playerMovement;

    [Header("Sliding")]
    private float slideTimer;
    private float startYScale;
    public float maxSliderTime;
    public float slideForce;
    public float slideYScale;
    private bool sliding;

    [Header("Inputs")]
    public KeyCode slide = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        startYScale = playerTransform.localScale.y;

    }
    public void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slide) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSliding();
        }
        if(Input.GetKeyUp(slide) && sliding)
        {
            EndSliding();
        }
    }
    public void FixedUpdate()
    {
        if (sliding)
        {
            SlideMovement();
        }
    }
    private void StartSliding()
    {
        sliding = true;
        playerTransform.localScale = new Vector3(playerTransform.localScale.x, slideYScale, playerTransform.localScale.z);
        playerBody.AddForce(Vector3.down * 5f, ForceMode.Force);
        slideTimer = maxSliderTime;
    }
    private void EndSliding()
    {
        sliding = false;

        playerTransform.localScale = new Vector3(playerTransform.localScale.x, startYScale, playerTransform.localScale.z);

    }
    private void SlideMovement()
    {
        Vector3 inputDirection = visor.forward * verticalInput + visor.right * horizontalInput;

        if (!playerMovement.OnSlope() || playerBody.velocity.y > -0.1f)
        {
            playerBody.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }

        else
        {
            playerBody.AddForce(playerMovement.SlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
        {
            EndSliding();
        }

    }
}
