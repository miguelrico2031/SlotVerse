using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//clase de todos los botones fisicos (3D) de la tragaperras
//al pulsarse el boton este queda desactivado y se tiene que activar externamente con EnableButton()
public class MenuButton : MonoBehaviour
{
    public UnityEvent<MenuButton> Pressed; //evento invocado al pulsar el boton

    protected Animator _animator;
    protected Collider _collider;

    [SerializeField] private float delayBeforeAction = 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    //al pulsarse
    private void OnMouseDown()
    {
        _collider.enabled = false; //se desactiva el collider por tanto el boton
        _animator.SetBool("Press", true); //se activa la animacion de ser pulsado

        StartCoroutine(DelayedButtonAction());

        //Pressed.Invoke(this); //se invoca el evento

        //Invoke(nameof(ResetAnimation), 0.1f); //se resetea la animacion
    }

    //corrutina
    private IEnumerator DelayedButtonAction()
    {
        yield return new WaitForSecondsRealtime(delayBeforeAction);

        Pressed.Invoke(this); //se invoca el evento
        ResetAnimation(); //se resetea la animaci�n
    }

    private void ResetAnimation() => _animator.SetBool("Press", false);
    

    //metodos publicos para desactivar y activar el boton
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
