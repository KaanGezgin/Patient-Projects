using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform face;
    public Transform playerTransform;
    public Transform playerObject;
    public Rigidbody playerBody;

    public float cameraRotationSpeed;

    [Header("Mouse Inputs")]
    private float mouseX;
    private float mouseY;
    private Vector3 inputDirection;

    [Header("Camere State")]
    [SerializeField] private CameraState cameraState;
    private Transform combatState;
    private enum CameraState
    {
        Default,
        Combat
    }
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector3 viewDir = playerTransform.position - new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
        face.forward = viewDir.normalized;
        if(cameraState == CameraState.Default)
        {
            DefaultState();
        }
        else if(cameraState == CameraState.Combat)
        {

        }
    }
    private void DefaultState()
    {
        mouseX = Input.GetAxis("Horizontal");
        mouseY = Input.GetAxis("Vertical");
        inputDirection = face.forward * mouseY + face.right * mouseX;
        if(inputDirection != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * cameraRotationSpeed);
        }
    }
    private void CombatState()
    {

    }
}
