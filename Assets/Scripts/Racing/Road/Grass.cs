using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grass : MonoBehaviour
{
    private Transform[] _children;
    private void Awake()
    {
        _children = new Transform[transform.childCount];
        for (int i = 0; i < _children.Length; i++) _children[i] = transform.GetChild(i);
    }

    private void Update()
    {
        Quaternion pointToCamRotation = Quaternion.LookRotation(-Camera.main.transform.forward);
        foreach(var c in _children)
        {
            c.rotation = pointToCamRotation;
        }
    }
}