using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private APlayerStats _playerStats;
    private IPlayerManager _playerManager;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        _playerManager = player.GetComponent<IPlayerManager>();

        // Configurar el valor máximo del slider con la salud máxima del jugador
        _healthSlider.maxValue = _playerStats.GetMaxHealth();

        //para suscribirse al evento GamePaused del PauseMenuController
        var pause = FindObjectOfType<PauseMenuController>();
        pause.GamePaused.AddListener(OnGamePaused);


    }

    public void OnPlayerTakeDamage() //suscribirse a evento de recibir daño jugador en el inspector!!
    {
        //cambiar valor del slider a la vida actual del jugador;
        _healthSlider.value = Mathf.Max(_playerManager.GetCurrentHealth(), 0);
    }


    private void OnGamePaused(bool isPaused)
    {
        _healthSlider.gameObject.SetActive(!isPaused);
    }

    public void OnPlayerDie() //suscvribirse a un evento de muerte del jugador en el inspector!!!
    {
        _healthSlider.gameObject.SetActive(false);
    }
}
