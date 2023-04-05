using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Renderer _renderer;
    private Material _defaultMaterail;
    private bool flaggedforDelete;
    public bool FlaggedForDelete => flaggedforDelete;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer)
        {
            _defaultMaterail = _renderer.material;
        }
    }

    public void UpdateMaterail(Material newMaterail)
    {
        if(_renderer.material != newMaterail)
        {
            _renderer.material = newMaterail;
        }
    }
    public void PlaceBuilding()
    {
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer)
        {
            _defaultMaterail = _renderer.material;
        }
        UpdateMaterail(_defaultMaterail);
    }
    public void FlagForDelete(Material deleteMat)
    {
        UpdateMaterail(deleteMat);
        flaggedforDelete = true;
    }
    public void RemoveFlag()
    {
        UpdateMaterail(_defaultMaterail);
        flaggedforDelete = false;
    }
}
