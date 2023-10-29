using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MonkeyBehaviour : MonoBehaviour, IRacingEnemy
{
    private AudioSource _audioSource;
    [SerializeField] AudioClip _shootSound;

    enum States
    {
        Walk,
        Stop
    }

    [SerializeField] private float _walkSpeed = 15;
    [SerializeField] private float _minStopTime = 3.0f;
    [SerializeField] private float _maxStopTime = 8.0f;

    private Vector3 _direction;
    private States _currentState;
    private Rigidbody _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private int _damageToPlayer = 0;

    void Awake()
    {
        _direction = transform.right;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeState(States.Stop);

        //audioSource
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (_currentState)
        {
            case States.Walk:
                _rb.constraints = ~RigidbodyConstraints.FreezePosition;
                _rb.velocity = _direction * _walkSpeed * Time.fixedDeltaTime;
                break;
            case States.Stop:
                _rb.constraints = RigidbodyConstraints.FreezePosition;
                break;
        }
    }

    private IEnumerator WalkForRandomSecondsThenStop()
    {
        float secondsToWait = Random.Range(_minStopTime, _maxStopTime);
        yield return new WaitForSeconds(secondsToWait);

        ChangeState(States.Stop);
    }

    private void ChangeState(States newState)
    {
        switch (newState)
        {
            case States.Walk:
                _animator.SetBool("Stopped", false);
                StartCoroutine(WalkForRandomSecondsThenStop());
                break;
            case States.Stop:
                _rb.velocity = Vector3.zero;
                _animator.SetBool("Stopped", true);
                break;
        }

        _currentState = newState;
    }

    private void OnAnimationExit()
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("RacingWall"))
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;

            _direction *= -1;
        }

        if (collision.collider.TryGetComponent<CarManager>(out var carManager))
        {
            //audioSource
            _audioSource.PlayOneShot(_shootSound);

            ChangeState(States.Stop);

            _damageToPlayer = carManager.Health;
            carManager.PlayerHit(this);
        }
    }

    public int GetDamage() => _damageToPlayer; //daño al jugador al chocar con el
    public GameObject GetGameObject() => gameObject;
}
