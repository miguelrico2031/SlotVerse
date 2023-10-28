using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private MenuButton _homeButton, _quitButton;
    [SerializeField] private GameObject _deathMenuPanel;

    void Awake()
    {
        _deathMenuPanel.SetActive(false);
        _homeButton.Pressed.AddListener(HomeButtonPressed);
        _quitButton.Pressed.AddListener(QuitButtonPressed);
    }

    //cambiar de escena al menu principal al pulsar boton de home
    private void HomeButtonPressed(MenuButton button)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    //quitar el juego
    private void QuitButtonPressed(MenuButton button)
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void DeathScreen()
    {
        Invoke(nameof(SetDeathScreen), 0.05f);
    }


    private void SetDeathScreen()
    {
        Time.timeScale = 0f;
        _deathMenuPanel.gameObject.SetActive(true);

        var pause = FindAnyObjectByType<PauseMenuController>();

        if (pause != null) pause.enabled = false;
    }

}
