using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SquirrelBehaviour : MonoBehaviour, IRacingEnemy
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootSound;

    enum States
    {
        Walk,
        Shoot,
        Die
    }

    [SerializeField] private RacingBullet _bullet;
    [SerializeField] private float _walkSpeed = 45;
    [SerializeField] private float _minShootTime = 0.5f;
    [SerializeField] private float _maxShootTime = 2.5f;
    [SerializeField] private float _startingMinShootTime = 1.75f;
    [SerializeField] private float _shootSpeedIncrease = 0.00025f;
    [SerializeField] private float _bulletSpeed = 8.0f;
    [SerializeField] private float _bulletOffset = 1.0f;
    [SerializeField] private float _destructionTime = 5.0f;

    private Vector3 _direction, _forward;
    private States _currentState;
    private Rigidbody _rb;
    private Rigidbody _bulletRb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Collider _collider;
    [SerializeField] private float _currentShootTime;

    void Awake()
    {
        _direction = -transform.right;
        _forward = transform.forward;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();

        _currentShootTime = _startingMinShootTime;

        SelectRandomWalkDirection();

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        switch (_currentState)
        {
            case States.Walk:
                _rb.velocity = _direction * _walkSpeed * Time.fixedDeltaTime;
                break;
            case States.Shoot:
                break;
            case States.Die:
                break;
        }

        var direction = Camera.main.transform.forward;
        direction.y = 0;
        _rb.rotation = Quaternion.LookRotation(direction, Vector3.up);

        _currentShootTime = Mathf.Max(_minShootTime, _currentShootTime);

        if (_currentShootTime > _minShootTime) _currentShootTime -= _shootSpeedIncrease;
        if (_maxShootTime > _startingMinShootTime) _maxShootTime -= _shootSpeedIncrease;
    }

    private IEnumerator WalkForRandomSecondsThenShoot()
    {
        float secondsToWait = Random.Range(_currentShootTime, _maxShootTime);
        yield return new WaitForSeconds(secondsToWait);

        ChangeState(States.Shoot);
    }

    private void ChangeState(States newState)
    {
        switch (newState)
        {
            case States.Walk:
                _animator.SetBool("Shooting", false);
                StartCoroutine(WalkForRandomSecondsThenShoot());
                break;
            case States.Shoot:
                _rb.velocity = Vector3.zero;
                _animator.SetBool("Shooting", true);
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

    private void OnShoot()
    {
        //audioSource
        _audioSource.PlayOneShot(_shootSound);

        var bullet =
            Instantiate(_bullet, transform.position - _forward * _bulletOffset, Quaternion.LookRotation(-_forward));
        bullet.FireBullet(_bulletSpeed);
    }

    private void OnAnimationExit()
    {
        SelectRandomWalkDirection();
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if (_currentState == States.Die) return;

        if (collision.gameObject.tag == "Wall")
        {
            _spriteRenderer.flipX = !(_spriteRenderer.flipX);
            _direction *= -1;
        }

        if (collision.gameObject.TryGetComponent<CarManager>(out var carManager))
        {
            ChangeState(States.Die);
            carManager.PlayerHit(this);
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

    public int GetDamage() => 0; //daño al jugador al chocar con el
    public GameObject GetGameObject() => gameObject;
}
