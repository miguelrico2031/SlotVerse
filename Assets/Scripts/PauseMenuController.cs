using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private MenuButton _resumeButton, _homeButton;
    [SerializeField] private GameObject _pauseMenuPanel;

    private bool isPaused = false; //variable que indica si el juego está pausado 

    void Start()
    {
        //suscribirse a ambos botones que van a ser manejados por este script
        _resumeButton.Pressed.AddListener(TogglePause);
        _homeButton.Pressed.AddListener(HomeButtonPressed);

        //aseguramos que el panel este oculto al entrar a cualquier minijuego
        _pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        //Pulsando esc nos aparece el panel 
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    //funcion para pausar o reanudar el juego
    private void TogglePause(MenuButton button = null)
    {
        isPaused = !isPaused;

        //pausar juego y mostrar panel
        if (isPaused)
        {
            Time.timeScale = 0f;
            _pauseMenuPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            _pauseMenuPanel.SetActive(false);
        }
    }

    //funcion para reanudar el juego
    private void HomeButtonPressed(MenuButton button)
    {
        SceneManager.LoadScene("Menu");
    }
}

