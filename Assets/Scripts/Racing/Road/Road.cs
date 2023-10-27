using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Road : MonoBehaviour
{

    public UnityEvent<Road> RoadEnter;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car")) RoadEnter.Invoke(this);
    }
}
