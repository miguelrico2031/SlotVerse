using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//clase que se encarga de gestionar la salud y estado del enemigo; recibir daño, morir, y lanzar
//los eventos correspondientes para los otros scripts del enemigo
public class ShooterEnemyManager : MonoBehaviour, IPlayerBulletTarget, ISpawnableEnemy
{
    public int CurrentHealth { get; private set; } //getter de la health
    public bool IsAlive { get; private set; } //para saber si esta vivo

    //eventos que se lanzan al morir y ser golpeado
    [HideInInspector] public UnityEvent EnemyDie;
    [HideInInspector] public UnityEvent<PlayerAttackInfo> EnemyHit;


    private ShooterEnemyStats _stats;


    private void Awake()
    {
        _stats = GetComponent<ShooterEnemy>().Stats;

        CurrentHealth = _stats.Health;
        IsAlive = true;
    }

    //metodo llamado por la bala al detectar una colision con un IBulletTarget
    public void Hit(PlayerAttackInfo attackInfo)
    {
        if (!IsAlive) return;

        int damage = attackInfo.Bullet.Damage;
        attackInfo.Bullet.DestroyBullet(); //destruir (devolver a la pool) la bala despues de usarla

        TakeDamage(damage);

        EnemyHit?.Invoke(attackInfo); //invocar al evento de ser golpeado

    }

    private void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0) Die();
    }

    private void Die()
    {
        IsAlive = false;
        EnemyDie?.Invoke(); //invocar evento de muerte
    }

    //funcion para resetear estado del script cuando se spawnee con object pooling
    public void Reset()
    {
        CurrentHealth = _stats.Health;
        IsAlive = true;
    }
}
