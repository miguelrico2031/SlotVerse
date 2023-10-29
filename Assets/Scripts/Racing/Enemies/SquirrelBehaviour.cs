using System.Collections;
using UnityEngine;

public class SquirrelBehaviour : MonoBehaviour
{

    enum States
    {
        Walk,
        Shoot,
        Die
    }

    [SerializeField] private RacingBullet _bullet;
    [SerializeField] private float _walkSpeed = 20;
    [SerializeField] private float _minShootTime = 3.0f;
    [SerializeField] private float _maxShootTime = 8.0f;
    [SerializeField] private float _bulletSpeed = 8.0f;
    [SerializeField] private float _bulletOffset = 1.0f;
    [SerializeField] private float _destructionTime = 5.0f;

    private Vector3 _direction;
    private States _currentState;
    private Rigidbody _rb;
    private Rigidbody _bulletRb;
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
            case States.Shoot:
                break;
            case States.Die:
                break;
        }
    }

    private IEnumerator WalkForRandomSecondsThenShoot()
    {
        float secondsToWait = Random.Range(_minShootTime, _maxShootTime);
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
        var bullet =
            Instantiate(_bullet, transform.position - transform.forward * _bulletOffset, Quaternion.LookRotation(-transform.forward));
        bullet.FireBullet(_bulletSpeed);
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
