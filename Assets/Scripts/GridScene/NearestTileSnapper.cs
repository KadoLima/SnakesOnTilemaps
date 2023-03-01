using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestTileSnapper : MonoBehaviour
{
    void Start()
    {
        SnapToNearestTile(TilemapsManager.instance.Scenario.WorldToCell(transform.position));
    }

    void SnapToNearestTile(Vector3Int currentPos)
    {
        transform.position = TilemapsManager.instance.Scenario.GetCellCenterWorld(currentPos);
    }
}
