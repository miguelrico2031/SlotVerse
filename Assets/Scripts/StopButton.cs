using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StopButton : MonoBehaviour
{
    public UnityEvent Pressed;

    private Animator _animator;
    private Collider _collider;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    private void OnMouseDown()
    {
        _collider.enabled = false;
        _animator.SetBool("Press", true);

        Pressed.Invoke();

        Invoke(nameof(ResetAnimation), 0.1f);
    }

    private void ResetAnimation() => _animator.SetBool("Press", false);

    public void EnableButton(bool e)
    {
        _collider.enabled = e;
    }
}
