using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarMovement : MonoBehaviour
{

    //Car movement variables

    public bool pause = false;

    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxTurnSpeed;
    [SerializeField] private float _bounceSpeed;
    [SerializeField] private float _straightenForce;

    private int _turnInput;
    [SerializeField] private float _turnRatio;

    private bool _isBouncing = false;
    private Rigidbody _rb;
    private CarManager _carManager;

    private AudioSource _audioSource;
    [SerializeField] AudioClip _skiddingSound;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _carManager = GetComponent<CarManager>();

        //audiosource
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        RoadManager.Instance.ResetCoordinates.AddListener(OnResetCoordinates);
        _turnRatio = (_angularSpeed / _speed) / 5; //He cambiado tanto las fisicas que voy a poner numeros al azar
    }


    private void Update()
    {
        if (!_carManager.IsAlive) return;
        //Get turn input
        _turnInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) _audioSource.PlayOneShot(_skiddingSound);
    }

    private void FixedUpdate()
    {
        if (!pause)
        {
            //Add fixed linear velocity
            if(!_isBouncing) _rb.velocity = transform.forward * _speed * Time.fixedDeltaTime;

            //Add turning speed
            if (_turnInput != 0)
            {
                _rb.AddTorque(Vector3.up * _turnInput * _angularSpeed * Time.fixedDeltaTime);
            }

            _carManager.SetX(_turnInput);

            //Add acceleration to speed value
            _speed = Mathf.Min(_speed + _acceleration * Time.fixedDeltaTime, _maxSpeed);

            _angularSpeed = Mathf.Min(_angularSpeed + _turnRatio * Time.fixedDeltaTime, _maxTurnSpeed);
        }
    }

    private void OnResetCoordinates(Vector3 pos)
    {
        //Resets the car coordinates to origin
        transform.Translate(pos, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall") 
        {
            _carManager.TakeDamage(_carManager.DamageByWall);

            if (!_carManager.IsAlive) return;

            Vector3 targetDirection = Vector3.zero;
            var roadData = collision.gameObject.GetComponentInParent<Road>();

            switch (roadData.RoadTileData.ExitDirection)
            {
                case RoadTileData.Direction.North:
                    targetDirection = Vector3.forward;
                    break;
                case RoadTileData.Direction.East:
                    targetDirection = Vector3.right;
                    break;
                case RoadTileData.Direction.West:
                    targetDirection = Vector3.left;
                    break;
            }
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            Vector3 bounceDirection = -transform.forward;

            float angle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);

            _rb.AddTorque(Vector3.up * angle * _straightenForce, ForceMode.Impulse);

            Bounce(bounceDirection);
        }
    }

    public void Bounce(Vector3 bounceDirection)
    {
        //audioSource
        _audioSource.PlayOneShot(_skiddingSound);

        _rb.AddForce(bounceDirection * _bounceSpeed, ForceMode.Impulse);
            _isBouncing = true;
            Invoke(nameof(StopBounce), .3f);
    }

    private void StopBounce() { _isBouncing = false; }

    public void SlowDown(float speedToSlow)
    {
        _speed = _speed - speedToSlow;
    }

    public void StopCar()
    {
        _speed = 0;
        _acceleration = 0;
        _angularSpeed = 0;
    }

    private void OnDisable() => RoadManager.Instance.ResetCoordinates.RemoveListener(OnResetCoordinates);
}