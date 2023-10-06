using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform _firePoint; //punto desde el que se instancian las balas

    private ShooterPlayerMovement _playerMovement; //referencia al script de movimiento

    private Vector2 _lastDirection; //ultima direccion (para calcular el angulo de rotacion)

    private void Awake()
    {
        _playerMovement = GetComponent<ShooterPlayerMovement>();

        _playerMovement.DirectionChanged?.AddListener(OnDirectionChanged);

        _lastDirection = Vector2.right;
    }


    void OnDirectionChanged(Vector2 newDirection)
    {
        //calcular el angulo entre la ultima y la nueva direccion
        float angle = Vector2.SignedAngle(_lastDirection, newDirection);
        //rotar el fire point alrededor del centro del personaje segun el angulo
        _firePoint.RotateAround(transform.position, Vector3.forward, angle);

        _lastDirection = newDirection; //actualizar la ultima direccion
    }

    private void OnDisable()
    {
        _playerMovement?.DirectionChanged?.RemoveListener(OnDirectionChanged);
    }


}
