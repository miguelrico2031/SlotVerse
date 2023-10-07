using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform _firePoint; //punto desde el que se instancian las balas
    [SerializeField] private Bullet _bulletPrefab; //prefab para instanciar balas

    private ShooterPlayerMovement _playerMovement; //referencia al script de movimiento

    private Vector2 _currentDirection; //ultima direccion (para calcular el angulo de rotacion)

    private void Awake()
    {
        _playerMovement = GetComponent<ShooterPlayerMovement>();

        _playerMovement.DirectionChanged?.AddListener(OnDirectionChanged);

        _currentDirection = Vector2.right;
    }

    private void Update()
    {
        //Codigo temporal para disparar balas
        if(Input.GetKeyDown(KeyCode.Space)) CreateBullet();
    }

    void OnDirectionChanged(Vector2 newDirection)
    {
        //calcular el angulo entre la ultima y la nueva direccion
        float angle = Vector2.SignedAngle(_currentDirection, newDirection);
        //rotar el fire point alrededor del centro del personaje segun el angulo
        _firePoint.RotateAround(transform.position, Vector3.forward, angle);

        _currentDirection = newDirection; //actualizar la ultima direccion
    }

    private Bullet CreateBullet()
    {
        //instancia una bala en la posicion del firepoint
        Bullet bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

        bullet.FireBullet(_currentDirection); //dispara la bala asignandole la direccion actual
        return bullet;
    }

    private void OnDisable()
    {
        _playerMovement?.DirectionChanged?.RemoveListener(OnDirectionChanged);
    }


}
