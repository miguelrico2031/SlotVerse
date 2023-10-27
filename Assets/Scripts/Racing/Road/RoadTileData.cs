using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RoadTileData")]
public class RoadTileData : ScriptableObject
{

    //Available directions for road entry and exit
    public enum Direction
    {
        North, 
        East, 
        South, 
        West
    }

    //Selects the possible grass spawn points for a road tile
    [System.Flags]
    public enum GrassDirection
    {
        North = 1,
        East = 2,
        South = 4,
        West = 8,
        NorthEast = 16,
        SouthEast = 32,
        NorthWest = 64,
        SouthWest = 128,
    }

    //Array of tiles that share the same entry and exit direction. Useful for road variants
    public Road[] RoadTiles;

    public Direction EntryDirection;
    public Direction ExitDirection;

    public GrassDirection GrassSpawnPoints;
}
