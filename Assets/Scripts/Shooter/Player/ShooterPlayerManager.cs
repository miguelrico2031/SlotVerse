using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

//script encargado de manejar la salud y estado del jugador (basicamente recibir daño y morir)
public class ShooterPlayerManager : MonoBehaviour, ISEnemyTarget, ISEnemyBulletTarget, IPlayerManager
{
    public int CurrentHealth { get; private set; }
    public bool IsAlive { get; private set; }
    public ShooterPlayerStats Stats { get { return _stats; } } //getter de los stats para las demas clases

    public UnityEvent PlayerDie; //evneto de muerte
    public UnityEvent<EnemyAttackInfo> PlayerHit; //evento de ser golpeado

    [SerializeField] private ShooterPlayerStats _stats;

    private SpriteRenderer _renderer;
    private Color _spriteColor;

    //para saber si es invulnerable y no hacerse daño, pues lo es temporalmente al ser atacado
    private bool _invulnerable;

    public int GetCurrentHealth() => CurrentHealth;

    private AudioSource _audioSource;
    [SerializeField] AudioClip _takeDamageSound, _deathMusic;

    private void Awake()
    {
        IsAlive = true;

        CurrentHealth = Stats.Health;

        _renderer = GetComponent<SpriteRenderer>();
        _spriteColor = _renderer.color;

        //audiosource
        _audioSource = GetComponent<AudioSource>();
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
        {
            CurrentHealth -= damage;

            //reproducimos audioclip
            _audioSource.PlayOneShot(_takeDamageSound);
        }
        
        if (CurrentHealth <= 0)Die();
    }

    private void Die()
    {
        IsAlive = false;
        _renderer.color = _stats.DeadColor;
        PlayerDie?.Invoke();

        //audiosource
        _audioSource.PlayOneShot(_deathMusic);
    }

    //funcion que hace al jugador invulnerable por unos segundos
    private IEnumerator InvulnerabilityTime()
    {
        //hacemos que no pueda ser golpeado cuerpo a cuerpo (con _invulnerable)
        //y que no sea impactado por balas enemigas (excluyendo EnemyBullet de las capas de colision
        _invulnerable = true;
        GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask("EnemyBullet");

        //cambiamos de color
        _renderer.color = _stats.DamageColor;

        yield return new WaitForSeconds(Stats.BulletKnockbackDuration);

        //se revierten los cambios y se deja de ser invulnerable
        _invulnerable = false;
        GetComponent<Rigidbody2D>().excludeLayers = new LayerMask();

        //volvemos al color original
        _renderer.color = _spriteColor;

    }


}
