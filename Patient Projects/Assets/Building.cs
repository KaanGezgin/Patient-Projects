using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Renderer renderer;
    private Material defaultMaterail;
    private bool flaggedforDelete;
    public bool FlaggedForDelete => flaggedforDelete;

    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
        if (renderer)
        {
            defaultMaterail = renderer.material;
        }
    }

    public void UpdateMaterail(Material newMaterail)
    {
        if(renderer.material != newMaterail)
        {
            renderer.material = newMaterail;
        }
    }
    public void PlaceBuilding()
    {
        renderer = GetComponentInChildren<Renderer>();
        if (renderer)
        {
            defaultMaterail = renderer.material;
        }
        UpdateMaterail(defaultMaterail);
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
}
