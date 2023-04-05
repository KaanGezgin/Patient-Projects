using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [Header("Base Object")]
    public GameObject buildingTemplate;
    public GameObject buildingForesight;

    //public GameObject chosenBuilding;
    [Header("Layers")]
    [SerializeField] private LayerMask buildableLayer;
    [Header("Builder")]
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float toolRayDistance;
    [SerializeField] private bool buildable;

    [Header("Camera")]
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        BuildControl();
    }

    void BuildControl()
    {
        //BaseManagment build;
        buildingForesight = buildingTemplate;


        if (isHittingSomething(buildableLayer, out RaycastHit hitInfo))
        {
            buildable = true;


            if (buildable)
            {

                buildingTemplate.transform.position = hitInfo.point;

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Instantiate(buildingTemplate, hitInfo.point, Quaternion.identity);
                }
            }
        }
        else
        {
            buildable = false;
            ChangeMaterial();
        }
        
    }
    private bool isHittingSomething(LayerMask layermask, out RaycastHit hitInfo)
    {
        Ray builderController = new Ray(rayOrigin.position, mainCamera.transform.forward * toolRayDistance);
        return Physics.Raycast(builderController, out hitInfo, toolRayDistance, layermask);
    }
    void ChangeMaterial()
    {

    }
}
