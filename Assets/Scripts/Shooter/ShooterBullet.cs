using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//clase abstracta para las balas, de la que hereda la bala del jugador y la de la adilla (enemybullet)
public abstract class ShooterBullet : MonoBehaviour
{
    public UnityEvent<ShooterBullet> BulletDestroyed; //Evento que se invoca cuando la bala se destruye

    public int Damage { get { return _damage; } } //getter del daño

    protected float _speed; //velocidad de la bala
    protected int _damage; //daño que hace la bala a los enemigos
    protected float _lifetime; //segundos antes de que la bala se destruya sola

    protected Rigidbody2D _rb;
    protected Vector2 _direction; //dirección en la que se dispara la bala

    protected virtual void Awake()
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
        OnCollision(collision);
    }

    public void DestroyBullet()
    {
        _rb.velocity = Vector2.zero; //Se le quita la velocidad a la bala
        StopCoroutine(DestroyAfterTime()); //Se detiene la corrutina para no destruir la bala 2 veces
        BulletDestroyed.Invoke(this); //Se invoca el evento 
    }

    //funcion que implementan las clases hijas que se llama al colisionar por esta clase
    protected abstract void OnCollision(Collision2D collision);
}
