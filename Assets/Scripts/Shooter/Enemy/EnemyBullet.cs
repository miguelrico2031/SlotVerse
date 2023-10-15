using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

//bala del enemigo ardilla
public class EnemyBullet : ShooterBullet
{
    [SerializeField] private ShooterSquirrelStats _stats; //referencia a los stats

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

        //objeto con toda la informacion del ataque que vaya a necesitar el impactado

        var info = new EnemyAttackInfo()
        {
            Bullet = this,
            Damage = _damage,
            Position = _rb.position,
            KnockbackForce = _stats.KnockbackForce,
            KnockbackDuration = _stats.KnockbackDuration
        };

        //metodo implementado por todos los objetos interactuables
        //al final de este m�todo se llamar� a DestroyBullet siempre 
        target.Hit(info);
    }
}
