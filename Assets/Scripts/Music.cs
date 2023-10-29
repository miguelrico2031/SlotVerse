using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Music : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _beachMusic, _halloweenMusic, _futuristicMusic;
    [SerializeField] private GameInfo _gameInfo;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        switch (_gameInfo.Setting)
        {
            case Setting.Beach:
                _audioSource.clip = _beachMusic;
                break;

            case Setting.Halloween:
                _audioSource.clip = _halloweenMusic;
                break;
            case Setting.Futuristic:
                _audioSource.clip = _futuristicMusic;
                break;
        }
        _audioSource.Play();
    }

    public void OnPlayerDie() 
    {
        _audioSource.Stop();
    }

}
