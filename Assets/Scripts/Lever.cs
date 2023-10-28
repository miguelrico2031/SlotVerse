using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

//clase que controla la palanca de la tragaperras, al pulsarla giran los carriles
public class Lever : MonoBehaviour
{
    public UnityEvent LeverPulled; //evento que se llama cuando es pulsada

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _leverSound;

    private Animator _animator;
    private Collider _collider;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();

        _audioSource = GetComponent<AudioSource>();
    }

    //cuando se pulsa con el raton sobre ella, se desactiva el collider para no ser pulsada mas,
    //se ativa la animacion de pulsarse y se invoca el evento
    private void OnMouseDown()
    {
        _collider.enabled = false;
        _animator.SetTrigger("Play");
        
        _audioSource.PlayOneShot(_leverSound);

        LeverPulled.Invoke();
    }

    //metodos publicos para desactivar y activar la palanca cuando corresponda
    public void EnableLever() => _collider.enabled = true;
    public void DisableLever() => _collider.enabled = false;
}
