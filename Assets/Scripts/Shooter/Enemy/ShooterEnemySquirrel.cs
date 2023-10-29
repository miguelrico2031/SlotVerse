using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ShooterEnemySquirrel : ShooterEnemy
{
    [SerializeField] private Transform _rightFP, _upFP, _downFP, _leftFP;

    private Transform _firePoint; //objeto hijo desde donde se dispara
    private ShooterBulletSpawner _bulletSpawner; //spawner de balas
    private ShooterSquirrelStats _squirrelStats; //stats casteados para facil acceso
    private Rigidbody2D _rb;
    private ShooterEnemyMovement _movement;
    private Animator _animator;
    private Transform _player;
    private SquirrelState _state; //estado actual de la ardilla

    private Vector2 _firePointDirection = Vector2.right; //dirección cardinal del firepoint
    //array de las 4 direcciones cardinales
    private Vector2[] _firePointDirections = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    private bool _isShooting = false; //para saber si está disparando

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootSound;

    enum SquirrelState
    {
        Walking, //la ardilla camina hacia el jugador sin disparar
        Shooting, //la ardilla dispara mientras camina hacia el jugador
        Stopped //la ardilla se queda quieta disparando hacia el jugador
    }

    protected override void Awake()
    {
        base.Awake();

        _squirrelStats = (ShooterSquirrelStats) Stats;
        _rb = GetComponent<Rigidbody2D>();
        _movement = GetComponent<ShooterEnemyMovement>();
        _animator = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player").transform;
        _bulletSpawner = GameObject.Find("Enemy Bullet Spawner").GetComponent<ShooterBulletSpawner>();

        _movement.UpdateAnimationDirection = false;

        _state = SquirrelState.Walking; //el estado inicial es caminando

        //audioSource
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        //actualiza el estado y la direccion cardinal del firepoint cada 0.5 segundos
        //InvokeRepeating(nameof(SetStateAndDirection), 0.25f, 0.5f);
    }

    private void SetStateAndDirection()
    {
        //si el jugador o la ardilla estan muertos deja de actualizar
        if (!Manager.IsAlive || !_playerManager.IsAlive)
        {
            CancelInvoke(nameof(SetStateAndDirection));
            return;
        }

        SetDirection();
        SetState();
    }

    //funcion que actualiza el estado de la ardilla
    private void SetState()
    {
        float distanceToPlayer = Vector2.Distance(_rb.position, _player.position);

        //si la ardilla esta a la distancia de pararse
        if(distanceToPlayer <= _squirrelStats.StopDistance)
        {
            //si está viendo al jugador cambia a Stopped, y si no le estaba disparando lo hace
            if (PlayerOnSight())
            {
                //cambio a stop
                _state = SquirrelState.Stopped;

                _animator.SetBool("Walking", false);
                _animator.SetBool("Shooting", false);
                _animator.SetBool("Stopped", true);

                _movement.CanMove = false;
                _rb.velocity = Vector2.zero;
                if (!_isShooting) StartCoroutine(Shoot());
            }
            //si no ve al jugador cambia a Walking y camina hasta que lo vea
            else
            {
                //cambio a walking
                _state = SquirrelState.Walking;


                _animator.SetBool("Shooting", false);
                _animator.SetBool("Stopped", false);
                _animator.SetBool("Walking", true);

                _movement.CanMove = true;
            }

        }
        //si la ardilla esta entre la distancia de disparar y la de pararse
        else if(distanceToPlayer <= _squirrelStats.ShootDistance)
        {
            //si estaba Stopped es que sale de la distancia de pararse a la de disparar moviendose
            if (_state == SquirrelState.Stopped)
            {
                _movement.CanMove = true;

                //si el jugador está a la vista cambia a Shooting, si no a Walking hasta verlo
                if (PlayerOnSight())
                {
                    _state = SquirrelState.Shooting;

                    _animator.SetBool("Walking", false);
                    _animator.SetBool("Stopped", false);
                    _animator.SetBool("Shooting", true);

                    if (!_isShooting) StartCoroutine(Shoot());
                }
                else

                {
                    _state = SquirrelState.Walking;

                    _animator.SetBool("Shooting", false);
                    _animator.SetBool("Stopped", false);
                    _animator.SetBool("Walking", true);
                }
            }

            //Si estaba Walking es que entro a la distancia de disparar moviendose
            else if(_state == SquirrelState.Walking)
            {
                //si no ve al jugador sigue andando (Walkign) hasta verlo
                if (!PlayerOnSight()) return;

                //si ve al jugador, cambio a Shooting desde Walking
                _state = SquirrelState.Shooting;

                _animator.SetBool("Walking", false);
                _animator.SetBool("Stopped", false);
                _animator.SetBool("Shooting", true);

                if (!_isShooting) StartCoroutine(Shoot()); // empieza a disparar si no lo estaba
            }
        }

        //Si está a mayor distancia y no estaba en Walking, se actualiza a Walking
        else
        {
            if (_state == SquirrelState.Shooting)
            {
                _state = SquirrelState.Walking;

                _animator.SetBool("Shooting", false);
                _animator.SetBool("Stopped", false);
                _animator.SetBool("Walking", true);
            }
        }
    }

    //funcion que calcula si el jugador esta a la vista o no
    private bool PlayerOnSight()
    {
        //hace una layermask solo con la capa Enemy, para ignorarla en el raycast
        var mask = LayerMask.GetMask("Enemy");

        //hace un raycast hacia la posicion del jugador
        var hit = 
            Physics2D.Raycast(_firePoint.position, _player.position - _firePoint.position, 100f, ~mask);

        //devuelve true si hubo impacto en el raycast Y si el objeto impactado es el jugador
        return (hit.collider != null && hit.collider.CompareTag("Player"));
    }

    //funcion recursiva que dispara cada x tiempo una bala hacia el jugador
    private IEnumerator Shoot()
    {
        //Si esta solo caminando, no esta vivo o el jugador no esta vivo detiene el ciclo de disparo
        if (_state == SquirrelState.Walking || !Manager.IsAlive || !_playerManager.IsAlive)
        {
            _isShooting = false;
            yield break;
        }

        //declara que está disparando y espera un tiempo semialeatorio 
        _isShooting = true;

        //audiosource
        _audioSource.PlayOneShot(_shootSound);

        yield return new WaitForSeconds(_squirrelStats.ShootCooldown + Random.Range(0f, 0.7f));

        //despues de esperar, comprueba si el estado ahora es Walking o si esta muerto para no disparar
        if (_state == SquirrelState.Walking || !Manager.IsAlive)
        {
            _isShooting = false;
            yield break;
        }

        _animator.SetBool("Fire", true);

        //si no, dispara creando una balal con el spawner, y dandole la direccion hacia el jugador
        _bulletSpawner.CreateBullet(_firePoint.position, _player.position - _firePoint.position);

        yield return new WaitForSeconds(0.05f);
        _animator.SetBool("Fire", false);

        //llamada recursiva para hacer un ciclo de disparo
        yield return StartCoroutine(Shoot());
    }

    //funcion que comprueba y cambia la direccion del firepoint
    private void SetDirection()
    {
        //calcula la direccion al jugador y crea un vector para la direccion nueva
        Vector2 directionToPlayer = (Vector2) _player.position - _rb.position;
        Vector2 newDirection = _firePointDirection;

        //valor para encontrar la direccion cardinal con menor angulo respecto a la direccion
        //hacia el jugador
        float smallestAngle = 10000f;

        //itera las 4 direcciones cardinales y se queda con la que menor angulo tenga
        //que seria la mas cercana al jugador
        foreach(var d in _firePointDirections)
        {
            float angle = Vector2.Angle(directionToPlayer, d);
            if (angle < smallestAngle)
            {
                smallestAngle = angle;
                newDirection = d;
            }
        }

        //comprueba que la direccion nueva no sea la misma actual, y en ese caso la actualiza
        if(newDirection != _firePointDirection)
        {
            _firePointDirection = newDirection;

            if (_firePointDirection == Vector2.right) _firePoint = _rightFP;
            else if(_firePointDirection == Vector2.up) _firePoint = _upFP;
            else if(_firePointDirection == Vector2.left) _firePoint = _leftFP;
            else if(_firePointDirection == Vector2.down) _firePoint = _downFP;

            _animator.SetFloat("x", _firePointDirection.x);
            _animator.SetFloat("y", _firePointDirection.y);
        }
    }

    //funcion no usada pero implementada por necesidad al ser subclase de ShooterEnemy
    protected override void OnTargetAtRange(ISEnemyTarget target)
    {
        if (!Manager.IsAlive) return;
    }

    //funcion para resetear el estado de la ardilla al hacer object pooling
    public override void Reset()
    {
        base.Reset();

        _state = SquirrelState.Walking;
        _isShooting = false;
        _firePoint = _rightFP;
        _firePointDirection = Vector2.right;
        if(_playerManager.IsAlive) InvokeRepeating(nameof(SetStateAndDirection), 0.25f, 0.5f);
    }
}
