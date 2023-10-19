using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{

    public RoadTileData LastTile { get; private set; }

    [SerializeField] private Vector3 _tileSize;
    [SerializeField] private RoadTileData[] _curvesData;
    [SerializeField] private RoadTileData[] _straightsData;
    [SerializeField] private RoadTileData _firstTile;
    [SerializeField] [Range(0f, 1f)]private float _curveProbability; 

    private Vector3 _spawnPos;

    private void Awake()
    {
        _spawnPos = Vector3.zero;
        LastTile = null;
    }

    RoadTileData PickValidNextTile() //devolver un SO que sea valido para usarse como siguiewnte tile de carretera
    {
        if (LastTile == null) return _firstTile;

        List<RoadTileData> allowedTiles = new List<RoadTileData>();
        RoadTileData nextTile = null;

        RoadTileData.Direction nextRequiredDirection = RoadTileData.Direction.North;


        switch (LastTile.ExitDirection)
        {
            case RoadTileData.Direction.North:
                nextRequiredDirection = RoadTileData.Direction.South;
                _spawnPos = _spawnPos + new Vector3(0f, 0, _tileSize.z);
                break;

            case RoadTileData.Direction.East:
                nextRequiredDirection = RoadTileData.Direction.West;
                _spawnPos = _spawnPos + new Vector3(_tileSize.x, 0, 0);
                break;

            case RoadTileData.Direction.South:
                nextRequiredDirection = RoadTileData.Direction.North;
                _spawnPos = _spawnPos + new Vector3(0f, 0, -_tileSize.z);
                break;

            case RoadTileData.Direction.West:
                nextRequiredDirection = RoadTileData.Direction.East;
                _spawnPos = _spawnPos + new Vector3(-_tileSize.x, 0, 0);
                break;

            default:
                break;
        }

        var array = Random.Range(0f, 1f) <= _curveProbability ? _curvesData : _straightsData; 

        foreach (var d in array)  if (d.EntryDirection == nextRequiredDirection) allowedTiles.Add(d);
        

        int index = Random.Range(0, allowedTiles.Count);

        Debug.Log("Index = " + index);

        nextTile = allowedTiles[index];

        return nextTile;
    }

    public Road SpawnTile()
    {
        RoadTileData tileToSpawn = PickValidNextTile();

        Road objectFromTile = tileToSpawn.RoadTiles[Random.Range(0, tileToSpawn.RoadTiles.Length)];
        LastTile = tileToSpawn;
        Road road = Instantiate(objectFromTile, _spawnPos, Quaternion.identity);
        return road;
    }

    public void ResetSpawnPos() => _spawnPos = Vector3.zero;

    //public void UpdateSpawnOrigin(Vector3 originDelta)
    //{
    //    spawn = spawn + originDelta;
    //}
}
