using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemyMonkey : ShooterEnemy
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _collider;
    private ShooterEnemyMovement _movement;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _collider = transform.GetChild(0).GetComponent<CapsuleCollider2D>();
        _movement = GetComponent<ShooterEnemyMovement>();
    }

    protected override void OnTargetAtRange(IEnemyTarget target)
    {
        StartCoroutine(AttackAfterDelay(target));
    }

    private IEnumerator AttackAfterDelay(IEnemyTarget target)
    {
        _movement.CanMove = false;
        _rb.velocity = Vector2.zero;

        float delay = ((ShooterMonkeyStats)Stats).AttackDelay;
        yield return new WaitForSeconds(delay);

        var overlaps = Physics2D.OverlapCapsuleAll
            (_collider.bounds.center, _collider.bounds.size, _collider.direction, 0f);

        foreach (var c in overlaps)
        {
            if (!c.TryGetComponent<IEnemyTarget>(out var t)) continue;
            if(t != target) continue;

            var info = new EnemyAttackInfo()
            {
                IsBullet = false,
                Bullet = null,
                Damage = Stats.Damage,
                Position = _rb.position,
                KnockbackForce = Stats.KnockbackForce,
                KnockbackDuration = Stats.KnockbackDuration
            };

            target.Hit(info);
        }

        _movement.CanMove = true;
    }

}
