using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase del enemigo erizo que se encarga de atacar a los objetos con la interfaz IEnemyTarget
//usando un trigger collider que representa su área de ataque
//Tambien se encarga de las animaciones y de cualquier cosa especifica de este tipo de enemigo
public class ShooterEnemyHedgehog : ShooterEnemy
{
    private Rigidbody2D _rb;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.keepAnimatorStateOnDisable = false;
    }

    //Hace daño al jugador o a cualquier cosa que sea IEnemyTarget al entrar en su area de ataque
    protected override void OnTargetAtRange(ISEnemyTarget target)
    {
        if(!Manager.IsAlive) return;
        //crea un objeto con toda la info relevante del ataque
        var info = new EnemyAttackInfo()
        {
            Bullet = null,
            Damage = Stats.Damage,
            Position = _rb.position,
            KnockbackForce = Stats.KnockbackForce,
            KnockbackDuration = Stats.KnockbackDuration
        };
        target.Hit(info); //llama al metodo hit del target

        _animator.SetBool("Attack", true); //activamos animacion de ataque
        Invoke(nameof(ResetAnimation), 0.05f); //reseteamos la animación para luego
    }

    private void ResetAnimation() => _animator.SetBool("Attack", false);

}
