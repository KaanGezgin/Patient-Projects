using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTool : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask buildModeLayerMask;
    [SerializeField] private LayerMask deleteModeLayerMask;
    [SerializeField] private int defaultLayerInt = 8;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Material buildingMatPositive;
    [SerializeField] private Material buildingMatNegative;

    private bool deleteModeEnable;
    private Camera mainCamera;
    
    [SerializeField] private Building _spawnedBuilging;
    private Building targetBuilding;


    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            deleteModeEnable = !deleteModeEnable;
        }
        if (deleteModeEnable)
        {
            DeleteMode();
        }
        if (!deleteModeEnable)
        {
            Build();
        }
       
    }

    private bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(rayOrigin.position, mainCamera.transform.forward * rayDistance);
        return Physics.Raycast(ray, out hitInfo, rayDistance, layerMask);
    }

    private void Build()
    {
        if(targetBuilding != null && targetBuilding.FlaggedForDelete)
        {
            targetBuilding.RemoveFlag();
            targetBuilding = null;
        }

        if(_spawnedBuilging is null)
        {
            return;
        }

        if (!IsRayHittingSomething(buildModeLayerMask, out RaycastHit hitInfo))
        {
            _spawnedBuilging.UpdateMaterail(buildingMatNegative);
        }
        else
        {
            _spawnedBuilging.UpdateMaterail(buildingMatPositive);
            //_spawnedBuilging.transform.position = hitInfo.point; Gridless building
            var gridPosition = WorldGrid.GridPositionFromWorld(hitInfo.point, 1f);
            _spawnedBuilging.transform.position = gridPosition;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Building placeBuilding = Instantiate(_spawnedBuilging, gridPosition, Quaternion.identity);
                placeBuilding.PlaceBuilding();
            }
        }
        /* Gridless building 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Building placeBuilding = Instantiate(_spawnedBuilging, hitInfo.point, Quaternion.identity);
            placeBuilding.PlaceBuilding();
        }
        */
    }
    private void DeleteMode()
    {
        if (IsRayHittingSomething(deleteModeLayerMask, out RaycastHit hitInfo))
        {

            var detectedBuiding = hitInfo.collider.gameObject.GetComponentInParent<Building>();

            if (detectedBuiding == null)
            {
                return;
            }
            if (targetBuilding == null)
            {
                targetBuilding = detectedBuiding;
            }
            if (detectedBuiding != targetBuilding && targetBuilding.FlaggedForDelete)
            {
                targetBuilding.RemoveFlag();
                targetBuilding = detectedBuiding;
            }
            if (detectedBuiding == targetBuilding && !targetBuilding.FlaggedForDelete)
            {
                targetBuilding.FlagForDelete(buildingMatNegative);
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Destroy(targetBuilding.gameObject);
                targetBuilding = null;
            }
        }
        else
        {
            if(targetBuilding != null && targetBuilding.FlaggedForDelete)
            {
                targetBuilding.RemoveFlag();
                targetBuilding = null;
            }
        }
    }
}
