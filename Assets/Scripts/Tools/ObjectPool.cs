using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase modificada a partir de esta: https://learn.unity.com/tutorial/introduction-to-object-pooling
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pooledObjects;
    [SerializeField] private GameObject _objectToPool;
    [SerializeField] private int _amountToPool;
    [SerializeField] private Transform _parent;

    void Start()
    {
        _pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < _amountToPool; i++)
        {
            if(_parent) tmp = Instantiate(_objectToPool, _parent);
            else tmp = Instantiate(_objectToPool);
            tmp.SetActive(false);
            _pooledObjects.Add(tmp);
        }
    }

    //Para obtener un objeto de la pool
    public GameObject GetFromPool(bool returnActive = true)
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                if(returnActive) _pooledObjects[i].SetActive(true);
                return _pooledObjects[i];
            }
        }
        return null;
    }

    //Devolver un objeto a la pool
    public bool ReturnToPool(GameObject pooledObject)
    {
        if (!_pooledObjects.Contains(pooledObject)) return false;

        pooledObject.SetActive(false);
        return true;
    }


}
