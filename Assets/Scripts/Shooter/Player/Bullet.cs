using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//IMPORTANTE: para usar esta clase se debe instanciar una bala y justo después llamar al método
//FireBullet para asignarle una dirección y una velocidad.

public class Bullet : MonoBehaviour //Clase Monolítica de una bala disparada por el jugador
{
    public UnityEvent<Bullet> BulletDestroyed; //Evento que se invoca cuando la bala se destruye

    public int Damage { get { return _damage; } }

    [SerializeField] private float _speed; //velocidad de la bala
    [SerializeField] private int _damage; //daño que hace la bala a los enemigos
    [SerializeField] private float _lifetime; //segundos antes de que la bala se destruya sola

    private Rigidbody2D _rb;
    private Vector2 _direction; //dirección en la que se dispara la bala

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void FireBullet(Vector2 direction)
    {
        //da una direccion y posicion a la bala
        _direction = direction;
        _rb.velocity = _direction.normalized * _speed;

        //empieza la corrutina para que la bala se destruya sola
        StartCoroutine(DestroyAfterTime());
    }

    //Corrutina para esperar x tiempo y, si la bala no se ha destruido por una colision, destruirla sola
    //(Esto para evitar que haya demasiadas balas activas a la vez en el object pool)
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(_lifetime);
        DestroyBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Comprueba si el objeto colisionado es interactuable con la bala (ITarget)
        if (!collision.collider.TryGetComponent<IBulletTarget>(out var target)) return;

        //metodo implementado por todos los objetos interactuables
        //al final de este método se llamará a DestroyBullet siempre 
        target.Hit(this); 
    }

    public void DestroyBullet()
    {
        _rb.velocity = Vector2.zero; //Se le quita la velocidad a la bala
        StopCoroutine(DestroyAfterTime()); //Se detiene la corrutina para no destruir la bala 2 veces
        BulletDestroyed.Invoke(this); //Se invoca el evento 
    }
}
