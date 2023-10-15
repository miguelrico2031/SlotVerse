using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase abstracta del enemigo, cada clase hija se encargará de manejar
//los aspectos y comportamiento únicos de cada enemigo, como atacar entre otros
public abstract class ShooterEnemy : MonoBehaviour
{
    //getter de los stats y el manager, lo usarán los otros scripts de enemigos
    //para que no haya demasiadas cross references
    public ShooterEnemyStats Stats { get { return _stats; } }
    public ShooterEnemyManager Manager { get { return _manager; } }

    [SerializeField] private ShooterEnemyStats _stats;

    private ShooterEnemyManager _manager;

    //Script que detecta cuando un objetivo esta a melee
    private ShooterEnemyRangeTrigger _rangeTrigger;


    protected virtual void Awake()
    {
        _manager = GetComponent<ShooterEnemyManager>();
        _rangeTrigger = GetComponentInChildren<ShooterEnemyRangeTrigger>();

    }

    protected virtual void Start()
    {
        //suscribirse a los eventos de los otros scripts importantes para cada comportamiento
        _manager.EnemyDie.AddListener(OnDie);
        _rangeTrigger.TargetAtRange.AddListener(OnTargetAtRange);
    }

    protected abstract void OnTargetAtRange(IEnemyTarget target);

    protected virtual void OnDie()
    {

    }
}

public struct EnemyAttackInfo
{
    public EnemyBullet Bullet;
    public int Damage;
    public Vector2 Position;
    public float KnockbackForce;
    public float KnockbackDuration;
}

