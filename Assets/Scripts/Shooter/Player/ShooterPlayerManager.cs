using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//script encargado de manejar la salud y estado del jugador (basicamente recibir daño y morir)
public class ShooterPlayerManager : MonoBehaviour, IEnemyTarget, IEnemyBulletTarget
{
    public int CurrentHealth { get; private set; }
    public bool IsAlive { get; private set; }
    public PlayerStats Stats { get { return _stats; } } //getter de los stats para las demas clases


    [HideInInspector] public UnityEvent<EnemyAttackInfo> PlayerHit; //evento de ser golpeado
    [HideInInspector] public UnityEvent PlayerDie; //evneto de muerte

    [SerializeField] private PlayerStats _stats;

    private bool _invulnerable;


    private void Awake()
    {
        IsAlive = true;

        CurrentHealth = Stats.Health;
    }

    public void Hit(EnemyAttackInfo attackInfo)
    {
        if (_invulnerable) return;

        TakeDamage(attackInfo.Damage);

        PlayerHit?.Invoke(attackInfo); //invocar evento de ser golpeado, con la posicion del enemigo

        if (attackInfo.Bullet) attackInfo.Bullet.DestroyBullet();

        StartCoroutine(InvulnerabilityTime());
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

    private IEnumerator InvulnerabilityTime()
    {
        _invulnerable = true;
        GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask("EnemyBullet");
        yield return new WaitForSeconds(Stats.BulletKnockbackDuration);
        _invulnerable = false;
        GetComponent<Rigidbody2D>().excludeLayers = new LayerMask();

    }


}
