using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


[CreateAssetMenu(menuName = "Building Datas")]
public class BuildingsData : ScriptableObject
{
    public string DisplayName;
    public Color color;
    public float gridSnapSize;
    public GameObject buildingPrefab;
    public Vector3 BuildingsSize;
    public BuildingsType buildingType;
}

public enum BuildingsType
{
    HealthStation = 0,
    TeslaCoil = 1,
    UpgradeAndCraftingBench = 2
}

#if UNITY_EDITOR
[CustomEditor(typeof(BuildingsData))]

public class BuildingsDataEditor: Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (BuildingsData)target;
        if(data == null)
        {
            return null;
        }
        var texture = new Texture2D(width, height);
        //EditorUtility.CopySerialized(data.icon.texture, texture);
        return texture;
    }
}

#endif
