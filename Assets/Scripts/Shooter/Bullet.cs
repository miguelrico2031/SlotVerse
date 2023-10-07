using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IMPORTANTE: para usar esta clase se debe instanciar una bala y justo después llamar al método
//FireBullet para asignarle una dirección y una velocidad.

public class Bullet : MonoBehaviour //Clase Monolítica de una bala disparada por el jugador
{

    [SerializeField] private float _speed; //velocidad de la bala
    [SerializeField] private float _damage; //daño que hace la bala a los enemigos

    private Rigidbody2D _rb;
    private Vector2 _direction; //dirección en la que se dispara la bala

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void FireBullet(Vector2 direction)
    {
        _direction = direction;
        _rb.velocity = _direction * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent<ITarget>(out var target)) return;

        target.Hit(this);

    }

    public void DestroyBullet()
    {

    }
}
