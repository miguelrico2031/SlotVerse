using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    public Slider _volumeSlider;
    public const string musicVolumeKey = "MusicVolume";//clave para cambiar el volumen

    //valor inicial del slider
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(musicVolumeKey, 0.5f);

        //se configura el slider y el volumen inicial
        _volumeSlider.value = savedVolume;
        UpdateVolume(savedVolume);
    }
    private void OnVolumeChanged(float volume)
    {
        UpdateVolume(volume);

        //seteamos el volumen en el playerpref
        PlayerPrefs.SetFloat(musicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    private void UpdateVolume(float volume)
    {
        AudioListener.volume = volume;
    }

}
