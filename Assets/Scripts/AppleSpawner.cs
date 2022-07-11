using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    public GameObject applePrefab;
    private GameObject apple;
    Vector2 currentApplePosition;

    private void Start()
    {
        SpawnApple();
    }

    public void SpawnApple()
    {
        if (apple != null)
        {
            Destroy(apple);
        }

        currentApplePosition = GetApplePosition();
        apple = Instantiate(applePrefab, GridSystem.TranslateCoordinates(currentApplePosition), Quaternion.identity);
    }

    private Vector2 GetApplePosition()
    {
        // TODO handle case when all space is consumed
        bool foundEmptySpace = false;
        var coords = new Vector2();

        while (!foundEmptySpace)
        {
            var x = Random.Range(-10, 10);
            var y = Random.Range(-10, 10);
            coords = new Vector2(x, y);

            if (!Physics.CheckBox(GridSystem.TranslateCoordinates(coords), new Vector3(0.1f, 0.1f, 0.1f)))
            {
                foundEmptySpace = true;
            }
            else
            {
                Debug.Log($"Collision detected and can't spawn apple at: ({x}, {y})");
            }
        }

        return coords;
    }
}
