using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//clase de las balas del jugador
public class PlayerBullet : ShooterBullet, IEnemyBulletTarget
{
    [SerializeField] private PlayerStats _stats; //referencia a los stats del jugador

    protected override void Awake()
    {
        base.Awake();

        _speed = _stats.BulletSpeed;
        _lifetime = _stats.BulletLifetime;
        _damage = _stats.BulletDamage;
    }

    protected override void OnCollision(Collision2D collision)
    {
        //Comprueba si el objeto colisionado es interactuable con la bala
        if (!collision.collider.TryGetComponent<IPlayerBulletTarget>(out var target)) return;

        //objeto con toda la informacion del ataque que vaya a necesitar el impactado
        var info = new PlayerAttackInfo()
        {
            Bullet = this,
            Damage = _damage,
            Position = _rb.position,
            KnockbackForce = _stats.BulletKnockbackForce,
            KnockbackDuration = _stats.BulletKnockbackDuration
        };

        //metodo implementado por todos los objetos interactuables
        //al final de este método se llamará a DestroyBullet siempre
        target.Hit(info); 
    }

    public void Hit(EnemyAttackInfo attackInfo)
    {
        DestroyBullet();
    }
}

public struct PlayerAttackInfo
{
    public PlayerBullet Bullet;
    public int Damage;
    public Vector2 Position;
    public float KnockbackForce;
    public float KnockbackDuration;
}
