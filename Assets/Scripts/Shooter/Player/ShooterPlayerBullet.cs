using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//clase de las balas del jugador
public class ShooterPlayerBullet : ShooterBullet, ISEnemyBulletTarget
{
    [SerializeField] private ShooterPlayerStats _stats; //referencia a los stats del jugador
    //sprites de las balas en cada ambientacion
    [SerializeField] private Sprite _beachSprite;
    [SerializeField] private Sprite _halloweenSprite;
    [SerializeField] private Sprite _futuristicSprite;
    [SerializeField] private GameInfo _gameInfo;

    protected override void Awake()
    {
        base.Awake();

        _speed = _stats.BulletSpeed;
        _lifetime = _stats.BulletLifetime;
        _damage = _stats.BulletDamage;

        var renderer = GetComponent<SpriteRenderer>();

        switch(_gameInfo.Setting)
        {
            case Setting.Beach: renderer.sprite = _beachSprite; break;
            case Setting.Halloween: renderer.sprite = _halloweenSprite; break;
            case Setting.Futuristic: renderer.sprite = _futuristicSprite; break;
        }
    }

    protected override void OnCollision(Collision2D collision)
    {
        //Comprueba si el objeto colisionado es interactuable con la bala
        if (!collision.collider.TryGetComponent<ISPlayerBulletTarget>(out var target)) return;

        //objeto con toda la informacion del ataque que vaya a necesitar el impactado
        var info = new PlayerAttackInfo()
        {
            Bullet = this,
            Damage = _damage,
            Position = _rb.position,
            KnockbackForce = _stats.BulletKnockbackForce,
            KnockbackDuration = _stats.BulletKnockbackDuration
        };

        //metodo implementado por todos los objetos interactuables
        //al final de este m�todo se llamar� a DestroyBullet siempre
        target.Hit(info); 
    }

    //funcion para destruirse al chocar con una bala enemiga
    public void Hit(EnemyAttackInfo attackInfo)
    {
        DestroyBullet();
    }
}

//estructura para la informacion del ataque del jugador
public struct PlayerAttackInfo
{
    public ShooterPlayerBullet Bullet;
    public int Damage;
    public Vector2 Position;
    public float KnockbackForce;
    public float KnockbackDuration;
}
