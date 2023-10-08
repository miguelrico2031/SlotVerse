using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShooterPlayerMovement : MonoBehaviour
{
    public UnityEvent<Vector2> DirectionChanged; //Evento que se invoca cuando cambia de dirección

    [SerializeField] private float _moveSpeed; //velocidad de movimiento en unidades /segundo
    [SerializeField] private float _knockBackForce; //fuerza de retroceso al ser golpeado
    [SerializeField] private float _knockBackDuration; //segundos en los que no se puede mover al ser golpeado

    private Rigidbody2D _rb; //rigidbody del jugador
    private ShooterPlayerManager _manager; //referencia al manager para eventos de ser golpeado y morir

    private Vector2 _movementInput; //vector para guardar los ejes horizontal y vertical normalizados     
    private Vector2 _direction; //dirección no normalizada donde mira el jugador
    private bool _canMove = true; //si se puede mover o no

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _manager = GetComponent<ShooterPlayerManager>();

        _manager.PlayerHit.AddListener(OnPlayerHit);
        _manager.PlayerDie.AddListener(OnPlayerDie);

        _movementInput = new Vector2();
        _direction = Vector2.right;
    }

    private void Update()
    {
        if (!_canMove) return; //si no se puede mover no hacer nada

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
        if (!_canMove) return; //si no se puede mover no hacer nada

        //mover el rigidbody del jugador segun el input y la velocidad
        _rb.velocity = _movementInput.normalized * _moveSpeed * Time.fixedDeltaTime;
    }

    //funcion llamada por el evento de jugador golpeado
    private void OnPlayerHit(Vector2 enemyPosition)
    {
        //cuando lo golpeen no se podrá mover temporalmente, esto se hace para que
        //no se anule la fuerza de retroceso, porque al moverse se sobreescribe la velocidad por completo
        _canMove = false;

        //calcular la direccion de retroceso y aplicar la fuerza de retroceso como impulso
        Vector2 knockbackDirection = (_rb.position - enemyPosition).normalized;
        _rb.AddForce(knockbackDirection * _knockBackForce, ForceMode2D.Impulse);

        StartCoroutine(KnockbackTime());
    }

    //esperar y activar el movimiento de nuevo
    private IEnumerator KnockbackTime()
    {
        yield return new WaitForSeconds(_knockBackDuration);
        _canMove = true;
    }

    //funcion llamada por el evento de muerte
    private void OnPlayerDie()
    {
        _canMove = false;
    }
}
