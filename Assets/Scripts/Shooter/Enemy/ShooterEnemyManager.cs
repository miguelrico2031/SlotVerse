using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//clase que se encarga de gestionar la salud y estado del enemigo; recibir daño, morir, y lanzar
//los eventos correspondientes para los otros scripts del enemigo
public class ShooterEnemyManager : MonoBehaviour, IBulletTarget, IEnemy
{
    public int Health { get { return _health; } } //getter de la health
    public bool IsAlive { get; private set; } //para saber si esta vivo

    //eventos que se lanzan al morir y ser golpeado
    public UnityEvent EnemyDie;
    public UnityEvent EnemyHit;

    [SerializeField] private int _health; //salud

    private int _maxHealth; //salud máxima

    private void Awake()
    {
        IsAlive = true;

        _maxHealth = _health;
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

        _health -= damage;

        if (_health <= 0) Die();
    }

    private void Die()
    {
        IsAlive = false;
        EnemyDie?.Invoke(); //invocar evento de muerte
    }

    //funcion para resetear estado del script cuando se spawnee con object pooling
    public void Reset()
    {
        _health = _maxHealth;
        IsAlive = true;
    }
}
