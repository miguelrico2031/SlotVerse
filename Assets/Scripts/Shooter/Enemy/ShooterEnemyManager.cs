using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//clase que se encarga de gestionar la salud y estado del enemigo; recibir daño, morir, y lanzar
//los eventos correspondientes para los otros scripts del enemigo
public class ShooterEnemyManager : MonoBehaviour, ISPlayerBulletTarget, ISSpawnableEnemy
{
    public int CurrentHealth { get; private set; } //getter de la health
    public bool IsAlive { get; private set; } //para saber si esta vivo

    //eventos que se lanzan al morir y ser golpeado
    [HideInInspector] public UnityEvent<ShooterEnemy> EnemyDie;
    [HideInInspector] public UnityEvent<PlayerAttackInfo> EnemyHit;


    private ShooterEnemyStats _stats;
    private SpriteRenderer _renderer;

    private Color _spriteColor;

    private void Awake()
    {
        _stats = GetComponent<ShooterEnemy>().Stats;
        _renderer = GetComponent<SpriteRenderer>();

        _spriteColor = _renderer.color;

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

        var duration = IsAlive ? attackInfo.KnockbackDuration : _stats.DeadTime;
        StartCoroutine(ChangeColor(duration)); //mientras dure el knockback se pondra rojo si fue dañado o gris si fue dañado y murio

        EnemyHit?.Invoke(attackInfo); //invocar al evento de ser golpeado

    }

    private IEnumerator ChangeColor(float duration)
    {
        _renderer.color = IsAlive ? _stats.DamageColor : _stats.DeadColor;
        yield return new WaitForSeconds(duration);
        _renderer.color = _spriteColor;
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
        EnemyDie?.Invoke(GetComponent<ShooterEnemy>()); //invocar evento de muerte
    }

    //funcion para resetear estado del script cuando se spawnee con object pooling
    public void Reset()
    {
        CurrentHealth = _stats.Health;
        IsAlive = true;
        _renderer.color = _spriteColor;
    }
}
