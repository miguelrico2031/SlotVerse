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

    public Vector2 TileSize = new Vector2(10f, 10f);

    public GameObject[] RoadTiles;
    public Direction EntryDirection;
    public Direction ExitDirection;
}
