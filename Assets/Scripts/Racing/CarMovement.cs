using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    
    private int _turnInput;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        RoadManager.Instance.ResetCoords.AddListener(OnResetCoords);
    }


    private void Update()
    {
        _turnInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
    }

    private void FixedUpdate()
    {

        //añadir velocidad lineal constante 
        _rb.velocity = transform.up * _speed * Time.fixedDeltaTime;

        //añadir velocidad angular cuando gire
        if(_turnInput != 0)
        {
            _rb.AddTorque(Vector3.up * _turnInput * _angularSpeed * Time.fixedDeltaTime);
        }

        //añadimos la aceleracion a la velocidad
        _speed = Mathf.Min(_speed + _acceleration * Time.fixedDeltaTime, _maxSpeed);
    }

    private void OnResetCoords(Vector3 pos)
    {
        transform.Translate(pos, Space.World);
    }

    private void OnDisable() => RoadManager.Instance.ResetCoords.RemoveListener(OnResetCoords);
    
}
