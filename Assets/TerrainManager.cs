using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject groundPlanePrefab;

    private GameObject _ground;

    private float groundMultiple = 10;
    
    private void Start()
    {
        _ground = Instantiate(groundPlanePrefab);
        var groundGridSize = GridSystem.gridSize * 2 / groundMultiple;
        _ground.transform.localScale = Vector3.one * groundGridSize;
        
        var topWallCoords = new Vector3(0, 0.5f, GridSystem.gridSize + 0.5f);
        var topWall = Instantiate(wallPrefab, topWallCoords, Quaternion.identity, transform);
        topWall.transform.localScale = new Vector3(GridSystem.gridSize * 2, 1, 1);
        
        var bottomWallCoords = new Vector3(0, 0.5f, -GridSystem.gridSize - 0.5f);
        var bottomWall = Instantiate(wallPrefab, bottomWallCoords, Quaternion.identity, transform);
        bottomWall.transform.localScale = new Vector3(GridSystem.gridSize * 2, 1, 1);

        var rightWallCoords = new Vector3(GridSystem.gridSize + 0.5f, 0.5f, 0);
        var rightWall = Instantiate(wallPrefab, rightWallCoords, Quaternion.identity, transform);
        rightWall.transform.localScale = new Vector3(1, 1, GridSystem.gridSize * 2);
        
        var leftWallCoords = new Vector3(-GridSystem.gridSize - 0.5f, 0.5f, 0);
        var leftWall = Instantiate(wallPrefab, leftWallCoords, Quaternion.identity, transform);
        leftWall.transform.localScale = new Vector3(1, 1, GridSystem.gridSize * 2);
    }
}
