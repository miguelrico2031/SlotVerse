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
        bullet.DestroyBullet();

        if (!_destructible) return;
        
        
        _health -= bullet.GetDamage();
        if(_health <= 0) Destroy(gameObject);
    }
}
