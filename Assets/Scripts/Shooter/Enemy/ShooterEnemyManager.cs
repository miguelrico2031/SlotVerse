using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//clase que se encarga de gestionar la salud y estado del enemigo; recibir daño, morir, y lanzar
//los eventos correspondientes para los otros scripts del enemigo
public class ShooterEnemyManager : MonoBehaviour, IBulletTarget, ISpawnableEnemy
{
    public int CurrentHealth { get { return _currentHealth; } } //getter de la health
    public bool IsAlive { get; private set; } //para saber si esta vivo

    //eventos que se lanzan al morir y ser golpeado
    [HideInInspector] public UnityEvent EnemyDie;
    [HideInInspector] public UnityEvent EnemyHit;

    private int _currentHealth; //salud

    private ShooterEnemy _enemy;


    private void Awake()
    {
        _enemy = GetComponent<ShooterEnemy>();

        _currentHealth = _enemy.Stats.Health;
        IsAlive = true;
    }

    //metodo llamado por la bala al detectar una colision con un IBulletTarget
    public void Hit(Bullet bullet)
    {
        int damage = bullet.Damage;
        bullet.DestroyBullet(); //destruir (devolver a la pool) la bala despues de usarla

        TakeDamage(damage);

        EnemyHit?.Invoke(); //invocar al evento de ser golpeado

    }

    private void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        IsAlive = false;
        EnemyDie?.Invoke(); //invocar evento de muerte
    }

    //funcion para resetear estado del script cuando se spawnee con object pooling
    public void Reset()
    {
        _currentHealth = _enemy.Stats.Health;
        IsAlive = true;
    }
}
