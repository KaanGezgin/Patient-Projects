using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManagment : MonoBehaviour
{
    private Renderer materialControl;
    private Material buildingsMaterial;
    private Material destroyMaterial;


    private bool destroyFlag;

    private void Start()
    {
        materialControl = GetComponent<Renderer>();
        if (materialControl)
        {
            buildingsMaterial = materialControl.material;
        }
    }

    public void UpdateMaterial(Material replacedMaterial)
    {
        if(materialControl.material != replacedMaterial)
        {
            materialControl.material = replacedMaterial;
        }
    }
    public void PlaceBuildings()
    {
        UpdateMaterial(buildingsMaterial);
    }
    public void FlagForDeletingBuilding(Material desMaterial)
    {
        UpdateMaterial(desMaterial);
        destroyFlag = true;
    }
    public void RemoveDeletFlag()
    {
        UpdateMaterial(buildingsMaterial);
        destroyFlag = false;
    }
}
