using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemySquirrel : ShooterEnemy
{
    [SerializeField] private Transform _firePoint;


    private BulletSpawner _bulletSpawner;
    private ShooterSquirrelStats _squirrelStats; //stats casteados para facil acceso
    private Rigidbody2D _rb;
    private ShooterEnemyMovement _movement;
    private Transform _player;
    private SquirrelState _state;

    private Vector2 _firePointDirection = Vector2.right;
    private Vector2[] _firePointDirections = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    private bool _isShooting = false;

    protected override void Awake()
    {
        base.Awake();

        _squirrelStats = (ShooterSquirrelStats) Stats;
        _rb = GetComponent<Rigidbody2D>();
        _movement = GetComponent<ShooterEnemyMovement>();
        _player = GameObject.FindWithTag("Player").transform;
        _bulletSpawner = GameObject.Find("Enemy Bullet Spawner").GetComponent<BulletSpawner>();

        _state = SquirrelState.Walking;
    }

    protected override void Start()
    {
        InvokeRepeating(nameof(SetStateAndDirection), 0.25f, 0.5f);
    }

    private void SetStateAndDirection()
    {
        if (!Manager.IsAlive || !_playerManager.IsAlive)
        {
            CancelInvoke(nameof(SetStateAndDirection));
            return;
        }

        SetState();
        SetDirection();
    }

    private void SetState()
    {
        float distanceToPlayer = Vector2.Distance(_rb.position, _player.position);

        if(distanceToPlayer <= _squirrelStats.StopDistance)
        {
            if (PlayerOnSight())
            {
                //cambio a stop
                _state = SquirrelState.Stopped;
                _movement.CanMove = false;
                _rb.velocity = Vector2.zero;
                if (!_isShooting) StartCoroutine(Shoot());
            }
            else
            {
                //cambio a walking
                _state = SquirrelState.Walking;
                _movement.CanMove = true;
            }

        }

        else if(distanceToPlayer <= _squirrelStats.ShootDistance)
        {
            if (_state == SquirrelState.Stopped)
            {
                //cambio desde stop
                _movement.CanMove = true;
                //si el jugador está a la vista dispara, si no camina solo
                if(PlayerOnSight())
                {
                    _state = SquirrelState.Shooting;
                    if (!_isShooting) StartCoroutine(Shoot());
                }   
                else
                {
                    _state =  SquirrelState.Walking;
                }
                
            }
            else if(_state == SquirrelState.Walking)
            {
                if (!PlayerOnSight()) return;
                //cambio a shooting desde walking
                _state = SquirrelState.Shooting;
                if (!_isShooting) StartCoroutine(Shoot());
            }
        }

        else
        {
            //cambio a walking desde shooting
            if (_state == SquirrelState.Shooting) _state = SquirrelState.Walking;
        }
    }

    private bool PlayerOnSight()
    {
        var mask = LayerMask.GetMask("Enemy");
        var hit = Physics2D.Raycast(_firePoint.position, _player.position - _firePoint.position, 100f, ~mask);

        //if (hit.collider) Debug.Log(hit.collider.gameObject);
        return (hit.collider != null && hit.collider.CompareTag("Player"));
    }

    private IEnumerator Shoot()
    {
        
        if (_state == SquirrelState.Walking || !Manager.IsAlive || !_playerManager.IsAlive)
        {
            _isShooting = false;
            yield break;
        }
        _isShooting = true;
        yield return new WaitForSeconds(_squirrelStats.ShootCooldown + Random.Range(0f, 0.7f));

        if (_state == SquirrelState.Walking || !Manager.IsAlive)
        {
            _isShooting = false;
            yield break;
        }

        //disparar
        _bulletSpawner.CreateBullet(_firePoint.position, _player.position - _firePoint.position);

        yield return StartCoroutine(Shoot());
    }

    private void SetDirection()
    {
        Vector2 directionToPlayer = (Vector2) _player.position - _rb.position;
        Vector2 newDirection = _firePointDirection;

        float smallestAngle = 10000f;

        foreach(var d in _firePointDirections)
        {
            float angle = Vector2.Angle(directionToPlayer, d);
            if (angle < smallestAngle)
            {
                smallestAngle = angle;
                newDirection = d;
            }
        }

        if(newDirection != _firePointDirection)
        {
            _firePointDirection = newDirection;

            _firePoint.localPosition = _firePointDirection * 1.2f;
        }


    }

    protected override void OnTargetAtRange(IEnemyTarget target)
    {
        if (!Manager.IsAlive) return;
    }

    public override void Reset()
    {
        base.Reset();

        _state = SquirrelState.Walking;
        _isShooting = false;
        if(_playerManager.IsAlive) InvokeRepeating(nameof(SetStateAndDirection), 0.25f, 0.5f);
    }
}

enum SquirrelState
{
    Walking, Shooting, Stopped
}
