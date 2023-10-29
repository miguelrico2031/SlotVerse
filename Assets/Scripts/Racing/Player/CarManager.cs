using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CarManager : MonoBehaviour, IPlayerManager
{
    public UnityEvent PlayerDamaged;
    public UnityEvent PlayerDie;

    public Road CurrentRoad;
    public int DamageByWall = 20;

    public int Health { get { return _currentHealth; } }
    public bool IsAlive { get; private set; }

    [SerializeField] private RacingPlayerStats _playerStats;
    [SerializeField] private int _currentHealth;

    [SerializeField] private Animator _explosionAnimator;

    [SerializeField] private GameInfo _gameInfo;

    [SerializeField] private GameObject _futuristicPrefab;
    [SerializeField] private GameObject _halloweenPrefab;
    [SerializeField] private GameObject _beachPrefab;

    [SerializeField] private RuntimeAnimatorController _futuristicAnim;
    [SerializeField] private RuntimeAnimatorController _halloweenAnim;
    [SerializeField] private RuntimeAnimatorController _beachAnim;

    private CarMovement _carMovement;
    private Animator _animator;
    private int x;


    private void Awake()
    {
        IsAlive = true;
        _carMovement = GetComponent<CarMovement>();
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

        _currentHealth = _playerStats.Health;
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetInteger("x", x);

        //oncollision
    }


    //chocamos melee con un enemigo
    public void PlayerHit(IRacingEnemy enemy)
    {
        TakeDamage(enemy.GetDamage());

        if(enemy.GetGameObject().TryGetComponent<SquirrelBehaviour>(out var s))
        {
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        } 
        else if (enemy.GetGameObject().TryGetComponent<MonkeyBehaviour>(out var m))
        {
            _carMovement.Bounce(-_carMovement.transform.forward);
        }
        else if (enemy.GetGameObject().TryGetComponent<HedgehogBehaviour>(out var h))
        {
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            
            if(IsAlive) _carMovement.SlowDown(h.SlowPlayerSpeed);
        }

    }

    public void SetX(int value) { x = value; }

    public void TakeDamage(int value) {

        _currentHealth -=  value;

        PlayerDamaged.Invoke();

        if (_currentHealth <= 0) Die();
        else if(value > 0)
        {
            _explosionAnimator.SetBool("Damage", true);
            Invoke(nameof(OnFinishDamageAnimation), .1f);
        }
    }

    private void Die()
    {
        IsAlive = false;
        _carMovement.StopCar();

        _explosionAnimator.SetTrigger("Die");
        Invoke(nameof(FinishDeath), 2f);
    }

    public void FinishDeath() => PlayerDie.Invoke();

    private void OnFinishDamageAnimation()
    {
        _explosionAnimator.SetBool("Damage", false);
    }

    public int GetCurrentHealth() => Health;
}
