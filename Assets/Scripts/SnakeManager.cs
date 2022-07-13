using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeManager : MonoBehaviour
{
    public GameObject SnakePrefab;
    public AppleSpawner appleSpawner;
    public ScoreManager scoreManager;
    public GameObject snakeHeadRef;
    public Queue<GameObject> snakeBody = new Queue<GameObject>();

    private MoveDirection _direction;
    private Vector2 _position;
    KeyCode lastValidKeyPress;
    SnakePart currentHead = null;

    void Start()
    {
        GameObject appleSpawnerObject = GameObject.Find("AppleSpawner");
        appleSpawner = appleSpawnerObject.GetComponent<AppleSpawner>();

        GameObject scoreCounterObject = GameObject.Find("ScoreCounter");
        scoreManager = scoreCounterObject.GetComponent<ScoreManager>();

        Array values = Enum.GetValues(typeof(MoveDirection));
        System.Random random = new System.Random();
        MoveDirection randomDirection = (MoveDirection)values.GetValue(random.Next(values.Length));
        _direction = randomDirection;       

        var initialSize = 5; // including head, but because the tail and the head are expanding, size 3 looks like 2 for example
        var initialBodyDirection = DirectionToVector2(_direction);
        for (int i = initialSize - 1; i > 0; i -= 1)
        {
            UpdateHead(GridSystem.TranslateCoordinates(_position - initialBodyDirection * i));
        }
        UpdateHead(GridSystem.TranslateCoordinates(_position));

        var tail = snakeBody.Peek();
        var tailComponent = tail.GetComponent<SnakePart>();
        tailComponent.SetIsTail();
        snakeHeadRef.transform.position = currentHead.transform.position;
    }

    private Vector2 DirectionToVector2(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Left:
                return Vector2.left;
            case MoveDirection.Right:
                return Vector2.right;
            case MoveDirection.Up:
                return Vector2.up;
            case MoveDirection.Down:                    
                return Vector2.down;
            default:
                throw new Exception("Wrong");
        }
    }

    private void UpdateHead(Vector3 coords)
    {
        var oldHead = currentHead;
        if (snakeBody.Count > 0)
        {
            oldHead.SetIsBody();
        }

        var newHead = Instantiate(SnakePrefab, coords, Quaternion.identity);
        var newHeadComponent = newHead.GetComponent<SnakePart>();
        newHeadComponent.SetIsHead(_direction);
        currentHead = newHeadComponent;
        snakeBody.Enqueue(newHead);

        if (oldHead != null)
        {
            oldHead.SetNextPart(currentHead);
        }
        currentHead.SetPreviousPart(oldHead);
    }

    private void UpdateTail()
    {
        var tail = snakeBody.Dequeue();
        Destroy(tail);

        var newTail = snakeBody.Peek();
        var newTailComponent = newTail.GetComponent<SnakePart>();
        newTailComponent.SetIsTail();
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
            UpdateTail();
        }
        if (hitColliders.Length == 1)
        {
            if (hitColliders[0].CompareTag("Apple"))
            {
                UpdateHead(translatedCoords);
                scoreManager.incrementScore();
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
        float delta = Time.deltaTime;
        var currentPosition = snakeHeadRef.transform.position;
        var nextPosition = currentHead.transform.position;
        snakeHeadRef.transform.position = Vector3.Lerp(currentPosition, nextPosition, Mathf.Clamp01(delta / Time.fixedDeltaTime));

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
