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

    void Start()
    {
        _direction = MoveDirection.Up;
        GameObject appleSpawnerObject = GameObject.Find("AppleSpawner");
        appleSpawner = appleSpawnerObject.GetComponent<AppleSpawner>();
        // SnakePrefab.
        // Instantiate(SnakePrefab, transform);
        snakeBody.Enqueue(Instantiate(SnakePrefab, GridSystem.TranslateCoordinates(_position), Quaternion.identity));
    }    

    private void FixedUpdate()
    {
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

        var translatedCoords = GridSystem.TranslateCoordinates(_position);

        Collider[] hitColliders = Physics.OverlapBox(translatedCoords, new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity);
        if (hitColliders.Length == 0)
        {
            var lastBodyPart = snakeBody.Dequeue();
            Destroy(lastBodyPart);
            snakeBody.Enqueue(Instantiate(SnakePrefab, translatedCoords, Quaternion.identity));
        }
        if (hitColliders.Length == 1)
        {
            if (hitColliders[0].CompareTag("Apple"))
            {
                snakeBody.Enqueue(Instantiate(SnakePrefab, translatedCoords, Quaternion.identity));
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
                _direction = MoveDirection.Up;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_direction != MoveDirection.Up)
            {
                _direction = MoveDirection.Down;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_direction != MoveDirection.Right)
            {
                _direction = MoveDirection.Left;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_direction != MoveDirection.Left)
            {
                _direction = MoveDirection.Right;
            }
        }
    }

}
