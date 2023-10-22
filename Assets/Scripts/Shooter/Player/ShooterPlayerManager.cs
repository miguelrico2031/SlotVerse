using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//script encargado de manejar la salud y estado del jugador (basicamente recibir daño y morir)
public class ShooterPlayerManager : MonoBehaviour, ISEnemyTarget, ISEnemyBulletTarget
{
    public int CurrentHealth { get; private set; }
    public bool IsAlive { get; private set; }
    public ShooterPlayerStats Stats { get { return _stats; } } //getter de los stats para las demas clases


    [HideInInspector] public UnityEvent<EnemyAttackInfo> PlayerHit; //evento de ser golpeado
    [HideInInspector] public UnityEvent PlayerDie; //evneto de muerte

    [SerializeField] private ShooterPlayerStats _stats;

    //para saber si es invulnerable y no hacerse daño, pues lo es temporalmente al ser atacado
    private bool _invulnerable;


    private void Awake()
    {
        IsAlive = true;

        CurrentHealth = Stats.Health;
    }

    //funcion que recibe el ataque enemigo
    public void Hit(EnemyAttackInfo attackInfo)
    {
        if (_invulnerable) return; //si el jugador es invulnerable no hace nada

        TakeDamage(attackInfo.Damage); //se descuenta el daño y comprueba estado

        PlayerHit?.Invoke(attackInfo); //invocar evento de ser golpeado, con la posicion del enemigo

        //si el ataque es de una bala, se manda a destruir
        if (attackInfo.Bullet) attackInfo.Bullet.DestroyBullet();

        //el jugador se vuelve invulnerable por un momento tras recibir el ataque
        if(IsAlive) StartCoroutine(InvulnerabilityTime());
    }

    //recibir daño y cuando corresponda morir
    private void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0) Die();
    }

    private void Die()
    {
        IsAlive = false;
        PlayerDie?.Invoke();
    }

    //funcion que hace al jugador invulnerable por unos segundos
    private IEnumerator InvulnerabilityTime()
    {
        //hacemos que no pueda ser golpeado cuerpo a cuerpo (con _invulnerable)
        //y que no sea impactado por balas enemigas (excluyendo EnemyBullet de las capas de colision
        _invulnerable = true;
        GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask("EnemyBullet");

        yield return new WaitForSeconds(Stats.BulletKnockbackDuration);

        //se revierten los cambios y se deja de ser invulnerable
        _invulnerable = false;
        GetComponent<Rigidbody2D>().excludeLayers = new LayerMask();

    }


}
