using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallTilesEffector : MonoBehaviour
{
    [SerializeField] Tilemap wallsTilemap;
    [SerializeField] TileBase invisTile;
    [SerializeField] TileBase normalTile;
    [SerializeField] GameObject wallLight;

    [SerializeField] float effectSpeed = 0.01f;

    bool isDone = false;
    public bool IsDone => isDone;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartEffect());
    }

    IEnumerator StartEffect()
    {
        foreach (Vector3Int pos in wallsTilemap.cellBounds.allPositionsWithin)
        {
            if (wallsTilemap.HasTile(pos))
            {
                wallsTilemap.SetTile(pos, invisTile);
            }
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Vector3Int pos in wallsTilemap.cellBounds.allPositionsWithin)
        {
            if (wallsTilemap.HasTile(pos))
            {
                yield return new WaitForSeconds(effectSpeed);
                wallsTilemap.SetTile(pos, normalTile);
                
                GameObject _g = Instantiate(wallLight, wallsTilemap.GetCellCenterWorld(pos), Quaternion.identity);
                _g.transform.SetParent(this.transform);
            }
        }
        yield return new WaitForSeconds(0.1f);
        isDone = true;
    }


}
