using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public Road CurrentRoad;

    public int Health { get { return _currentHealth; } }

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private GameInfo _gameInfo;
    [SerializeField] private GameObject _futuristicPrefab;
    [SerializeField] private GameObject _halloweenPrefab;
    [SerializeField] private GameObject _beachPrefab;

    [SerializeField] private RuntimeAnimatorController _futuristicAnim;
    [SerializeField] private RuntimeAnimatorController _halloweenAnim;
    [SerializeField] private RuntimeAnimatorController _beachAnim;

    private Animator _animator;
    private Animator _explosionAnimator;
    private int x;

    private int _currentHealth;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _explosionAnimator = GetComponentInChildren<Animator>();

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

        _currentHealth = _maxHealth;
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

        if(collision.collider.TryGetComponent<SquirrelBehaviour>(out var s))
        {

            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void setX(int value) { x = value; }

    public void TakeDamage(int value) {

        _explosionAnimator.SetBool("Damage", true);
        _currentHealth -=  value;
        _explosionAnimator.SetBool("Damage", false);

        if (_currentHealth <= 0)
        {
            Debug.Log("Tas matao xaval");
            _explosionAnimator.SetBool("Dead", true);
            //Muerte
        }
    }
}
