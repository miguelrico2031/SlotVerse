using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    public Slider VolumeSlider;
    public const string MusicVolumeKey = "MusicVolume";//clave para cambiar el volumen

    //valor inicial del slider
    void Awake()
    {
        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);

        //se configura el slider y el volumen inicial
        VolumeSlider.value = savedVolume;  
        UpdateVolume(savedVolume);
    }
    private void OnVolumeChanged(float volume)
    {
        UpdateVolume(volume);

        //seteamos el volumen en el playerpref
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        //PlayerPrefs.Save();
    }

    public void UpdateVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
