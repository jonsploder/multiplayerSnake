using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    public static int gridSize = 20;

    public static Vector3 TranslateCoordinates(Vector2 gridCoords)
    {
        return new Vector3(gridCoords.x + 0.5f, 0.5f, gridCoords.y + 0.5f);
    }
}
