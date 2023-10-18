using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTag : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with" + collision.gameObject.name);
    }
}
