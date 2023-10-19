using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RoadTileData")]
public class RoadTileData : ScriptableObject
{
    public enum Direction
    {
        North, 
        East, 
        South, 
        West
    }

    public Road[] RoadTiles;
    public Direction EntryDirection;
    public Direction ExitDirection;
}
