using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    public AudioMixer _audioMixer; //asignamos el audio mixer en inspector
    public Slider _slider;

    private string musicVolumeParameter = "MusicVolume"; //nombre parámetro volumen música en AudioMixer
    private string sfxVolumeParameter = "SFXVolume"; //nombre parámetro volumen música sfx

    //valor inicial del slider
    void Start()
    {
        float initialVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        _slider.value = initialVolume;
        SetVolume(initialVolume);
    }

    //ajustar volumen musica y sfx en audio mixer
    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat(musicVolumeParameter, Mathf.Log10(volume) * 20); //convertir a escala logarítimica más propia de aplicaciones de audio
        _audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(volume) * 20);

        //guardamos el valor en el playerprefs para que el juego tenga el volumen deseado por el jugador cada vez que entra al juego de nuevo
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
