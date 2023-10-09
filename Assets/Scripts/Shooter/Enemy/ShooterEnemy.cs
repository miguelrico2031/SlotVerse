using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShooterEnemy : MonoBehaviour
{
    public ShooterEnemyStats Stats { get { return _stats; } }
    public ShooterEnemyManager Manager { get { return _manager; } }

    [SerializeField] private ShooterEnemyStats _stats;

    private ShooterEnemyManager _manager;
    private ShooterEnemyRangeTrigger _rangeTrigger;

    protected virtual void Awake()
    {
        _manager = GetComponent<ShooterEnemyManager>();
        _manager.EnemyDie.AddListener(OnDie);

        _rangeTrigger = GetComponentInChildren<ShooterEnemyRangeTrigger>();
        _rangeTrigger.TargetAtRange.AddListener(OnTargetAtRange);
    }

    protected abstract void OnTargetAtRange(IEnemyTarget target);

    protected virtual void OnDie()
    {

    }
}

