using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShooterEnemyStats : ScriptableObject
{
    public int Health;
    public int Damage;
    public float MoveSpeed;
    public float NextWaypointDistance;
    
}
