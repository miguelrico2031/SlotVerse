using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//script que lanza un evento cuando un target del enemigo entra en el rango de ataque/melee
public class ShooterEnemyRangeTrigger : MonoBehaviour
{
    [HideInInspector] public UnityEvent<ISEnemyTarget> TargetAtRange;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Accede al script que implementa la interfaz target (si lo hay)
        if (!collider.TryGetComponent<ISEnemyTarget>(out var target)) return;

        TargetAtRange.Invoke(target);
    }
}
