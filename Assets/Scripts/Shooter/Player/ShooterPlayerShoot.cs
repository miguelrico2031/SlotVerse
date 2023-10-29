using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayerShoot : MonoBehaviour
{
    //puntos de los que se dispara
    [SerializeField] private Transform _firepointR;
    [SerializeField] private Transform _firepointL;
    [SerializeField] private Transform _firepointU;
    [SerializeField] private Transform _firepointD;
    [SerializeField] private ShooterBulletSpawner _bulletSpawner;

    private ShooterPlayerManager _manager;
    private ShooterPlayerMovement _playerMovement; //referencia al script de movimiento
    private Animator _animator;
    private Transform _firePoint;

    private Vector2 _currentDirection; //ultima direccion (para calcular el angulo de rotacion)

    private void Awake()
    {
        _manager = GetComponent<ShooterPlayerManager>();
        _playerMovement = GetComponent<ShooterPlayerMovement>();
        _animator = GetComponent<Animator>();

        _manager.PlayerDie?.AddListener(OnPlayerDie);
        //añadimos el observador para el evento de cambio de direccion del script de movimiento
        _playerMovement.DirectionChanged?.AddListener(OnDirectionChanged);

        _currentDirection = Vector2.right;
        _firePoint = _firepointR;
    }

    private void Update()
    {
        if (!_manager.IsAlive) return;

        //Codigo temporal para disparar balas
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Creamos una bala con el spawner (object pooling) con la direccion y posicion
            _bulletSpawner.CreateBullet(_firePoint.position, _currentDirection);

            //actualizamos el animator
            _animator.SetBool("Fire", true);
            Invoke(nameof(ResetShootAnimator), 0.1f);
        }
    }

    private void ResetShootAnimator() => _animator.SetBool("Fire", false);

    void OnDirectionChanged(Vector2 newDirection)
    {
        //calcular el angulo entre la ultima y la nueva direccion
        float angle = Vector2.SignedAngle(_currentDirection, newDirection);
        //rotar el fire point alrededor del centro del personaje segun el angulo
        //_firePoint.RotateAround(transform.position, Vector3.forward, angle);

        _currentDirection = newDirection; //actualizar la ultima direccion

        //actualizar el animator
        var normDirection = _currentDirection.normalized;
        _animator.SetFloat("x", normDirection.x);
        _animator.SetFloat("y", normDirection.y);

        _firePoint.gameObject.SetActive(false);
        //settear el firepoint
        if (_currentDirection.x == 1f) _firePoint = _firepointR;
        else if (_currentDirection.x == -1f) _firePoint = _firepointL;
        else if (_currentDirection.y == 1f) _firePoint = _firepointU;
        else if (_currentDirection.y == -1f) _firePoint = _firepointD;

        _firePoint.gameObject.SetActive(true);
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
