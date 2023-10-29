using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//clase que mueve al jugador con el input
public class ShooterPlayerMovement : MonoBehaviour
{
    //Evento que se invoca cuando cambia de dirección
    [HideInInspector] public UnityEvent<Vector2> DirectionChanged;

    private Rigidbody2D _rb; //rigidbody del jugador
    private Collider2D _collider;
    private ShooterPlayerManager _manager; //referencia al manager para eventos de ser golpeado y morir
    private Animator _animator;

    private Vector2 _movementInput; //vector para guardar los ejes horizontal y vertical normalizados     
    private Vector2 _direction; //dirección no normalizada donde mira el jugador
    private bool _canMove = true; //si se puede mover o no

    private int _animatorWalkHash;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _manager = GetComponent<ShooterPlayerManager>();
        _animator = GetComponent<Animator>();
        _animatorWalkHash = Animator.StringToHash("Walk");

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
        _rb.velocity = _movementInput.normalized * _manager.Stats.MoveSpeed * Time.fixedDeltaTime;

        _animator.SetBool(_animatorWalkHash, _rb.velocity.magnitude > 0f);
    }

    //funcion llamada por el evento de jugador golpeado
    private void OnPlayerHit(EnemyAttackInfo attackInfo)
    {
        //cuando lo golpeen no se podrá mover temporalmente, esto se hace para que
        //no se anule la fuerza de retroceso, porque al moverse se sobreescribe la velocidad por completo
        _canMove = false;
        _rb.velocity = Vector2.zero;
        //calcular la direccion de retroceso y aplicar la fuerza de retroceso como impulso
        Vector2 knockbackDirection = (_rb.position - attackInfo.Position).normalized;
        _rb.AddForce(knockbackDirection * attackInfo.KnockbackForce, ForceMode2D.Impulse);

        StartCoroutine(KnockbackTime(attackInfo.KnockbackDuration));
    }

    //esperar y activar el movimiento de nuevo
    private IEnumerator KnockbackTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        if(_manager.IsAlive) _canMove = true;
    }

    //funcion llamada por el evento de muerte
    private void OnPlayerDie()
    {
        _canMove = false;
        _manager.PlayerHit.RemoveListener(OnPlayerHit);
        _collider.enabled = false;
    }
}
