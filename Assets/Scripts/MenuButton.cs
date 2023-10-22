using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public UnityEvent<MenuButton> Pressed;

    protected Animator _animator;
    protected Collider _collider;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    private void OnMouseDown()
    {
        _collider.enabled = false;
        _animator.SetBool("Press", true);

        Pressed.Invoke(this);

        Invoke(nameof(ResetAnimation), 0.1f);
    }

    private void ResetAnimation()
    {
        _animator.SetBool("Press", false);
    }

    public void EnableButton()
    {
        if (_collider.enabled) return;

        _collider.enabled = true;
    }

    public void DisableButton()
    {
        if (!_collider.enabled) return;

        _collider.enabled = false;
    }
}
