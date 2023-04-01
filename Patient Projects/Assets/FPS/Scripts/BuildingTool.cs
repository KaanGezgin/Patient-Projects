using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTool : MonoBehaviour
{
    public BaseManagment buildingLocation;
    [SerializeField] private float toolRayDistance;
    [SerializeField] private LayerMask buildableLayer;
    [SerializeField] private LayerMask buildsLayer;
    private int spawningPoint = 8;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Material buildingMaterial;
    [SerializeField] private Material deletingMaterial;


    private bool deleteMode;
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;

    }

    private void Update()
    {
        BuildControl();
    }

    private void BuildControl()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            deleteMode = !deleteMode;
        }

        if (deleteMode == false)
        {
            PlaceBuilding();
        }
        else
        {
            DestroyBuildings();
        }
    }

    private void PlaceBuilding()
    {
        if (!isHittingSomething(buildableLayer, out RaycastHit hitInfo))
        {
            buildingLocation.UpdateMaterial(buildingMaterial);
        }
        else
        {
            buildingLocation.UpdateMaterial(deletingMaterial);
            buildingLocation.transform.position = hitInfo.point;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            BaseManagment placedBuildings = Instantiate(buildingLocation, hitInfo.point, Quaternion.identity);
            placedBuildings.PlaceBuildings();
        }
    }
    private void DestroyBuildings()
    {
        if (!isHittingSomething(buildsLayer, out RaycastHit hitInfo))
        {
            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Destroy(hitInfo.collider.gameObject);
            }
        }
    }

    private bool isHittingSomething(LayerMask layermask, out RaycastHit hitInfo)
    {
        Ray ray = new Ray(rayOrigin.position, mainCamera.transform.forward * toolRayDistance);
        return Physics.Raycast(ray, out hitInfo, toolRayDistance, layermask);
    }
}
