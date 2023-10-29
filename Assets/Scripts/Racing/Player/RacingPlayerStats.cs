using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "que")]
public class RacingPlayerStats : APlayerStats
{
    public int Health;

    public override int GetMaxHealth() => Health;
}
