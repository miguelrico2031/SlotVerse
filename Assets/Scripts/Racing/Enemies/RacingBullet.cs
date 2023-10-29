using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingBullet : MonoBehaviour
{
    [SerializeField] private int _bulletDamage = 1;
    [SerializeField] private float _destructionTime = 10f;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var direction = -Camera.main.transform.forward;
        direction.y = 0;
        _rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public void FireBullet(float bulletSpeed)
    {
        _rb.velocity = transform.forward * bulletSpeed;
        StartCoroutine(WaitAndDestroy(_destructionTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarManager>(out var carManager)) carManager.TakeDamage(_bulletDamage);

        Destroy(gameObject);
    }

    private IEnumerator WaitAndDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }

}
