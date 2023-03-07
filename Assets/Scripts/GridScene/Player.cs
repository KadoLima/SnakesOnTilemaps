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
    [SerializeField] Transform headSprite;
    [SerializeField] GameObject headLight;
    [SerializeField] CurrentDirection currentDirection;

    [SerializeField] float moveSpeed = 5f;

    [SerializeField] Transform bodyPrefab;

    [SerializeField] List<Transform> body = new List<Transform>();
    public List<Transform> Body => body;
    float nextMove = 0;
    int movedTilesInCurrentDirection = 0;

    Vector3 originalScale;

    bool isReady;
    public bool IsReady => isReady;

    bool isDead;

    int collectedCount = 0;
    public int CollectedCount => collectedCount;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AppearingEffect());
    }



    private void Update()
    {
        if (GameController.instance.IsPlayable() && !isDead && !PauseScreen.isPaused && !VictoryScreen.beatGame)
        {
            ReadUserInput();
            MoveToAnotherTile();
        }

    }

    IEnumerator AppearingEffect()
    {
        headLight.SetActive(false);
        originalScale = transform.localScale;
        transform.DOScale(0, 0);

        yield return new WaitUntil(() => GameEffects.instance.IntroDone);

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(originalScale.x + 2f, originalScale.y + 2f, originalScale.z + 2f), .2f));
        sequence.AppendInterval(0.025f);
        sequence.Append(transform.DOScale(originalScale, .2f));

        yield return sequence.WaitForCompletion();

        yield return new WaitForSeconds(0.25f);

        isReady = true;
        GameController.isFirstOpen = false;
        headLight.SetActive(true);
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

    void MoveToAnotherTile() //Move to the next cell.
    {
        if (Time.time > nextMove)
        {
            Vector3Int _currentPosition = TilemapsManager.instance.Scenario.WorldToCell(transform.position);

            Vector3Int _nextPosition = SetDirection(_currentPosition);

            for (int i = body.Count - 1; i > 0; i--)
            {
                body[i].position = body[i - 1].position;
            }

            if (body.Count > 0)
                body[0].position = (Vector2)transform.position;

            transform.position = TilemapsManager.instance.Scenario.GetCellCenterWorld(_nextPosition);

            nextMove = Time.time + 1 / moveSpeed;
            movedTilesInCurrentDirection++;
        }
    }

    private Vector3Int SetDirection(Vector3Int _currentPosition)
    {

        switch (currentDirection)
        {
            case CurrentDirection.RIGHT:
                headSprite.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                return _currentPosition + new Vector3Int((int)TilemapsManager.instance.CellSizeX(), 0);
            case CurrentDirection.LEFT:
                headSprite.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                return _currentPosition - new Vector3Int((int)TilemapsManager.instance.CellSizeX(), 0);
            case CurrentDirection.UP:
                headSprite.rotation = Quaternion.Euler(Vector3.zero);
                return _currentPosition + new Vector3Int(0, (int)TilemapsManager.instance.CellSizeY());
            case CurrentDirection.DOWN:
                headSprite.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
                return _currentPosition - new Vector3Int(0, (int)TilemapsManager.instance.CellSizeY());
            default:
                headSprite.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                return _currentPosition + new Vector3Int((int)TilemapsManager.instance.CellSizeX(), 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            GrowBody();

            if (TilemapsManager.instance.spawnableCreator.IsPsychoFruit())
                GameEffects.instance.PlayPsychoEffect();

            Destroy(collision.gameObject);
            TilemapsManager.instance.ClearBusyTileAt(TilemapsManager.instance.Scenario.WorldToCell(transform.position));
            TilemapsManager.instance.spawnableCreator.CreatePowerUpAtRandomPosition();
            collectedCount++;
            CheckIfBeatGame();
            return;
        }

        if (collision.CompareTag("Wall"))
        {
            Debug.LogWarning("I hit a WALL! " + collision.gameObject.name);
            isDead = true;
            StartCoroutine(ReloadScene_Coroutine());
        }

        if (collision.CompareTag("Body") && body.Count > 1)
        {
            Debug.LogWarning("I hit MYSELF!! " + collision.gameObject.name);
            isDead = true;
            StartCoroutine(ReloadScene_Coroutine());
        }

        if (collision.CompareTag("Bomb"))
        {
            isDead = true;
            collision.GetComponent<SpriteRenderer>().sortingOrder = this.transform.GetComponentInChildren<SpriteRenderer>().sortingOrder + 1;
            collision.GetComponent<Animator>().SetTrigger("explode");
            StartCoroutine(ReloadScene_Coroutine());
        }
    }

    IEnumerator ReloadScene_Coroutine(float delay=0)
    {
        yield return new WaitForSeconds(delay);
        GameEffects.instance.CameraShake();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GrowBody()
    {
        Vector3Int headPos = TilemapsManager.instance.Scenario.WorldToCell(transform.position);
        Vector3Int tailPos = (body.Count > 0) ? Vector3Int.FloorToInt(body[body.Count - 1].position) : headPos;

        Vector3Int diff = headPos - tailPos;
        Vector3Int newPos = tailPos + diff;

        Transform _instiatedBody = Instantiate(bodyPrefab, TilemapsManager.instance.Scenario.GetCellCenterWorld(newPos), Quaternion.identity).transform;

        body.Add(_instiatedBody);

        StartCoroutine(EnableBodyCollider_Delayed(_instiatedBody.GetComponent<Collider2D>()));
    }

    IEnumerator EnableBodyCollider_Delayed(Collider2D coll)
    {
        coll.enabled = false;
        yield return new WaitForSeconds(0.25f);
        coll.enabled = true;
    }

    public void CheckIfBeatGame()
    {
        if (collectedCount >= GameController.instance.FruitsToBeatGame)
        {
            GetComponent<Collider2D>().enabled = false;
            GameController.instance.victoryScreen.ShowVictoryScreen();
        }
    }
}
