using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemySquirrel : ShooterEnemy, ISpawnableEnemy
{
    [SerializeField] private Transform _firePoint;

    private ShooterSquirrelStats _squirrelStats; //stats casteados para facil acceso
    private Rigidbody2D _rb;
    private ShooterEnemyMovement _movement;
    private Transform _player;
    private SquirrelState _state;

    private Vector2 _firePointDirection = Vector2.right;
    private Vector2[] _firePointDirections = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    protected override void Awake()
    {
        base.Awake();

        _squirrelStats = (ShooterSquirrelStats) Stats;
        _rb = GetComponent<Rigidbody2D>();
        _movement = GetComponent<ShooterEnemyMovement>();
        _player = GameObject.FindWithTag("Player").transform;

        _state = SquirrelState.Walking;
    }

    protected override void Start()
    {
        InvokeRepeating(nameof(SetState), 0.25f, 0.5f);
        InvokeRepeating(nameof(SetDirection), 0.25f, 0.5f);
    }

    private void SetState()
    {
        if (!Manager.IsAlive) return;

        float distanceToPlayer = Vector2.Distance(_rb.position, _player.position);

        if(distanceToPlayer <= _squirrelStats.StopDistance)
        {
            if (PlayerOnSight())
            {
                //cambio a stop
                _state = SquirrelState.Stopped;
                _movement.CanMove = false;
                _rb.velocity = Vector2.zero;
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
                _state = PlayerOnSight() ? SquirrelState.Shooting : SquirrelState.Walking;
                
            }
            else if(_state == SquirrelState.Walking)
            {
                if (!PlayerOnSight()) return;
                //cambio a shooting desde walking
                _state = SquirrelState.Shooting;

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
        var hit = Physics2D.Raycast(_rb.position, (Vector2)_player.position - _rb.position, 100f, ~mask);

        //if (hit.collider) Debug.Log(hit.collider.gameObject);
        return (hit.collider != null && hit.collider.CompareTag("Player"));
    }

    private void SetDirection()
    {
        if (!Manager.IsAlive) return;

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

    public void Reset()
    {
        _state = SquirrelState.Walking;
    }
}

enum SquirrelState
{
    Walking, Shooting, Stopped
}
