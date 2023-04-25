using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Spaceship")]
    [SerializeField] FlyingTest spaceship;

    [Header("Follow components")]
    [SerializeField] Transform target;
    [SerializeField] Transform player;
    Transform cameraTransform;

    [Header("Camera distance")]
    [SerializeField] float xDistance = 0.0f;
    [SerializeField] float zDistance = -19.0f;//Between Target and camera distance
    [SerializeField] float height = 8.0f;//Between Target and camera height

    [Header("Sensitivity & damping control")]
    [SerializeField] float distanceDamping = 2.0f;
    [SerializeField] float rotationDamping = 3.0f;
    
    

    void Awake()
    {
        cameraTransform = transform;
    }

    void Start()
    {
        target = GameObject.Find("Look Target").GetComponent<Transform>();
        player = GameObject.Find("Spaceship").GetComponent<Transform>();
        spaceship = GameObject.Find("Spaceship").GetComponent<FlyingTest>();
    }


    void LateUpdate()
    {
        CameraFollow();
    }
    

    void CameraFollow()
    {
        Vector3 distance = new Vector3(xDistance, height, zDistance);
        Vector3 wantedPosition = target.position + (target.rotation * distance);
        Vector3 currentPosition = Vector3.Lerp(cameraTransform.position, wantedPosition, distanceDamping * Time.deltaTime);

        //Vector3 currentPosition = Vector3.SmoothDamp(cameraTransform.position, wantedPosition, ref velocity, distanceDamping);
        cameraTransform.position = currentPosition;

        Quaternion wantedRotaion = Quaternion.LookRotation(target.position - cameraTransform.position, target.up);
        Quaternion currentRotaion = Quaternion.Slerp(cameraTransform.rotation, wantedRotaion, rotationDamping * Time.deltaTime);
        cameraTransform.rotation = currentRotaion;
    }
}