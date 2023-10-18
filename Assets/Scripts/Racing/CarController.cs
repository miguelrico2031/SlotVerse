using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private float speed = 90f;
    [SerializeField]
    private float turnSpeed = 5f;
    [SerializeField]
    private float smoothing = 0.5f;
    [SerializeField]
    private float turnInput;

    private Rigidbody rb;

    private bool accelerating = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetButton("Vertical"))
        {
            accelerating = true;
        }
        else
        {
            accelerating = false;
        }

        turnInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        float tempInput = 0f;

        float smoothedTurn = Mathf.Lerp(turnInput, tempInput, smoothing);

        if (accelerating)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);    
        }

        rb.transform.Rotate(new Vector3(0f, smoothedTurn * turnSpeed, 0f));
    }
}
