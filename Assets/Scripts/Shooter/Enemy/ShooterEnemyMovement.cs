using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//Script que mueve al enemigo hacia el jugador usando A* pathfinding y el rigidbody
//Basado en el tutorial de Brackeys: https://www.youtube.com/watch?v=jvtFUfJ6CP8&t=1138s
public class ShooterEnemyMovement : MonoBehaviour, ISSpawnableEnemy
{
    [HideInInspector] public bool CanMove = true;

    private Path _path; //camino creado con el A*
    private int _currentWaypoint = 0; //indice del waypoint actual
    private Transform _player; //transform del jugador para seguirlo

    private ShooterEnemy _enemy; //Componente con los datos
    private Seeker _seeker; //Componente que genera el Path
    private Rigidbody2D _rb;
    private Collider2D _collider;

    private ShooterPlayerManager _playerManager;

    private void Awake()
    {
        _enemy = GetComponent<ShooterEnemy>();
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _player = GameObject.FindWithTag("Player").transform;
        _playerManager = _player.GetComponent<ShooterPlayerManager>();

        CanMove = true;
    }

    private void Start()
    {
        //nos suscribimos al evento de ser golpeado para encargarnos del knockback
        _enemy.Manager.EnemyHit.AddListener(OnEnemyHit);
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
        //si no hay camino o no se puede mover, no entra
        if (_path == null || !CanMove) return;
          
        //direccion sacada con el waypoint actual
        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;

        //movemos al rigidbody a�adiendo fuerza en la direccion calculada 
        _rb.AddForce(direction * _enemy.Stats.MoveSpeed * Time.fixedDeltaTime);

        //calcular la distancia con el waypoint para saber si estamos lo suficientemente
        //cerca para avanzar al siguiente waypoint
        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
        if (distance < _enemy.Stats.NextWaypointDistance) _currentWaypoint ++;
    }

    //funci�n que detiene el movimiento del enemigo por un tiempo cuando es golpeado, esto para 
    //que la fuerza de knockback se aplique bien y no se contrarreste con la fuerza del movimiento
    private void OnEnemyHit(PlayerAttackInfo attackInfo)
    {
        CanMove = false;

        //Se calcula la fuerza con la direccion a la bala y la fuerza de la bala
        Vector2 force = (_rb.position - attackInfo.Position).normalized * attackInfo.KnockbackForce;
        _rb.velocity = Vector2.zero; //se quita la velocidad previa
        _rb.AddForce(force, ForceMode2D.Impulse); //se a�ade la fuerza

        //espera un tiempo y habilita de nuevo el movimiento
        StartCoroutine(KnockbackTime(attackInfo.KnockbackDuration));
    }

    private IEnumerator KnockbackTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        CanMove = true;
    }
    

    //funcion llamada con el evento de muerte, para quitar el path y asi no moverse
    //y dejar de llamar al UpdatePath
    private void OnEnemyDie(ShooterEnemy enemy)
    {
        _path = null;
        CanMove = false;
        CancelInvoke();

        _collider.enabled = false;

        _enemy.Manager.EnemyDie.RemoveListener(OnEnemyDie);

        _playerManager.PlayerDie.RemoveListener(OnPlayerDie);
    }

    private void OnPlayerDie()
    {
        _path = null;
        CanMove = false;
        CancelInvoke();

    }

    //reseteo del estado para el object pooling
    public void Reset()
    {
        _collider.enabled = true;
        CanMove = true;
        _currentWaypoint = 0;

        _enemy.Manager.EnemyDie.AddListener(OnEnemyDie);

        _playerManager.PlayerDie.AddListener(OnPlayerDie);

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);

        if (!_playerManager.IsAlive) OnPlayerDie();
    }
}
