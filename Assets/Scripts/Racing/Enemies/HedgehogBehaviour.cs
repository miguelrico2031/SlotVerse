using System.Collections;
using UnityEngine;

public class HedgehogBehaviour : MonoBehaviour
{

    enum States
    {
        Walk,
        Die
    }

    [SerializeField] private float _walkSpeed = 25;
    [SerializeField] private float _destructionTime = 5.0f;

    private Vector3 _direction;
    private States _currentState;
    private Rigidbody _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Collider _collider;

    void Awake()
    {
        _direction = -transform.right;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider>();

        ChangeState(States.Walk);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (_currentState)
        {
            case States.Walk:
                _rb.velocity = _direction * _walkSpeed * Time.fixedDeltaTime;
                break;
            case States.Die:
                break;
        }
    }

    private void ChangeState(States newState)
    {
        switch (newState)
        {
            case States.Walk:
                _animator.SetBool("Shooting", false);
                break;
            case States.Die:
                _rb.velocity = Vector3.zero;
                _animator.SetTrigger("Dead");
                _collider.enabled = false;

                Invoke(nameof(DestroyThisGameObject), _destructionTime);
                break;
        }

        _currentState = newState;
    }

    private void OnAnimationExit()
    {
        ChangeState(States.Walk);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_currentState == States.Die) return;

        if (collision.gameObject.tag == "Wall")
        {
            _spriteRenderer.flipX = !(_spriteRenderer.flipX);
            _direction *= -1;
        }

        if (collision.gameObject.tag == "Player")
        {
            ChangeState(States.Die);
        }
    }

    private void DestroyThisGameObject() { Destroy(this); }
}
