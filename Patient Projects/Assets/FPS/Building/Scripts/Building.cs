using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : MonoBehaviour
{
    private BuildingsData buildingData;
    private BoxCollider buildingCollider;
    private GameObject buildingGraphic;
    private Collider colliders;
    private bool isOverlapping;
    
    private Renderer buildingMaterialRenderer;
    private Material defaultMaterail;
    private bool flaggedforDelete;
    public bool FlaggedForDelete => flaggedforDelete;
    public bool IsOverLapping => isOverlapping;
    public BuildingsData BuildingData => buildingData;


    public void Init(BuildingsData data)
    {
        buildingData = data;
        buildingCollider = GetComponent<BoxCollider>();
        buildingCollider.size = buildingData.BuildingsSize;
        buildingCollider.center = new Vector3(0, (buildingData.BuildingsSize.y + 2.0f) * 0.5f, 0);
        buildingCollider.isTrigger = true;

        var buildingRigidbody = gameObject.AddComponent<Rigidbody>();
        buildingRigidbody.isKinematic = true;

        buildingGraphic = Instantiate(data.buildingPrefab, transform);
        buildingMaterialRenderer = GetComponentInChildren<Renderer>();
        defaultMaterail = buildingMaterialRenderer.material;

        colliders = GetComponentInChildren<BoxCollider>();
        if(colliders != null)
        {
            colliders.gameObject.SetActive(true);
        }
    }
    public void UpdateMaterail(Material newMaterail)
    {
        if(buildingMaterialRenderer.material != newMaterail)
        {
            buildingMaterialRenderer.material = newMaterail;
        }
    }
    public void PlaceBuilding()
    {
        buildingCollider.enabled = false;
        if(colliders != null)
        {
            colliders.gameObject.SetActive(true);
        }
        UpdateMaterail(defaultMaterail);
        gameObject.layer = 10;// BURASI PROJEDEN PROJEYE DEÐÝÞÝYOR "Buildings" Layerý oluþturup baþýndaki numara girilecek
        gameObject.name = buildingData.DisplayName + " - " + transform.position;
    }
    public void FlagForDelete(Material deleteMat)
    {
        UpdateMaterail(deleteMat);
        flaggedforDelete = true;
    }
    public void RemoveFlag()
    {
        UpdateMaterail(defaultMaterail);
        flaggedforDelete = false;
    }
    private void OnTriggerStay(Collider other)
    {
        isOverlapping = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isOverlapping = false;
    }
}
