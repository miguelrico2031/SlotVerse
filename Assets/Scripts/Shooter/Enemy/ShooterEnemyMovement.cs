using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


//
public class ShooterEnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _nextWaypointDistance;

    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;
    private Transform _player;

    Seeker _seeker;
    Rigidbody2D _rb;

    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (!_seeker.IsDone()) return;
        
        _seeker.StartPath(_rb.position, _player.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (p.error) return;

        _path = p;
        _currentWaypoint = 0;
    }

    private void FixedUpdate()
    {
        if (_path == null) return;

        if(_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            return;
        }

        _reachedEndOfPath = false;

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position);

        _rb.AddForce(direction.normalized * _speed * Time.fixedDeltaTime);

        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);

        if (distance < _nextWaypointDistance) _currentWaypoint ++;
    }

}
