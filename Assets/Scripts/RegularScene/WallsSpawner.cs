using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsSpawner : MonoBehaviour
{
    [SerializeField] Transform wallPrefab;
    [SerializeField] List<Transform> walls;
    [SerializeField] SnakeBehaviour snake;


    // Start is called before the first frame update
    void Start()
    {
        CreateWalls();   
    }

    void CreateWalls()
    {
        int _cellX = -29;
        int _cellY = 16;
        int _height = 32;

        float _horizontal = _cellX * snake.CellSize;
        float _vertical = _cellY * snake.CellSize;

        for (int i = 0; i < (int)Mathf.Abs((_horizontal*2)/ snake.CellSize)+1; i++)
        {
            Vector2 _top = new Vector2(_horizontal + snake.CellSize * i, _vertical);
            Vector2 _bottom = new Vector2(_horizontal + snake.CellSize * i, _vertical - _height * snake.CellSize);
            walls.Add(Instantiate(wallPrefab, _top, Quaternion.identity).transform);
            walls.Add(Instantiate(wallPrefab, _bottom, Quaternion.identity).transform);
        }

        for (int i = 0; i < _height; i++)
        {
            Vector2 _right = new Vector2(_horizontal, _vertical - snake.CellSize * i);
            Vector2 _left = new Vector2(-_horizontal, _vertical - snake.CellSize * i);
            walls.Add(Instantiate(wallPrefab, _right, Quaternion.identity).transform);
            walls.Add(Instantiate(wallPrefab, _left, Quaternion.identity).transform);
        }

        foreach (var w in walls)
        {
            w.SetParent(this.transform);
        }
    }
}
