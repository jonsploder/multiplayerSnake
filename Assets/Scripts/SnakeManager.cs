using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeManager : MonoBehaviour
{
    public GameObject SnakePrefab;

    enum MoveDirection
    {
        Up,
        Right,
        Down,
        Left,
    }

    private MoveDirection _direction;
    private Vector2 _position;
    public AppleSpawner appleSpawner;
    public Queue<GameObject> snakeBody = new Queue<GameObject>();
    KeyCode lastValidKeyPress;
    SnakePart currentHead;

    void Start()
    {
        _direction = MoveDirection.Up;
        GameObject appleSpawnerObject = GameObject.Find("AppleSpawner");
        appleSpawner = appleSpawnerObject.GetComponent<AppleSpawner>();
        UpdateHead(GridSystem.TranslateCoordinates(_position));
    }

    private void UpdateHead(Vector3 coords)
    {        
        if (snakeBody.Count > 0)
        {
            currentHead.SetIsHead(false);
        }

        var newHead = Instantiate(SnakePrefab, coords, Quaternion.identity);
        var newHeadComponent = newHead.GetComponent<SnakePart>();
        newHeadComponent.SetIsHead(true);
        currentHead = newHeadComponent;
        snakeBody.Enqueue(newHead);
    }

    
    private void HandleNextMove()
    {
        switch (lastValidKeyPress)
        {
            case KeyCode.UpArrow:
                _direction = MoveDirection.Up;
                break;
            case KeyCode.DownArrow:
                _direction = MoveDirection.Down;
                break;
            case KeyCode.RightArrow:
                _direction = MoveDirection.Right;
                break;
            case KeyCode.LeftArrow:
                _direction = MoveDirection.Left;
                break;
                //default:
                //    _direction = MoveDirection.Up;
                //    break;
        }
        if (_direction == MoveDirection.Up)
        {
            _position.y += 1;
        }
        if (_direction == MoveDirection.Down)
        {
            _position.y -= 1;
        }
        if (_direction == MoveDirection.Left)
        {
            _position.x -= 1;
        }
        if (_direction == MoveDirection.Right)
        {
            _position.x += 1;
        }
    }

    private void FixedUpdate()
    {
        HandleNextMove();
        var translatedCoords = GridSystem.TranslateCoordinates(_position);
        if (Mathf.Abs(translatedCoords.x) >= GridSystem.gridSize || Mathf.Abs(translatedCoords.z) >= GridSystem.gridSize)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Collider[] hitColliders = Physics.OverlapBox(translatedCoords, new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity);
        if (hitColliders.Length == 0)
        {
            UpdateHead(translatedCoords);

            var tail = snakeBody.Dequeue();
            Destroy(tail);
        }
        if (hitColliders.Length == 1)
        {
            if (hitColliders[0].CompareTag("Apple"))
            {
                UpdateHead(translatedCoords);
                appleSpawner.SpawnApple();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        if (hitColliders.Length > 1)
        {
            Debug.Log("Unexpectedly large collider list");
        }
    }

    private void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_direction != MoveDirection.Down)
            {
                lastValidKeyPress = KeyCode.UpArrow;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_direction != MoveDirection.Up)
            {
                lastValidKeyPress = KeyCode.DownArrow;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_direction != MoveDirection.Right)
            {
                lastValidKeyPress = KeyCode.LeftArrow;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_direction != MoveDirection.Left)
            {
                lastValidKeyPress = KeyCode.RightArrow;
            }
        }
    }

}
