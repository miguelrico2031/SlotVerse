using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]

//Clase Singleton que se encarga del pooling y manejo de las balas
//Sirve como intermediario entre el script de disparar del jugador y los objetos Bullet

public class BulletSpawner : MonoBehaviour 
{
    public static BulletSpawner Instance;

    private ObjectPool _objectPool; //Componente que se encarga del object pooling

    private void Awake()
    {
        Instance = this;
        _objectPool = GetComponent<ObjectPool>();
    }

    public Bullet CreateBullet(Vector2 position, Vector2 direction)
    {
        //crea una bala al sacarla del object pool
        Bullet bullet = _objectPool.GetFromPool()?.GetComponent<Bullet>();

        //asigna la posicion y la dispara con la direccion
        bullet.transform.position = position;
        bullet.FireBullet(direction);

        //este objeto se suscribe al evento de la bala de cuando se destruye
        //Esto para poder devolver la bala a la pool al destruirla en el juego
        bullet.BulletDestroyed?.AddListener(DestroyBullet);
        return bullet;
    }

    public void DestroyBullet(Bullet bullet)
    {
        //se desuscribe del evento de la bala y devuelve la bala a la pool
        bullet.BulletDestroyed?.RemoveListener(DestroyBullet);
        _objectPool.ReturnToPool(bullet.gameObject);
    }
}
