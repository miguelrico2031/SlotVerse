using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShooterPlayerMovement : MonoBehaviour
{
    public UnityEvent<Vector2> DirectionChanged; //Evento que se invoca cuando cambia de dirección

    [SerializeField] private float _moveSpeed; //velocidad de movimiento en unidades /segundo

    private Rigidbody2D _rb; //rigidbody del jugador

    private Vector2 _movementInput; //vector para guardar los ejes horizontal y vertical normalizados     
    private Vector2 _direction; //dirección no normalizada donde mira el jugador

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _movementInput = new Vector2();
        _direction = Vector2.right;
    }

    private void Update()
    {
        //leer el input de movimiento y guardarlo para que lo lea el fixed update
        _movementInput.x = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        _movementInput.y = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        //actualizar la dirección e invocar el evento cuando se cambia de dirección
        if (_movementInput.magnitude > 0f && _movementInput != _direction)
        {
            _direction = _movementInput;
            DirectionChanged?.Invoke(_direction);
        }
    }

    private void FixedUpdate()
    {
        //mover el rigidbody del jugador segun el input y la velocidad
        _rb.velocity = _movementInput.normalized * _moveSpeed * Time.fixedDeltaTime;
    }
}
