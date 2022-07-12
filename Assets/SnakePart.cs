using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public Material snakeHeadMat;
    public Material snakeBodyMat;

    bool isHead = false;
    bool animating = false;
    MoveDirection moveDirection;
    private Vector3 initialPosition;

    public void SetIsHead(bool head, MoveDirection direction)
    {
        isHead = head;

        if (isHead)
        {
            GetComponent<Renderer>().material = snakeHeadMat;
            animating = true;
            moveDirection = direction;
            initialPosition = transform.position;
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
            Debug.Log("Created");
        }
        else
        {
            GetComponent<Renderer>().material = snakeBodyMat;
            animating = false;
            transform.localScale = Vector3.one;                
            transform.position = initialPosition;
        }
    }

    private void Update()
    {
        var delta = Time.deltaTime;
        if (animating)
        {
            var ratio = delta / Time.fixedDeltaTime;
            // grow in scale towards the moveDirection
            switch (moveDirection)
            {
                case MoveDirection.Left:
                    transform.position += Vector3.left * ratio / 2;
                    transform.localScale += Vector3.left * ratio; 
                    break;
                case MoveDirection.Right:
                    transform.position += Vector3.right * ratio / 2;
                    transform.localScale += Vector3.right * ratio;
                    break;
                case MoveDirection.Up:
                    transform.position += Vector3.forward * ratio / 2;
                    transform.localScale += Vector3.forward * ratio;
                    break;
                case MoveDirection.Down:
                    transform.position += Vector3.back * ratio / 2;
                    transform.localScale += Vector3.back * ratio;
                    break;
            }
            Debug.Log("Scale: " + transform.localScale);
        }
    }
}
