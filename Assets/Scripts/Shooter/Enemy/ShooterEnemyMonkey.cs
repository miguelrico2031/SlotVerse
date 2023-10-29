using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//clase del enemigo mono que se encarga de atacar a los objetos con la interfaz IEnemyTarget
//usando un trigger collider que representa su área de ataque
//Tambien se encarga de las animaciones y de cualquier cosa especifica de este tipo de enemigo
public class ShooterEnemyMonkey : ShooterEnemy
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _meleeTrigger; //referencia al trigger que detecta objetivos melee
    private ShooterEnemyMovement _movement;
    private Animator _animator;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootSound;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _meleeTrigger = transform.GetChild(0).GetComponent<CapsuleCollider2D>();
        _movement = GetComponent<ShooterEnemyMovement>();
        _animator = GetComponent<Animator>();
        _animator.keepAnimatorStateOnDisable = false;

        //audiosource
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void OnTargetAtRange(ISEnemyTarget target)
    {
        if (!Manager.IsAlive) return;
        //Cuando un target (el jugador) esta a melee
        StartCoroutine(AttackAfterDelay(target));
    }

    //función que deja quieto al mono un tiempo, y después comprueba si el target
    //(jugador) sigue dentro del área de ataque, siendo así le hace daño
    private IEnumerator AttackAfterDelay(ISEnemyTarget target)
    {
        //deshabilita el movimiento (se queda quieto)
        _movement.CanMove = false;
        _rb.velocity = Vector2.zero;

        //activa la animación de esperar
        _animator.SetBool("Hold", true);

        //espera x tiempo sin hacer nada
        float delay = ((ShooterMonkeyStats)Stats).AttackDelay;
        yield return new WaitForSeconds(delay);

        //calcula el overlap del trigger de melee para saber si el target sigue a rango
        var overlaps = Physics2D.OverlapCapsuleAll
            (_meleeTrigger.bounds.center, _meleeTrigger.bounds.size, _meleeTrigger.direction, 0f);

        bool attacked = false;
        //itera todos los objetos que estén a rango
        foreach (var c in overlaps)
        {
            if (!c.TryGetComponent<ISEnemyTarget>(out var t)) continue;
            if(t != target) continue;

            //si encuentra al target en los overlaps, lo ataca
            attacked = true;

            //crea un objeto con toda la info relevante del ataque
            var info = new EnemyAttackInfo()
            {
                Bullet = null,
                Damage = Stats.Damage,
                Position = _rb.position,
                KnockbackForce = Stats.KnockbackForce,
                KnockbackDuration = Stats.KnockbackDuration
            };

            target.Hit(info);

            _animator.SetBool("Hold", false);
            _animator.SetBool("Attack", true);

            //audiosource
            _audioSource.PlayOneShot(_shootSound);

            yield return new WaitForSeconds(0.05f);
            _animator.SetBool("Attack", false);
        }

        if(!attacked)
        {
            _animator.SetBool("Hold", false);
            _animator.SetBool("Attack", false);
        }

        //despues de revisar y posiblemente atacar, se puede volver a mover
        _movement.CanMove = true;
    }

}
