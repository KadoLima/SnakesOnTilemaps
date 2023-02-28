using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] Transform powerUpPrefab;
    List<Transform> powerUpList = new List<Transform>();
    public List<Transform> PowerUpList => powerUpList;
    [SerializeField] float startPowerUpCount;
    [SerializeField] SnakeBehaviour snake;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < startPowerUpCount; i++)
        {
            SpawnPowerUp();
        }
    }

    public void SpawnPowerUp()
    {
        float _x = Random.Range(-27, 27) * snake.CellSize;
        float _y = Random.Range(-13, 13) * snake.CellSize;
        Vector2 _randomPos = new Vector2(_x, _y);
        Transform _g = Instantiate(powerUpPrefab, _randomPos, Quaternion.identity).transform;
        powerUpList.Add(_g);


    }
}
