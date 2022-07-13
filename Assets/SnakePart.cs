using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public Material snakeHeadMat;
    public Material snakeBodyMat;
    public Material snakeTailMat;

    public MoveDirection moveDirection;

    private SnakePartType partType;
    private SnakePart previousPart;
    private SnakePart nextPart;

    private Vector3 initialPosition;
    private Vector3 xAxis = new Vector3(1, 0, 0);
    private Vector3 zAxis = new Vector3(0, 0, 1);

    public void SetIsHead(MoveDirection direction)
    {
        partType = SnakePartType.Head;
        GetComponent<Renderer>().material = snakeHeadMat;
        moveDirection = direction;
        initialPosition = transform.position;
        SetNextPart(null);
        // reset the origin, and flatting the scale initially before growing along the scale
        switch (moveDirection)
        {
            case MoveDirection.Left:
                transform.localScale = new Vector3(0, 1, 1);
                transform.position += Vector3.right * 0.5f;
                break;
            case MoveDirection.Right:
                transform.localScale = new Vector3(0, 1, 1);
                transform.position += Vector3.left * 0.5f;
                break;
            case MoveDirection.Up:
                transform.localScale = new Vector3(1, 1, 0);
                transform.position += Vector3.back * 0.5f;
                break;
            case MoveDirection.Down:
                transform.localScale = new Vector3(1, 1, 0);
                transform.position += Vector3.forward * 0.5f;
                break;
        }
    }

    public void SetIsBody()
    {
        partType = SnakePartType.Body;
        GetComponent<Renderer>().material = snakeBodyMat;
        transform.localScale = Vector3.one;
        transform.position = initialPosition;
    }

    public void SetIsTail()
    {
        partType = SnakePartType.Tail;
        GetComponent<Renderer>().material = snakeTailMat;
        SetPreviousPart(null);
    }

    private void Update()
    {
        var delta = Time.deltaTime;

        if (partType != SnakePartType.Body)
        {
            var ratio = Mathf.Clamp01(delta / Time.fixedDeltaTime);
            // grow in scale towards the moveDirection if we're not a 'corner tail'
            // otherwise if a corner tail, then 'shrink' according to the part in front of us
            var direction = (partType == SnakePartType.Tail) ? nextPart.moveDirection : moveDirection;
            switch (direction)
            {
                case MoveDirection.Left:
                    transform.position += Vector3.left * ratio / 2;
                    ScaleXAxis(ratio);
                    break;
                case MoveDirection.Right:
                    transform.position += Vector3.right * ratio / 2;
                    ScaleXAxis(ratio);
                    break;
                case MoveDirection.Up:
                    transform.position += Vector3.forward * ratio / 2;
                    ScaleZAxis(ratio);
                    break;
                case MoveDirection.Down:
                    transform.position += Vector3.back * ratio / 2;
                    ScaleZAxis(ratio);
                    break;
            }
        }
    }

    private void ScaleXAxis(float ratio)
    {
        if (partType == SnakePartType.Head)
        {
            transform.localScale += xAxis * ratio;
        }
        else if (partType == SnakePartType.Tail)
        {
            transform.localScale -= xAxis * ratio;
        }
    }

    private void ScaleZAxis(float ratio)
    {
        if (partType == SnakePartType.Head)
        {
            transform.localScale += zAxis * ratio;
        }
        else if (partType == SnakePartType.Tail)
        {
            transform.localScale -= zAxis * ratio;
        }
    }

    public void SetPreviousPart(SnakePart part)
    {
        previousPart = part;
    }

    public void SetNextPart(SnakePart part)
    {
        nextPart = part;
    }
}
