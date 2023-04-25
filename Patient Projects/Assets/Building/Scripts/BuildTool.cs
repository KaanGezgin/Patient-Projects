using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTool : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask buildModeLayerMask;
    [SerializeField] private LayerMask deleteModeLayerMask;
    [SerializeField] private int defaultLayerInt = 11;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Material buildingMatPositive;
    [SerializeField] private Material buildingMatNegative;

    public bool deleteModeEnable;
    private Camera mainCamera;
    
    private Building spawnedBuilging;
    private Building targetBuilding;
    private Quaternion placedBuildingsLastRotation;



    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        BuildPanelUI.ChosenBuilding += ChooseBuilding;
    }
    private void OnDisable()
    {
        BuildPanelUI.ChosenBuilding -= ChooseBuilding;
    }


    private void ChooseBuilding(BuildingsData data)
    {
        if (deleteModeEnable)
        {
            if(targetBuilding != null && targetBuilding.FlaggedForDelete)
            {
                targetBuilding.RemoveFlag();
            }
            targetBuilding = null;
            deleteModeEnable = false;
        }
        DeleteObjectInPreview();
        var newBuildingGameObject = new GameObject
        {
            layer = defaultLayerInt,
            name = "Build Preview"
        };

        spawnedBuilging = newBuildingGameObject.AddComponent<Building>();
        spawnedBuilging.Init(data);
        spawnedBuilging.transform.rotation = placedBuildingsLastRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("heyo");

            DeleteObjectInPreview();
            /*
            if (deleteModeEnable == false)
            {
                deleteModeEnable = true;
            }
            else if (deleteModeEnable)
            {
                deleteModeEnable = false;

            }
            */
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

        if (spawnedBuilging && Input.GetKeyDown(KeyCode.Escape))
        {
            DeleteObjectInPreview();
        }
        if (spawnedBuilging is null || !IsRayHittingSomething(buildModeLayerMask, out RaycastHit hitInfo))
        {
            return;
        }
       
       
    }
    private void DeleteObjectInPreview()
    {
        if (spawnedBuilging != null)
        {
            Destroy(spawnedBuilging.gameObject);
            spawnedBuilging = null;
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

        if(spawnedBuilging == null)
        {
            return;
        }
        BuildModePreview();
    }

    private void BuildModePreview()
    {
        spawnedBuilging.UpdateMaterail(spawnedBuilging.IsOverLapping ? buildingMatNegative : buildingMatPositive);
        if (Input.GetKeyDown(KeyCode.R))
        {
            spawnedBuilging.transform.Rotate(0, 90, 0);
            placedBuildingsLastRotation = spawnedBuilging.transform.rotation;
        }
        if (IsRayHittingSomething(buildModeLayerMask, out RaycastHit hitInfo))
        {
            spawnedBuilging.transform.position = hitInfo.point;
            var gridPosition = WorldGrid.GridPositionFromWorld(hitInfo.point, 1f);

            if (Input.GetKeyDown(KeyCode.Mouse0) && !spawnedBuilging.IsOverLapping)
            {
                spawnedBuilging.PlaceBuilding();
                var buildingDataCopy = spawnedBuilging.BuildingData;
                spawnedBuilging = null;
                ChooseBuilding(buildingDataCopy);
            }
        }
    }

    private void DeleteMode()
    {
        if (IsRayHittingSomething(deleteModeLayerMask, out RaycastHit hitInfo))
        {
            var detectedBuiding = hitInfo.collider.gameObject.GetComponentInParent<Building>();

            if (detectedBuiding == null)
            {
                Debug.Log("heyo");
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
