using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grass : MonoBehaviour
{
    private Camera _cam;
    private Transform[] _children;
    private void Awake()
    {
        _cam = Camera.main;
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