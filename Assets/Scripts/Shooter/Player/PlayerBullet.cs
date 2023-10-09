using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//IMPORTANTE: para usar esta clase se debe instanciar una bala y justo después llamar al método
//FireBullet para asignarle una dirección y una velocidad.

public class PlayerBullet : ShooterBullet //Clase Monolítica de una bala disparada por el jugador
{
    [SerializeField] private PlayerStats _stats;

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

        //metodo implementado por todos los objetos interactuables
        //al final de este método se llamará a DestroyBullet siempre

        var info = new PlayerAttackInfo()
        {
            Bullet = this,
            Damage = _damage,
            Position = _rb.position,
            KnockbackForce = _stats.BulletKnockbackForce,
            KnockbackDuration = _stats.BulletKnockbackDuration
        };

        target.Hit(info); 
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
