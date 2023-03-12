using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Vector3 grapperPoint;
    [SerializeField] private Transform grappleTransform, cameraTransform, playerTransform;
    [SerializeField] private float maxDistance = 120.0f;

    
    public LayerMask grapableLayers;
    private RaycastHit grapple;
    private SpringJoint joint;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();    
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartGrapping();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopGraping();
        }
    }

    private void StartGrapping()
    {
        float distanceFromPoint;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out grapple, maxDistance, grapableLayers))
        {
            grapperPoint = grapple.point;
            joint = playerTransform.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapperPoint;

            distanceFromPoint = Vector3.Distance(playerTransform.position, grapperPoint);
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

        }
    }
    private void StopGraping()
    {

    }
}
