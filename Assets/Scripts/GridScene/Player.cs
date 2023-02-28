using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using DG.Tweening;
public enum CurrentDirection
{
    RIGHT,
    LEFT,
    UP,
    DOWN
}

public class Player : MonoBehaviour
{
    [SerializeField] CurrentDirection currentDirection;

    [SerializeField] float moveSpeed = 5f;

    [SerializeField] Transform bodyPrefab;

    [SerializeField] List<Transform> body = new List<Transform>();
    public List<Transform> Body => body;
    float currentMoveTime = 0;
    int movedTilesInCurrentDirection = 0;

    Vector3 originalScale;

    public bool isReady;
    public bool IsReady => isReady;


    // Start is called before the first frame update
    void Start()
    {
        SetInitialPosition(TilemapsManager.instance.Scenario.WorldToCell(transform.position));

        //TilemapsManager.instance.ChangeTile(TilemapsManager.instance.ScenarioTilemap.WorldToCell(transform.position));
        //TilemapsManager.instance.PaintTile(TilemapsManager.instance.ScenarioTilemap.WorldToCell(transform.position));

        

        StartCoroutine(AppearingEffect());
    }



    private void Update()
    {
        if (GameController.instance.IsPlayable())
        {
            ReadUserInput();
            MoveToAnotherTile();
        }

    }

    IEnumerator AppearingEffect()
    {
        originalScale = transform.localScale;
        transform.DOScale(0, 0);

        yield return new WaitUntil(() => GameController.instance.wallTilesEffector.IsDone);

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .1f));
        sequence.AppendInterval(0.025f);
        sequence.Append(transform.DOScale(originalScale, .1f));

        yield return sequence.WaitForCompletion();

        yield return new WaitForSeconds(0.25f);

        isReady = true;
    }

    public void ReadUserInput()
    {
        Dictionary<KeyCode, CurrentDirection> keyToDirection = new Dictionary<KeyCode, CurrentDirection>
    {
        { KeyCode.S, CurrentDirection.DOWN },
        { KeyCode.DownArrow, CurrentDirection.DOWN },
        { KeyCode.W, CurrentDirection.UP },
        { KeyCode.UpArrow, CurrentDirection.UP },
        { KeyCode.A, CurrentDirection.LEFT },
        { KeyCode.LeftArrow, CurrentDirection.LEFT },
        { KeyCode.D, CurrentDirection.RIGHT },
        { KeyCode.RightArrow, CurrentDirection.RIGHT }
    };

        foreach (var k in keyToDirection)
        {
            if (Input.GetKeyDown(k.Key))
            {
                if (currentDirection != OppositeDirection(k.Value) && movedTilesInCurrentDirection > 0)
                {
                    currentDirection = k.Value;
                    movedTilesInCurrentDirection = 0;
                }
                return;
            }
        }
    }

    private CurrentDirection OppositeDirection(CurrentDirection direction)
    {
        switch (direction)
        {
            case CurrentDirection.UP: return CurrentDirection.DOWN;
            case CurrentDirection.DOWN: return CurrentDirection.UP;
            case CurrentDirection.LEFT: return CurrentDirection.RIGHT;
            case CurrentDirection.RIGHT: return CurrentDirection.LEFT;
            default: return CurrentDirection.RIGHT;
        }
    }

    //public void ReadUserInput()
    //{

    //    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        if (currentDirection != CurrentDirection.UP && movedTilesInCurrentDirection>0)
    //        {
    //            currentDirection = CurrentDirection.DOWN;
    //            movedTilesInCurrentDirection = 0;
    //            return;
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        if (currentDirection != CurrentDirection.DOWN && movedTilesInCurrentDirection > 0)
    //        {
    //            currentDirection = CurrentDirection.UP;
    //            movedTilesInCurrentDirection = 0;
    //            return;
    //        }

    //    }
    //    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
    //        {
    //        if (currentDirection != CurrentDirection.RIGHT && movedTilesInCurrentDirection > 0)
    //        {
    //            currentDirection = CurrentDirection.LEFT;
    //            movedTilesInCurrentDirection = 0;
    //            return;
    //        }

    //        }
    //    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        if (currentDirection != CurrentDirection.LEFT && movedTilesInCurrentDirection > 0)
    //        {
    //            currentDirection = CurrentDirection.RIGHT;
    //            movedTilesInCurrentDirection = 0;
    //            return;
    //        }
    //    }
    //}

    private void SetInitialPosition(Vector3Int pos) //Snaps the player gameObject in the center of the nearest cell, when game starts.
    {
        Vector3Int _cellPosition = pos;
        transform.position = TilemapsManager.instance.Scenario.GetCellCenterWorld(_cellPosition);
    }

    void MoveToAnotherTile() //Move to the next cell.
    {
        if (Time.time > currentMoveTime)
        {
            Vector3Int _currentPosition = TilemapsManager.instance.Scenario.WorldToCell(transform.position);
            Vector3Int _nextPosition = Vector3Int.zero;

            _nextPosition = SetDirection(_currentPosition, _nextPosition);

            for (int i = body.Count - 1; i > 0; i--)
            {
                body[i].position = body[i - 1].position;
            }

            if (body.Count > 0)
                body[0].position = (Vector2)transform.position;

            transform.position = TilemapsManager.instance.Scenario.GetCellCenterWorld(_nextPosition);

            currentMoveTime = Time.time + 1 / moveSpeed;
            movedTilesInCurrentDirection++;
        }
    }

    private Vector3Int SetDirection(Vector3Int _currentPosition, Vector3Int _nextPosition)
    {

        switch (currentDirection)
        {
            case CurrentDirection.RIGHT:
                return _currentPosition + new Vector3Int((int)TilemapsManager.instance.CellSizeX(), 0);
            case CurrentDirection.LEFT:
                return _currentPosition - new Vector3Int((int)TilemapsManager.instance.CellSizeX(), 0);
            case CurrentDirection.UP:
                return _currentPosition + new Vector3Int(0, (int)TilemapsManager.instance.CellSizeY());
            case CurrentDirection.DOWN:
                return _currentPosition - new Vector3Int(0, (int)TilemapsManager.instance.CellSizeY());
            default:
                return _currentPosition + new Vector3Int((int)TilemapsManager.instance.CellSizeX(), 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            GrowBody();
            TilemapsManager.instance.powerUpCreator.PlayEffectAt(collision.transform.position);
            collision.gameObject.GetComponent<PowerUpEffector>().CameraShake();
            Debug.LogWarning("I ate a powerup!");
            Destroy(collision.gameObject);
            TilemapsManager.instance.powerUpCreator.CreatePowerUpAtRandomPosition();
            return;
        }

        if (collision.CompareTag("Wall"))
        {
            Debug.LogWarning("I hit a wall!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (collision.CompareTag("Body") && body.Count > 1)
        {
            Debug.LogWarning("I hit myself!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
    }

    void GrowBody()
    {
        Vector3Int _pos = TilemapsManager.instance.Scenario.WorldToCell(transform.position);

        if (body.Count != 0)
        {
            Vector3Int _oldPos = Vector3Int.FloorToInt(body[body.Count - 1].position);
            _pos = TilemapsManager.instance.Scenario.WorldToCell(_oldPos);
        }

        body.Add(Instantiate(bodyPrefab, TilemapsManager.instance.Scenario.GetCellCenterWorld(_pos), Quaternion.identity).transform);

    }
}
