using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase que controla las interacciones de los botones que no tienen que ver con los carriles de
//la tragaperras, como el de salir, el de opciones, creditos, etc
//asi como los eventos y logica de cambiar de menus y tal
public class OptionsController : MonoBehaviour
{
    //referencia a todos los botones de la escena
    [SerializeField] private MenuButton _stopButton, _optionsButton, _quitButton, _creditsButton, _returnButton;
    [SerializeField] private Lever _lever; //palanca que activa los carriles
    [SerializeField] private GameObject _optionsPanel, _creditsPanel; //paneles UI de opciones y creditos

    private Animator _camAnimator; //animator de la camara, para la animacion girar sobre la tragaperras

    private void Awake()
    {
        _camAnimator = Camera.main.GetComponent<Animator>();   
    }

    private void Start()
    {
        //suscribirse a todos los botones menos el de parar, que lo maneja otro script (SlotController)
        _creditsButton.Pressed.AddListener(ToggleCredits);
        _returnButton.Pressed.AddListener(ReturnToSlot);
        _quitButton.Pressed.AddListener(QuitGame);
        _optionsButton.Pressed.AddListener(OptionsScreen);

        //desactivar botones de la parte de atras
        _creditsButton.DisableButton();
        _returnButton.DisableButton();
    }

    //funcion llamada por el evento de pulsar el boton de opciones
    private void OptionsScreen(MenuButton button)
    {
        //se activa la animacion de la camara de moverse atrás de la tragaperras
        _camAnimator.SetBool("Options", true);
        _camAnimator.SetBool("Return", false);

        //se desactiva la palanca y los botones del frente (el de parar ya esta desactivado)
        _lever.DisableLever();
        _optionsButton.DisableButton();
        _quitButton.DisableButton();

        //se activan los botones de atras
        _creditsButton.EnableButton();
        _returnButton.EnableButton();

        //se activa el panel de opciones (y desactiva el de creditos)
        _creditsPanel.SetActive(false);
        _optionsPanel.SetActive(true);
    }

    //funcion llamada por elevento del boton de return
    private void ReturnToSlot(MenuButton button)
    {
        //se activa la animacion de la camara de volver al punto inicial (al frente)
        _camAnimator.SetBool("Options", false);
        _camAnimator.SetBool("Return", true);

        //se desactivan los botones traseros
        _creditsButton.DisableButton();
        _returnButton.DisableButton();

        //se espera un segundo antes de terminar los resets
        Invoke(nameof(ResetToSlotState), 1f);
    }

    private void ResetToSlotState()
    {
        //se resetean los paneles (despues del delay para que no se vean desaparecer en camara)
        _creditsPanel.SetActive(false);
        _optionsPanel.SetActive(false);

        //se activa la palanca y los botones de frente (para que no se puedan pulsar mientras la camara
        //esta en movimiento)
        _lever.EnableLever();
        _optionsButton.EnableButton();
        _quitButton.EnableButton();
    }

    //funcion llamada por el evento del boton de creditos
    //se activa y desactivan creditos y opciones, para que solo haya 1 activo
    //despues de un momento se reactiva el boton de creditos
    private void ToggleCredits(MenuButton button)
    {
        _creditsPanel.SetActive(!_creditsPanel.activeSelf);
        _optionsPanel.SetActive(!_creditsPanel.activeSelf);
        Invoke(nameof(ResetCreditsButton), 0.2f);
    }

    private void ResetCreditsButton() => _creditsButton.EnableButton();


    //funcion llamada por el boton de salir, que sale del juego tras un momento, para ver la animacion del boton
    private void QuitGame(MenuButton button) => Invoke(nameof(Quit), 0.5f);
    private void Quit() => Application.Quit();
}
