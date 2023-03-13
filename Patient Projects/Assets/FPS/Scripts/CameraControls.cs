using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform visor;


    [Header("Mouse Inputs")]
    private float mouseX;
    private float mouseY;

    [Header("")]
    [SerializeField] private float xAxisSensivity;
    [SerializeField] private float yAxisSensivity;
    private float xAxisRotation;
    private float yAxisRotation;
   
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xAxisSensivity;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * yAxisSensivity;

        yAxisRotation += mouseX;
        xAxisRotation -= mouseY;
        xAxisRotation = Mathf.Clamp(xAxisRotation, -90f, 90f);

        cameraTransform.rotation = Quaternion.Euler(xAxisRotation, yAxisRotation, 0);
        visor.rotation = Quaternion.Euler(0, yAxisRotation, 0);

    }

}
