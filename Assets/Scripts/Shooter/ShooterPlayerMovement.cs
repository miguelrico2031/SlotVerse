using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed; //velocidad de movimiento en unidades /segundo

    private Rigidbody2D _rb;

    private Vector2 _movementInput; //vector para guardar los ejes horizontal y vertical normalizados


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _movementInput = new Vector2();
    }

    private void Update()
    {
        //leer el input de movimiento y guardarlo para que lo lea el fixed update
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        _rb.velocity = _movementInput.normalized * _moveSpeed * Time.fixedDeltaTime;
    }
}
