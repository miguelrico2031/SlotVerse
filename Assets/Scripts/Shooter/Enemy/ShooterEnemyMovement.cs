using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//Script que mueve al enemigo hacia el jugador usando A* pathfinding y el rigidbody
public class ShooterEnemyMovement : MonoBehaviour, IEnemy
{
    [SerializeField] private float _speed; //velocidad de movimiento
    [SerializeField] private float _nextWaypointDistance; //distancia a la cual cambia de waypoint

    private Path _path; //camino creado con el A*
    private int _currentWaypoint = 0; //indice del waypoint actual
    private bool _reachedEndOfPath = false;
    private Transform _player; //transform del jugador para seguirlo

    Seeker _seeker; //Componente que genera el Path
    Rigidbody2D _rb;
    ShooterEnemyManager _manager; //referencia al manager para suscribirse al evento de muerte

    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _manager = GetComponent<ShooterEnemyManager>();
        _manager.EnemyDie.AddListener(OnEnemyDie);

        _player = GameObject.FindWithTag("Player").transform;
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
        if (_path == null) return;

        //si el indice del waypoint actual es (mayor o) igual al maximo
        //de waypoints es que ha terminado el path
        if(_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            return;
        }
        else _reachedEndOfPath = false;
          
        //direccion sacada con el waypoint actual
        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;

        _rb.AddForce(direction * _speed * Time.fixedDeltaTime);
        //_rb.MovePosition(_rb.position + direction * _speed * Time.fixedDeltaTime);

        //calcular la distancia con el waypoint para saber si estamos lo suficientemente
        //cerca para avanzar al siguiente waypoint
        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
        if (distance < _nextWaypointDistance) _currentWaypoint ++;
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
        _currentWaypoint = 0;
        _reachedEndOfPath = false;
        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }
}
