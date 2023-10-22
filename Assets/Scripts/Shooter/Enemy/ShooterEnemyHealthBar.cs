using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//clase que maneja la barra de vida de cada enemigo
public class ShooterEnemyHealthBar : MonoBehaviour, ISSpawnableEnemy
{
    [SerializeField] ShooterEnemy _enemy; //referencia al enemigo
    [SerializeField] private Slider _healthBar; //slider de UI para representar la vida

    private void Start()
    {
        _healthBar.maxValue = _enemy.Stats.Health;
        _healthBar.value = _healthBar.maxValue;
        _enemy.Manager.EnemyHit.AddListener(OnHit); //suscribirse al evento de recibir daño
    }

    private void OnHit(PlayerAttackInfo info)
    {
        _healthBar.value = _enemy.Manager.CurrentHealth; //actualizar la barra de vida
    }

    public void Reset()
    {
        //resetear los valores al maximo
        _healthBar.maxValue = _enemy.Stats.Health;
        _healthBar.value = _healthBar.maxValue;
    }
}
