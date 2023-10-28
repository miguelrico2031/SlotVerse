using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText, _scoreTextDeath;
    

    private int _elapsedTime = 0;

    private float _updateTimer = 0f;

    private void Awake()
    {
        //encontramos el menu de pausa
        var pause = FindObjectOfType<PauseMenuController>();
        pause.GamePaused.AddListener(OnGamePaused);
    }

    void Update()
    {
        _updateTimer += Time.deltaTime;
        if (_updateTimer < 0.5f) return;

        _updateTimer = 0f;

        _elapsedTime = (int)Time.timeSinceLevelLoad; //tiempo transcurrido
        int minutes = _elapsedTime / 60; //cociente del tiempo transcurrido entre 60 al ser los dos ints ya está
        int seconds =_elapsedTime % 60; //módulo de 60 del tiempo transcurrido

        _scoreText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2"); //convertir a string con dos digitos
    }


    private void OnGamePaused(bool isPaused)
    {
        _scoreText.enabled = !isPaused;
    }

    public void OnPlayerDie() //suscribirse en el inspector al evento de muerte de jugador
    {
        var score = _scoreText.text;
        _scoreText.enabled = false;
        _scoreTextDeath.enabled = true;
        _scoreTextDeath.text = score;
    }
}
