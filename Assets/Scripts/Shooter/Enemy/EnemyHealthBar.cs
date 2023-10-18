using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EnemyHealthBar : MonoBehaviour, ISpawnableEnemy
{
    [SerializeField] ShooterEnemy _enemy;
    [SerializeField] private Slider _healthBar;

    private void Start()
    {
        _healthBar.maxValue = _enemy.Stats.Health;
        _healthBar.value = _healthBar.maxValue;
        _enemy.Manager.EnemyHit.AddListener(OnHit);
    }

    private void OnHit(PlayerAttackInfo info)
    {
        _healthBar.value = _enemy.Manager.CurrentHealth;
    }

    public void Reset()
    {
        _healthBar.maxValue = _enemy.Stats.Health;
        _healthBar.value = _healthBar.maxValue;
    }
}
