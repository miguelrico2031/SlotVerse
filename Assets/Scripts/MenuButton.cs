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

    //private bool actionPending = false;
    //[SerializeField] private float delayBeforeAction = 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    //al pulsarse
    private void OnMouseDown()
    {
        /*
        if (!actionPending)
        {
            _collider.enabled = false; //desactivar collider para que el botón no se pueda volver a presionar
            _animator.SetBool("Press", true); //activar animación de ser pulsado
            actionPending = true; //hay una acción pendiente

            //iniciar una corrutina para agregar un retraso antes de realizar la acción
            StartCoroutine(DelayedButtonAction());
        }
        */

        _collider.enabled = false; //se desactiva el collider por tanto el boton
        _animator.SetBool("Press", true); //se activa la animacion de ser pulsado

        Pressed.Invoke(this); //se invoca el evento

        Invoke(nameof(ResetAnimation), 0.1f); //se resetea la animacion
        
    }

    /*
    //corrutina
    private IEnumerator DelayedButtonAction()
    {
        yield return new WaitForSeconds(delayBeforeAction);

        Pressed.Invoke(this); //se invoca el evento
        ResetAnimation(); //se resetea la animación
        actionPending = false; //marcar que no hay una acción pendiente
    }
    */

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
