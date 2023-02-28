using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapsManager : MonoBehaviour
{
    [SerializeField] Tilemap scenario;
    [SerializeField] TileBase DebugTile;
    public Tilemap Scenario => scenario;

    public static TilemapsManager instance;

    [SerializeField] List<Vector3Int> movableTilesPositions = new List<Vector3Int>();

    public PowerUpCreator powerUpCreator;
    public List<Vector3Int> MovableTilesPositions => movableTilesPositions;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        GetMovableArea();

        powerUpCreator.CreatePowerUpAtRandomPosition();
    }

    private void GetMovableArea()
    {
        Vector3Int _firstBottomLeftTilePos = new Vector3Int(scenario.cellBounds.xMin + (int)CellSizeX(), scenario.cellBounds.yMin + (int)CellSizeY());

        BoundsInt _movingArea = new BoundsInt(_firstBottomLeftTilePos, new Vector3Int(scenario.cellBounds.size.x - 2, scenario.cellBounds.size.y - 2, 1));

        foreach (Vector3Int t in _movingArea.allPositionsWithin)
        {
            movableTilesPositions.Add(t);
        }
    }

    public void ChangeTile(Vector3Int pos) //Changes the tile at the given "pos" position to another tile.
    {
        scenario.SetTile(pos, DebugTile);
    }

    public void ColorTile(Vector3Int pos) //Changes the color of the tile at the given "pos" position.
    {
        scenario.SetTileFlags(pos, TileFlags.None);
        scenario.SetColor(pos, Color.magenta);
    }

    public float CellSizeX() //Returns cell's X size.
    {
        return scenario.cellSize.x;
    }

    public float CellSizeY() //Returns cell's Y size.
    {
        return scenario.cellSize.y;
    }

    public Vector3Int GetRandomTilePosition()
    {
        return movableTilesPositions[Random.Range(0, movableTilesPositions.Count)];
    }
}
