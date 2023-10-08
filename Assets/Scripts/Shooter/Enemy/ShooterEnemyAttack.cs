using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase del enemigo que se encarga de atacar a los objetos con la interfaz IEnemyTarget
//usando un trigger collider que representa su área de ataque
public class ShooterEnemyAttack : MonoBehaviour
{
    [SerializeField] private int _damage;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Accede al script que implementa la interfaz target (si lo hay)
        if (!collider.TryGetComponent<IEnemyTarget>(out var target)) return;

        target.Hit(_damage, _rb.position); //llama al metodo hit del target
    }
}
