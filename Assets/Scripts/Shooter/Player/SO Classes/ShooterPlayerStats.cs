using UnityEngine;

//clase de una sola instancia que tiene los stats del jugador
public class ShooterPlayerStats : ScriptableObject
{
    public float MoveSpeed;
    public int Health;
    public float BulletSpeed;
    public int BulletDamage;
    public float BulletLifetime;
    public float BulletKnockbackForce;
    public float BulletKnockbackDuration;
    public Color DamageColor = Color.red;
    public Color DeadColor = Color.gray;

}
