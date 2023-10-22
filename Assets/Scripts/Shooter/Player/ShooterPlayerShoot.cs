using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform _firePoint; //punto desde el que se instancian las balas
    [SerializeField] private ShooterBulletSpawner _bulletSpawner;

    private ShooterPlayerManager _manager;
    private ShooterPlayerMovement _playerMovement; //referencia al script de movimiento

    private Vector2 _currentDirection; //ultima direccion (para calcular el angulo de rotacion)

    private void Awake()
    {
        _manager = GetComponent<ShooterPlayerManager>();
        _playerMovement = GetComponent<ShooterPlayerMovement>();

        _manager.PlayerDie?.AddListener(OnPlayerDie);
        //añadimos el observador para el evento de cambio de direccion del script de movimiento
        _playerMovement.DirectionChanged?.AddListener(OnDirectionChanged);

        _currentDirection = Vector2.right;
    }

    private void Update()
    {
        if (!_manager.IsAlive) return;

        //Codigo temporal para disparar balas
        if (Input.GetKeyDown(KeyCode.Space))
            //Creamos una bala con el spawner (object pooling) con la direccion y posicion
            _bulletSpawner.CreateBullet(_firePoint.position, _currentDirection);
    }

    void OnDirectionChanged(Vector2 newDirection)
    {
        //calcular el angulo entre la ultima y la nueva direccion
        float angle = Vector2.SignedAngle(_currentDirection, newDirection);
        //rotar el fire point alrededor del centro del personaje segun el angulo
        _firePoint.RotateAround(transform.position, Vector3.forward, angle);

        _currentDirection = newDirection; //actualizar la ultima direccion
    }

    private void OnPlayerDie()
    {
        _playerMovement?.DirectionChanged?.RemoveListener(OnDirectionChanged);
    }

    private void OnDisable()
    {
        _playerMovement?.DirectionChanged?.RemoveListener(OnDirectionChanged);
    }


}
