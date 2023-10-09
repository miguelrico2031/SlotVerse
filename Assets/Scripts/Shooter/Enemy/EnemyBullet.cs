using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyBullet : ShooterBullet
{
    [SerializeField] private ShooterSquirrelStats _stats;

    protected override void Awake()
    {
        base.Awake();

        _speed = _stats.BulletSpeed;
        _lifetime = _stats.BulletLifeTime;
        _damage = _stats.Damage;
    }

    protected override void OnCollision(Collision2D collision)
    {
        //Comprueba si el objeto colisionado es interactuable con la bala
        if (!collision.collider.TryGetComponent<IEnemyBulletTarget>(out var target)) return;

        //metodo implementado por todos los objetos interactuables
        //al final de este método se llamará a DestroyBullet siempre 

        var info = new EnemyAttackInfo()
        {
            IsBullet = true,
            Bullet = this,
            Damage = _damage,
            Position = _rb.position,
            KnockbackForce = _stats.KnockbackForce,
            KnockbackDuration = _stats.KnockbackDuration
        };

        target.Hit(info);
    }
}
