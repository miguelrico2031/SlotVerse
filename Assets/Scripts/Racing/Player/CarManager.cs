using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CarManager : MonoBehaviour
{


    public Road CurrentRoad;

    [SerializeField] private int Health = 100;
    [SerializeField] private GameInfo _gameInfo;
    [SerializeField] private GameObject _futuristicPrefab;
    [SerializeField] private GameObject _halloweenPrefab;
    [SerializeField] private GameObject _beachPrefab;

    [SerializeField] private RuntimeAnimatorController _futuristicAnim;
    [SerializeField] private RuntimeAnimatorController _halloweenAnim;
    [SerializeField] private RuntimeAnimatorController _beachAnim;

    private Animator _animator;
    private int x;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        switch (_gameInfo.Setting)
        {
            case Setting.Futuristic:
                _animator.runtimeAnimatorController = _futuristicAnim;
                break;

            case Setting.Halloween:
                _animator.runtimeAnimatorController = _halloweenAnim;
                break;

            case Setting.Beach:
                _animator.runtimeAnimatorController = _beachAnim;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetInteger("x", x);

        //oncollision
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Si es un muro resta x
        //Si es enemigo resta enemigo.daño
    }

    public void setX(int value) { x = value; }

    public void TakeDamage(int value) {
        
        Health -=  value; 
        if (Health <= 0)
        {
            //Muerte
        }
    }
}
