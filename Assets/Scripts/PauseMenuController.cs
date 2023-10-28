using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PauseMenuController : MonoBehaviour
{
    public UnityEvent<bool> GamePaused;

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

        EventSystem e = _pauseMenuPanel.GetComponentInChildren<EventSystem>();
        foreach (EventSystem es in FindObjectsOfType<EventSystem>()) 
        {
            if (es != e) Destroy(es);
        }  
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
            _resumeButton.EnableButton();
        }
        else
        {
            Time.timeScale = 1f;
            _pauseMenuPanel.SetActive(false);
        }

        GamePaused.Invoke(isPaused);
    }

    //funcion para volver al menu
    private void HomeButtonPressed(MenuButton button)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void QuitButtonPressed(MenuButton button)
    {
        Application.Quit();
    }
}

