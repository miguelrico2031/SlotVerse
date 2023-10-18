using System.Collections.Generic;
using UnityEngine;

public class GenerateRoad : MonoBehaviour
{

    public RoadTileData[] roadTileData;
    public RoadTileData firstTile;

    private RoadTileData _lastTile;

    public Vector3 spawn;

    private Vector3 _spawnPos;
    public int TilesToSpawn = 10;

    void OnEnable()
    {
        TriggerExit.OnTileExited += PickSpawnTile;
    }

    private void OnDisable()
    {
        TriggerExit.OnTileExited -= PickSpawnTile;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PickSpawnTile();
        }
    }

    private void Start()
    {
        _lastTile = firstTile;

        for (int i = 0; i < TilesToSpawn; i++)
        {
            PickSpawnTile();
        }
    }

    RoadTileData PickTile()
    {
        List<RoadTileData> allowedTiles = new List<RoadTileData>();
        RoadTileData nextTile = null;

        RoadTileData.Direction nextRequiredDirection = RoadTileData.Direction.North;

        switch (_lastTile.ExitDirection)
        {
            case RoadTileData.Direction.North:
                nextRequiredDirection = RoadTileData.Direction.South;
                _spawnPos = _spawnPos + new Vector3(0f, 0, _lastTile.TileSize.y);
                break;

            case RoadTileData.Direction.East:
                nextRequiredDirection = RoadTileData.Direction.West;
                _spawnPos = _spawnPos + new Vector3(_lastTile.TileSize.x, 0, 0);
                break;

            case RoadTileData.Direction.South:
                nextRequiredDirection = RoadTileData.Direction.North;
                _spawnPos = _spawnPos + new Vector3(0f, 0, -_lastTile.TileSize.y);
                break;

            case RoadTileData.Direction.West:
                nextRequiredDirection = RoadTileData.Direction.East;
                _spawnPos = _spawnPos + new Vector3(-_lastTile.TileSize.x, 0, 0);
                break;

            default:
                break;
        }

        for (int i = 0; i < roadTileData.Length; i++)
        {
            if (roadTileData[i].EntryDirection == nextRequiredDirection)
            {
                allowedTiles.Add(roadTileData[i]);
            }

            int index = Random.Range(0, allowedTiles.Count);

            Debug.Log("Index = " + index);

            nextTile = allowedTiles[index];
    
        }

        return nextTile;
    }

    void PickSpawnTile()
    {
        RoadTileData tileToSpawn = PickTile();

        GameObject objectFromTile = tileToSpawn.RoadTiles[Random.Range(0, tileToSpawn.RoadTiles.Length)];
        _lastTile = tileToSpawn;
        Instantiate(objectFromTile, _spawnPos + spawn, Quaternion.identity);
    }

    public void UpdateSpawnOrigin(Vector3 originDelta)
    {
        spawn = spawn + originDelta;
    }
}
