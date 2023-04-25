using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGrid
{
    public static Vector3 GridPositionFromWorld(Vector3 worldPosition, float gridScale)
    {
        var x = Mathf.Round(worldPosition.x / gridScale) * gridScale;
        var y = Mathf.Round(worldPosition.y / gridScale) * gridScale;
        var z = Mathf.Round(worldPosition.z / gridScale) * gridScale;

        return new Vector3(x, y, z);
    }
}
