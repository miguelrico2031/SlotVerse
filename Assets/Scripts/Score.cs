using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _elapsedTime = 0;

    private float _updateTimer = 0f;

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
}
