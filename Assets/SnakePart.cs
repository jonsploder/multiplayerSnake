using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public Material snakeHeadMat;
    public Material snakeBodyMat;

    bool isHead = false;
    AnimateState animateState = AnimateState.None;
    MoveDirection moveDirection;

    private SnakePart previousPart;
    private SnakePart nextPart;

    private Vector3 initialPosition;
    private Vector3 xAxis = new Vector3(1, 0, 0);
    private Vector3 zAxis = new Vector3(0, 0, 1);

    public void SetIsHead(MoveDirection direction)
    {
        isHead = true;
        GetComponent<Renderer>().material = snakeHeadMat;
        animateState = AnimateState.Head;
        moveDirection = direction;
        initialPosition = transform.position;
        // SetNextPart(null);
        // reset the origin, and flatting the scale initially
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
        isHead = false;
        GetComponent<Renderer>().material = snakeBodyMat;
        animateState = AnimateState.None;
        transform.localScale = Vector3.one;
        transform.position = initialPosition;
    }

    public void SetIsTail()
    {
        animateState = AnimateState.Tail;
        isHead = false;
        // SetPreviousPart(null);
    }

    private void Update()
    {
        var delta = Time.deltaTime;

        if (animateState != AnimateState.None)
        {
            var ratio = delta / Time.fixedDeltaTime;
            // grow in scale towards the moveDirection
            switch (moveDirection)
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
        if (animateState == AnimateState.Head)
        {
            transform.localScale += xAxis * ratio;
        }
        else if (animateState == AnimateState.Tail)
        {
            transform.localScale -= xAxis * ratio;
        }
    }

    private void ScaleZAxis(float ratio)
    {
        if (animateState == AnimateState.Head)
        {
            transform.localScale += zAxis * ratio;
        }
        else if (animateState == AnimateState.Tail)
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
