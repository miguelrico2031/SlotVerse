using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    public UnityEvent LeverPulled;


    private Animator _animator;
    private Collider _collider;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    private void OnMouseDown()
    {
        _collider.enabled = false;
        _animator.SetTrigger("Play");

        LeverPulled.Invoke();
    }
}
