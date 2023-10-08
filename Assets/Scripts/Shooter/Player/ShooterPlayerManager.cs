using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShooterPlayerManager : MonoBehaviour, IEnemyTarget
{
    public bool IsAlive { get; private set; }
    public int Health { get { return _health; } }
    public UnityEvent<Vector2> PlayerHit; //evento de ser golpeado cuyo argumento es la posicion del enemigo
    public UnityEvent PlayerDie; //evneto de muerte

    [SerializeField] private int _health;

    private void Awake()
    {
        IsAlive = true;
    }

    public void Hit(int damage, Vector2 enemyPosition)
    {

        TakeDamage(damage);

        PlayerHit?.Invoke(enemyPosition); //invocar evento de ser golpeado, con la posicion del enemigo
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
