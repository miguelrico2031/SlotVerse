using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{

    public RoadTileData LastTile { get; private set; }

    [SerializeField] private Vector3 _tileSize;
    [SerializeField] private RoadTileData[] _turnTileData;
    [SerializeField] private RoadTileData[] _straightTileData;
    [SerializeField] private RoadTileData _firstTile;
    [SerializeField] [Range(0f, 1f)]private float _curveChance; 

    private Vector3 _spawnPosition;
    private Vector3 _grassSpawnPosition;

    private void Awake()
    {
        _spawnPosition = Vector3.zero;
        LastTile = null;
    }

    RoadTileData PickValidNextTile() //devolver un SO que sea valido para usarse como siguiewnte tile de carretera
    {
        if (LastTile == null) return _firstTile;

        //List storing all available tiles to spawn
        List<RoadTileData> allowedTiles = new List<RoadTileData>();
        //Set to north since first tile is supposed to be South-North
        RoadTileData.Direction nextRequiredDirection = RoadTileData.Direction.North;

        //Depending on the last tile spawned exit direction, picks a valid entry direction and a spawn position for the next tile
        switch (LastTile.ExitDirection)
        {
            case RoadTileData.Direction.North:
                nextRequiredDirection = RoadTileData.Direction.South;
                _spawnPosition = _spawnPosition + new Vector3(0f, 0, _tileSize.z);
                break;

            case RoadTileData.Direction.East:
                nextRequiredDirection = RoadTileData.Direction.West;
                _spawnPosition = _spawnPosition + new Vector3(_tileSize.x, 0, 0);
                break;

            case RoadTileData.Direction.South:
                nextRequiredDirection = RoadTileData.Direction.North;
                _spawnPosition = _spawnPosition + new Vector3(0f, 0, -_tileSize.z);
                break;

            case RoadTileData.Direction.West:
                nextRequiredDirection = RoadTileData.Direction.East;
                _spawnPosition = _spawnPosition + new Vector3(-_tileSize.x, 0, 0);
                break;

            default:
                break;
        }

        //This line makes straight roads more frequent than turns, so it is not an extremely twisted road
        var array = UnityEngine.Random.Range(0f, 1f) <= _curveChance ? _turnTileData : _straightTileData; 

        //Adds all valid road tiles to the list
        foreach (var d in array)  if (d.EntryDirection == nextRequiredDirection) allowedTiles.Add(d);

        //Picks a random tile from the previously selected tiles
        RoadTileData nextTile = allowedTiles[UnityEngine.Random.Range(0, allowedTiles.Count)];
        return nextTile;
    }

    public Road SpawnTile()
    {
        RoadTileData tileToSpawn = PickValidNextTile();

        //Creates the object that will store the road tile
        Road objectFromTile = tileToSpawn.RoadTiles[UnityEngine.Random.Range(0, tileToSpawn.RoadTiles.Length)];
        LastTile = tileToSpawn;

        SpawnGrassTiles(tileToSpawn);

        //Creates the road tile
        Road road = Instantiate(objectFromTile, _spawnPosition, Quaternion.identity);

        return road;
    }

    public void SpawnGrassTiles(RoadTileData roadTile)
    {
        Debug.Log("Hola quiero poner cespet");
        GrassTileData grassTile = null;
        Grass g = null;

        if (roadTile.GrassSpawnPoints.HasFlag(RoadTileData.GrassDirection.North))
        {
            Debug.Log("Cespe norte");
            Grass grassToSpawn = g.PickGrassTile(grassTile);
            _grassSpawnPosition = _spawnPosition + new Vector3(0f, 0, _tileSize.z);
            Instantiate(grassToSpawn, _grassSpawnPosition, Quaternion.identity);
        }
        if (roadTile.GrassSpawnPoints.HasFlag(RoadTileData.GrassDirection.West))
        {
            Debug.Log("Cespe oeste");
            Grass grassToSpawn = g.PickGrassTile(grassTile);
            _grassSpawnPosition = _spawnPosition + new Vector3(_tileSize.x, 0, 0);
            Instantiate(grassToSpawn, _grassSpawnPosition, Quaternion.identity);
        }
        if (roadTile.GrassSpawnPoints.HasFlag(RoadTileData.GrassDirection.South))
        {
            Debug.Log("Cespe sur");
            Grass grassToSpawn = g.PickGrassTile(grassTile);
            _grassSpawnPosition = _spawnPosition + new Vector3(0f, 0, -_tileSize.z);
            Instantiate(grassToSpawn, _grassSpawnPosition, Quaternion.identity);
        }
        if (roadTile.GrassSpawnPoints.HasFlag(RoadTileData.GrassDirection.East))
        {
            Debug.Log("Cespe este");
            Grass grassToSpawn = g.PickGrassTile(grassTile);
            _grassSpawnPosition = _spawnPosition + new Vector3(-_tileSize.x, 0, 0);
            Instantiate(grassToSpawn, _grassSpawnPosition, Quaternion.identity);
        }
        if (roadTile.GrassSpawnPoints.HasFlag(RoadTileData.GrassDirection.NorthEast))
        {
            Grass grassToSpawn = g.PickGrassTile(grassTile);
            _grassSpawnPosition = _spawnPosition + new Vector3(_tileSize.x, 0, _tileSize.z);
            Instantiate(grassToSpawn, _grassSpawnPosition, Quaternion.identity);
        }
        if (roadTile.GrassSpawnPoints.HasFlag(RoadTileData.GrassDirection.SouthEast))
        {
            Grass grassToSpawn = g.PickGrassTile(grassTile);
            _grassSpawnPosition = _spawnPosition + new Vector3(-_tileSize.x, 0, _tileSize.z);
            Instantiate(grassToSpawn, _grassSpawnPosition, Quaternion.identity);
        }
        if (roadTile.GrassSpawnPoints.HasFlag(RoadTileData.GrassDirection.NorthWest))
        {
            Grass grassToSpawn = g.PickGrassTile(grassTile);
            _grassSpawnPosition = _spawnPosition + new Vector3(_tileSize.x, 0, -_tileSize.z);
            Instantiate(grassToSpawn, _grassSpawnPosition, Quaternion.identity);
        }
        if (roadTile.GrassSpawnPoints.HasFlag(RoadTileData.GrassDirection.SouthWest))
        {
            Grass grassToSpawn = g.PickGrassTile(grassTile);
            _grassSpawnPosition = _spawnPosition + new Vector3(0f, 0, -_tileSize.z);
            Instantiate(grassToSpawn, _grassSpawnPosition, Quaternion.identity);
        }
    }

    public void ResetSpawnPosition() => _spawnPosition = Vector3.zero;
}
