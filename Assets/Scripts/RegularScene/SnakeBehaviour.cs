using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeBehaviour : MonoBehaviour
{
    Vector2 moveDirection = Vector2.up;
    [SerializeField] float cellSize = .3f;
    public float CellSize => cellSize;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] PowerUpSpawner powerUpSpawner;
    [SerializeField] Transform bodyPrefab;

    List<Transform> body = new List<Transform>();
    float currentMoveTime = 0;
    Vector2 snakeIndex;

    Vector2 lastInput = new Vector2(0,1);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        MoveOneUnit();
    }

    void MoveOneUnit()
    {
        if (Time.time > currentMoveTime)
        {

            for (int i = body.Count-1; i >0; i--)
            {
                body[i].position = body[i - 1].position;
            }

            if (body.Count > 0)
                body[0].position = (Vector2)transform.position;


            transform.position += (Vector3)moveDirection * cellSize;
            currentMoveTime = Time.time + 1 / moveSpeed;
            snakeIndex = transform.position / cellSize;
        }
    }

    void ChangeDirection()
    {
        Vector2 _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_input.y == -1 && lastInput.y != 1)
        {
            moveDirection = Vector2.down;
            lastInput = _input;
            return;
        }
        if (_input.y == 1 && lastInput.y != -1)
        {
            moveDirection = Vector2.up;
            lastInput = _input;
            return;
        }
        if (_input.x == -1 && lastInput.x != 1)
        {
            moveDirection = Vector2.left;
            lastInput = _input;
            return;
        }
        if (_input.x == 1 && lastInput.x != -1)
        {
            moveDirection = Vector2.right;
            lastInput = _input;
            return;
        }

    }

    void GrowBody()
    {
        Vector2 _position = transform.position;
        if (body.Count != 0)
        {
            _position = body[body.Count - 1].position;
        }
        body.Add(Instantiate(bodyPrefab, _position, Quaternion.identity).transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            Transform _currentPowerUp = collision.transform;
            
            for (int i = 0; i < powerUpSpawner.PowerUpList.Count; i++)
            {
                if (_currentPowerUp.GetInstanceID() == powerUpSpawner.PowerUpList[i].GetInstanceID())
                {
                    powerUpSpawner.PowerUpList.Remove(powerUpSpawner.PowerUpList[i]);
                    GrowBody();
                    powerUpSpawner.SpawnPowerUp();
                    break;
                }
            }

            Destroy(_currentPowerUp.gameObject);
        }

        if (collision.CompareTag("Wall"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.CompareTag("Body") && body.Count > 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
