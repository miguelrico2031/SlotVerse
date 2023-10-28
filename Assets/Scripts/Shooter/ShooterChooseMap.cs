using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterChooseMap : MonoBehaviour
{
    [SerializeField] private GameInfo _gameInfo;
    [SerializeField] private GameObject[] _beachMap;
    [SerializeField] private GameObject[] _halloweenMap;
    [SerializeField] private GameObject[] _futuristicMap;

    private GameObject _map;

    private void Awake()
    {
        switch (_gameInfo.Setting)
        {
            case Setting.Beach: _map = _beachMap[Random.Range(0, _beachMap.Length)]; break;
            case Setting.Halloween: _map = _halloweenMap[Random.Range(0, _halloweenMap.Length)]; break;
            case Setting.Futuristic: _map = _futuristicMap[Random.Range(0, _futuristicMap.Length)]; break;
        }

        _map.SetActive(true);
        AstarPath.active.Scan();
    }
}
