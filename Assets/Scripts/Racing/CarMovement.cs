using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    //Car movement variables
    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxTurnSpeed;

    private int _turnInput;
    [SerializeField] private float _turnRatio;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        RoadManager.Instance.ResetCoordinates.AddListener(OnResetCoordinates);
        _turnRatio = _angularSpeed / _speed;
    }


    private void Update()
    {
        //Get turn input
        _turnInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
    }

    private void FixedUpdate()
    {

        //Add fixed linear velocity
        _rb.velocity = transform.up * _speed * Time.fixedDeltaTime;

        //Add turning speed
        if (_turnInput != 0)
        {
            _rb.AddTorque(Vector3.up * _turnInput * _angularSpeed * Time.fixedDeltaTime);
        }

        //Add acceleration to speed value
        _speed = Mathf.Min(_speed + _acceleration * Time.fixedDeltaTime, _maxSpeed);
        
        _angularSpeed = Mathf.Min(_angularSpeed + _turnRatio * Time.fixedDeltaTime, _maxTurnSpeed);
    }

    private void OnResetCoordinates(Vector3 pos)
    {
        //Resets the car coordinates to origin
        transform.Translate(pos, Space.World);
    }

    private void OnDisable() => RoadManager.Instance.ResetCoordinates.RemoveListener(OnResetCoordinates);
}