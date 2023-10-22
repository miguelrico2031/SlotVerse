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

    private int _turnInput;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        RoadManager.Instance.ResetCoordinates.AddListener(OnResetCoordinates);
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
    }

    private void OnResetCoordinates(Vector3 pos)
    {
        //Resets the car coordinates to origin
        transform.Translate(pos, Space.World);
    }

    private void OnDisable() => RoadManager.Instance.ResetCoordinates.RemoveListener(OnResetCoordinates);
}