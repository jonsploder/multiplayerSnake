using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public Material snakeHeadMat;
    public Material snakeBodyMat;

    bool isHead = false;

    public void SetIsHead(bool head)
    {
        isHead = head;
        if (isHead)
        {
            GetComponent<Renderer>().material = snakeHeadMat;
        }
        else
        {
            GetComponent<Renderer>().material = snakeBodyMat;
        }
    }
}
