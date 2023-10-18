using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase obstaculo para ambos obstaculo destructible e indestructible
[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour, IPlayerBulletTarget, IEnemyBulletTarget
{
    [SerializeField] private bool _destructible; //indica si se puede destruir
    [SerializeField] private int _health; //la salud del ostaculo (si es destructible, sino es irrelevante)

    public void Hit(PlayerAttackInfo attackInfo) => ReceiveHit(attackInfo.Bullet);

    public void Hit(EnemyAttackInfo attackInfo) => ReceiveHit(attackInfo.Bullet);

    private void ReceiveHit(ShooterBullet bullet)
    {
        //si es indestructible no hace nada mas que destruir la bala que le impacta
        if (!_destructible)
        {
            bullet.DestroyBullet();
            return;
        }

        //si es destructible pierde salud, y se destruye al quedar sin salud
        _health -= bullet.Damage;

        bullet.DestroyBullet();

        if (_health <= 0) Destroy(gameObject);
    }
}
