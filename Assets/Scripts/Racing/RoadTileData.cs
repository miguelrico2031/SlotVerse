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

    //Array of tiles that share the same entry and exit direction. Useful for road variants
    public Road[] RoadTiles;

    public Direction EntryDirection;
    public Direction ExitDirection;
}
