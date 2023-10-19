using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grass : MonoBehaviour
{
    public Grass PickGrassTile(GrassTileData grassData)
    {
        return grassData.GrassTiles[Random.Range(0, grassData.GrassTiles.Length)];
    }
}