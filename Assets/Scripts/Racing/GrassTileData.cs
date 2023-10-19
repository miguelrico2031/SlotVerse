using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GrassTileData")]
public class GrassTileData : ScriptableObject
{
    //Array of available grass tiles. Allows adding some variations to the background
    public Grass[] GrassTiles;
}
