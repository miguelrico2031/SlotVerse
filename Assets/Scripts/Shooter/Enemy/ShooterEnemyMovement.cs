using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//Script que mueve al enemigo hacia el jugador usando A* pathfinding y el rigidbody
public class ShooterEnemyMovement : MonoBehaviour, ISpawnableEnemy
{
    [HideInInspector] public bool CanMove = true;

    private Path _path; //camino creado con el A*
    private int _currentWaypoint = 0; //indice del waypoint actual
    //private bool _reachedEndOfPath = false;
    private Transform _player; //transform del jugador para seguirlo

    private ShooterEnemy _enemy; //Componente con los datos
    Seeker _seeker; //Componente que genera el Path
    Rigidbody2D _rb;

    

    private void Awake()
    {
        _enemy = GetComponent<ShooterEnemy>();
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        
        _player = GameObject.FindWithTag("Player").transform;

        _enemy.Manager.EnemyHit.AddListener(OnEnemyHit);
        _enemy.Manager.EnemyDie.AddListener(OnEnemyDie);

        CanMove = true;
    }

    private void Start()
    {
        //Llama cada 0.5 segundos a actualizar el path
        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    void UpdatePath()
    {
        //esto sirve para no llamarlo mientras esta generando un path
        if (!_seeker.IsDone()) return;
        
        //empieza a generar un path y llama a OnPathComplete al terminarlo
        _seeker.StartPath(_rb.position, _player.position, OnPathComplete);
    }

    //asigna el path nuevo al actual
    private void OnPathComplete(Path p)
    {
        if (p.error) return;

        _path = p;
        _currentWaypoint = 0;
    }

    private void FixedUpdate()
    {
        if (_path == null || !CanMove) return;

        //si el indice del waypoint actual es (mayor o) igual al maximo
        //de waypoints es que ha terminado el path
        //if(_currentWaypoint >= _path.vectorPath.Count)
        //{
        //    _reachedEndOfPath = true;
        //    return;
        //}
        //else _reachedEndOfPath = false;
          
        //direccion sacada con el waypoint actual
        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;

        _rb.AddForce(direction * _enemy.Stats.MoveSpeed * Time.fixedDeltaTime);
        //_rb.MovePosition(_rb.position + direction * _speed * Time.fixedDeltaTime);

        //calcular la distancia con el waypoint para saber si estamos lo suficientemente
        //cerca para avanzar al siguiente waypoint
        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
        if (distance < _enemy.Stats.NextWaypointDistance) _currentWaypoint ++;
    }
    
    private void OnEnemyHit(PlayerAttackInfo attackInfo)
    {
        CanMove = false;
        var force = _rb.velocity.normalized * attackInfo.KnockbackForce;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(KnockbackTime(attackInfo.KnockbackDuration));
    }

    private IEnumerator KnockbackTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        CanMove = true;
    }
    

    //funcion llamada con el evento de muerte, para quitar el path y asi no moverse
    //y dejar de llamar al UpdatePath
    private void OnEnemyDie()
    {
        _path = null;
        CancelInvoke();
    }

    //reseteo del estado para el object pooling
    public void Reset()
    {
        CanMove = true;
        _currentWaypoint = 0;
        //_reachedEndOfPath = false;
        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }
}
