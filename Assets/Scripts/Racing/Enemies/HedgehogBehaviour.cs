using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class HedgehogBehaviour : MonoBehaviour, IRacingEnemy
{
    private AudioSource _audioSource;
    [SerializeField] AudioClip _shootSound;

    enum States
    {
        Walk,
        Die
    }

    public float SlowPlayerSpeed = 25;

    [SerializeField] private float _walkSpeed = 75;
    [SerializeField] private float _minWalkBoost = 40;
    [SerializeField] private int _maxWalkBoost = 65;
    [SerializeField] private int _spikeDamage = 35;
    [SerializeField] private float _destructionTime = 5.0f;

    private Vector3 _direction;
    private float _walkBoost;
    private States _currentState;
    private Rigidbody _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Collider _collider;

    void Awake()
    {
        _direction = transform.right;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider>();

        SelectRandomWalkDirection();

        _walkBoost = Random.Range(_minWalkBoost, _maxWalkBoost);

        //audioSource
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (_currentState)
        {
            case States.Walk:
                _rb.velocity = _direction * (_walkSpeed + _walkBoost) * Time.fixedDeltaTime;
                break;
            case States.Die:
                break;
        }

        var direction = Camera.main.transform.forward;
        direction.y = 0;
        _rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private void ChangeState(States newState)
    {
        switch (newState)
        {
            case States.Walk:
                break;
            case States.Die:
                _collider.enabled = false;
                _rb.velocity = Vector3.zero;
                _animator.SetTrigger("Dead");

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

        if (collision.collider.TryGetComponent<CarManager>(out var carManager))
        {
            //audioSource
            _audioSource.PlayOneShot(_shootSound);

            carManager.PlayerHit(this);
            ChangeState(States.Die);
        }
    }

    private void SelectRandomWalkDirection()
    {
        float walkRandomDir = Random.value;
        if (walkRandomDir <= 0.5)
        {
            ChangeState(States.Walk);
        }
        else
        {
            _direction *= -1;
            _spriteRenderer.flipX = !(_spriteRenderer.flipX);
            ChangeState(States.Walk);
        }
    }

    private void DestroyThisGameObject() { Destroy(this); }

    public int GetDamage() => _spikeDamage; //daño al jugador al chocaar con el
    public GameObject GetGameObject() => gameObject;
}