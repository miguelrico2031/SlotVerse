using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private MenuButton _homeButton, _quitButton;
    [SerializeField] private GameObject _deathMenuPanel;

    private bool isDeath = false; //variable a referenciar y poner en tru en el gamecontroller una vez muera el personaje

    void Start()
    {
        _homeButton.Pressed.AddListener(HomeButtonPressed);
        _quitButton.Pressed.AddListener(QuitButtonPressed);
    }

    // Update is called once per frame
    void fixedUpdate()
    {
        if (!isDeath)
        {
            _deathMenuPanel.SetActive(true);
        }
    }

    //cambiar de escena al menu principal al pulsar boton de home
    private void HomeButtonPressed(MenuButton button)
    {
        SceneManager.LoadScene("Menu");
    }

    //quitar el juego
    private void QuitButtonPressed(MenuButton button)
    {
        Application.Quit();
    }

}
