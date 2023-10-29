using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System.Security.Cryptography;

//clase de todos los botones fisicos (3D) de la tragaperras
//al pulsarse el boton este queda desactivado y se tiene que activar externamente con EnableButton()
public class MenuButton : MonoBehaviour
{
    public UnityEvent<MenuButton> Pressed; //evento invocado al pulsar el boton

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonSound;

    protected Animator _animator;
    protected Collider _collider;

    [SerializeField] private float delayBeforeAction = 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();

        _audioSource = GetComponent<AudioSource>();
    }

    //al pulsarse
    private void OnMouseDown()
    {
        _audioSource.PlayOneShot(_buttonSound);
        _collider.enabled = false; //se desactiva el collider por tanto el boton
        _animator.SetBool("Press", true); //se activa la animacion de ser pulsado

        StartCoroutine(DelayedButtonAction());

        //Pressed.Invoke(this); //se invoca el evento

        //Invoke(nameof(ResetAnimation), 0.1f); //se resetea la animacion
    }

    public void ManualPress()
    {
        _audioSource.PlayOneShot(_buttonSound);
        _collider.enabled = false; //se desactiva el collider por tanto el boton
        _animator.SetBool("Press", true); //se activa la animacion de ser pulsado

        StartCoroutine(DelayedButtonAction());
    }

    //corrutina
    private IEnumerator DelayedButtonAction()
    {
        yield return new WaitForSecondsRealtime(delayBeforeAction);

        Pressed.Invoke(this); //se invoca el evento
        ResetAnimation(); //se resetea la animación
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
