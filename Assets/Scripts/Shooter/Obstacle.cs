using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour, ITarget
{
    [SerializeField] private bool _destructible;
    [SerializeField] private int _health;

    public void Hit(Bullet bullet)
    {
        if (!_destructible)
        {
            bullet.DestroyBullet();
            return;
        }
        
        
        _health -= bullet.GetDamage();

        bullet.DestroyBullet();

        if (_health <= 0) Destroy(gameObject);
    }
}
