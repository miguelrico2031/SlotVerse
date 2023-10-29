using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingBullet : MonoBehaviour
{
    [SerializeField] private int _bulletDamage = 20;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void FireBullet(float bulletSpeed)
    {
        _rb.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarManager>(out var carManager)) carManager.TakeDamage(_bulletDamage);

        Destroy(gameObject);
    }

}
