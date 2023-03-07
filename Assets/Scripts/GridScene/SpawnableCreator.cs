using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnableCreator : MonoBehaviour
{
    [SerializeField] Tilemap scenario;
    //[SerializeField] Tilemap busyAreaTilemap;
    [Header("Prefabs")]
    [SerializeField] Transform[] powerUpPrefabs;
    [SerializeField] Transform landMinePrefab;
    //[SerializeField] TileBase busyTile;
    [SerializeField] Player player;
    [Header("Psycho Fruit Rules")]
    [SerializeField] float psychoFruitDropRate = 0.1f;
    [Header("Landmine Rules")]
    [SerializeField] float chanceToSpawnMine = 0.5f;
    [SerializeField] float minDistanceToSpawnLandmine = 5f;
    int spawnedCount = 0;

    bool currentFruitIsPsycho = false;

    public void CreatePowerUpAtRandomPosition()
    {
        StartCoroutine(CreatePowerUpAtRandomPosition_Coroutine());

    }

    IEnumerator CreatePowerUpAtRandomPosition_Coroutine()
    {
        yield return new WaitUntil(() => GameController.instance.IsPlayable());

        Transform _powerUp = null;

        if (Random.value < psychoFruitDropRate && spawnedCount >3)
        {
            currentFruitIsPsycho = true;
            _powerUp = Instantiate(powerUpPrefabs[powerUpPrefabs.Length-1]);
        }
        else
        {
            currentFruitIsPsycho = false;
            _powerUp = Instantiate(powerUpPrefabs[Random.Range(0,powerUpPrefabs.Length-1)]);
        }

        Vector3Int _randomPos = TilemapsManager.instance.GetRandomTilePosition();

        while (IsBodyInTile(_randomPos) || IsOverBusyArea(_randomPos))
        {
            _randomPos = TilemapsManager.instance.GetRandomTilePosition();
        }


        TilemapsManager.instance.SetBusyTileAt(_randomPos);
        _powerUp.position = TilemapsManager.instance.Scenario.GetCellCenterWorld(_randomPos);
        _powerUp.SetParent(transform);
        spawnedCount++;

        CreateLandMineAtRandomPos();
    }

    void CreateLandMineAtRandomPos()
    {
        if (GameController.instance.player.CollectedCount < 3)
            return;

        if (Random.value > chanceToSpawnMine)
            return;

        Transform _landMine = Instantiate(landMinePrefab);
        Vector3Int _randomPos = TilemapsManager.instance.GetRandomTilePosition_ToSpawnLandmine();

        while (IsBodyInTile(_randomPos) || IsOverBusyArea(_randomPos))
        {
            _randomPos = TilemapsManager.instance.GetRandomTilePosition_ToSpawnLandmine();
        }

        TilemapsManager.instance.SetBusyTileAt(_randomPos);
        _landMine.position = TilemapsManager.instance.Scenario.GetCellCenterWorld(_randomPos);
        _landMine.SetParent(transform);
    }

    bool IsBodyInTile(Vector3Int tilePosition)
    {
        for (int i = 0; i < player.Body.Count; i++)
        {
            Vector3Int _bodyPos = Vector3Int.FloorToInt(player.Body[i].position);

            if (_bodyPos == tilePosition)
                return true;
        }

        return false;
    }

    public bool IsPsychoFruit()
    {
        return currentFruitIsPsycho;
    }

    bool IsOverBusyArea(Vector3Int tilePosition)
    {
        return TilemapsManager.instance.BusySpotsTilemap.HasTile(tilePosition);
    }

}
