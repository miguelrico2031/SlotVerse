using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShooterPlayerManager : MonoBehaviour, IEnemyTarget, IEnemyBulletTarget
{
    public bool IsAlive { get; private set; }
    public PlayerStats Stats { get { return _stats; } }


    public UnityEvent<EnemyAttackInfo> PlayerHit; //evento de ser golpeado
    public UnityEvent PlayerDie; //evneto de muerte

    [SerializeField] private PlayerStats _stats;

    private int _health;

    private void Awake()
    {
        IsAlive = true;

        _health = Stats.Health;
    }

    public void Hit(EnemyAttackInfo attackInfo)
    {

        TakeDamage(attackInfo.Damage);

        PlayerHit?.Invoke(attackInfo); //invocar evento de ser golpeado, con la posicion del enemigo
    }

    public void Hit(EnemyBullet bullet)
    {
        TakeDamage(bullet.Damage);
    }

    //recibir daño y cuando corresponda morir
    private void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        _health -= damage;

        if (_health <= 0) Die();
    }

    private void Die()
    {
        IsAlive = false;
        PlayerDie?.Invoke();
    }


}
