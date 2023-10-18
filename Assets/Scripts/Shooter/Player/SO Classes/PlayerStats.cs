using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase de una sola instancia que tiene los stats del jugador
public class PlayerStats : ScriptableObject
{
    public float MoveSpeed;
    public int Health;
    public float BulletSpeed;
    public int BulletDamage;
    public float BulletLifetime;
    public float BulletKnockbackForce;
    public float BulletKnockbackDuration;
}
