using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase del enemigo erizo que se encarga de atacar a los objetos con la interfaz IEnemyTarget
//usando un trigger collider que representa su área de ataque
//Tambien se encarga de las animaciones y de cualquier cosa especifica de este tipo de enemigo
public class ShooterEnemyHedgehog : ShooterEnemy
{
    private Rigidbody2D _rb;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    //Hace daño al jugador o a cualquier cosa que sea IEnemyTarget al entrar en su area de ataque
    protected override void OnTargetAtRange(IEnemyTarget target)
    {
        var info = new EnemyAttackInfo()
        {
            IsBullet = false,
            Bullet = null,
            Damage = Stats.Damage,
            Position = _rb.position,
            KnockbackForce = Stats.KnockbackForce,
            KnockbackDuration = Stats.KnockbackDuration
        };
        target.Hit(info); //llama al metodo hit del target
    }

}
