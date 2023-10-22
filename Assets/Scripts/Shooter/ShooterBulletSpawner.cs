using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]

//Clase que se encarga del pooling y manejo de las balas
//Sirve como intermediario entre el script de disparar del jugador y enemigo y los objetos Bullet

public class ShooterBulletSpawner : MonoBehaviour 
{

    private ObjectPool _objectPool; //Componente que se encarga del object pooling

    private void Awake()
    {
        _objectPool = GetComponent<ObjectPool>();
    }

    public ShooterBullet CreateBullet(Vector2 position, Vector2 direction)
    {
        //crea una bala al sacarla del object pool
        ShooterBullet bullet = _objectPool.GetFromPool()?.GetComponent<ShooterBullet>();

        //asigna la posicion y la dispara con la direccion
        bullet.transform.position = position;
        bullet.FireBullet(direction);

        //este objeto se suscribe al evento de la bala de cuando se destruye
        //Esto para poder devolver la bala a la pool al destruirla en el juego
        bullet.BulletDestroyed?.AddListener(DestroyBullet);
        return bullet;
    }

    private void DestroyBullet(ShooterBullet bullet)
    {
        //se desuscribe del evento de la bala y devuelve la bala a la pool
        bullet.BulletDestroyed?.RemoveListener(DestroyBullet);
        _objectPool.ReturnToPool(bullet.gameObject);
    }
}
