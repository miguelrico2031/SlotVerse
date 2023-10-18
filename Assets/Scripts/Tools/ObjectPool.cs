using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase modificada a partir de esta: https://learn.unity.com/tutorial/introduction-to-object-pooling
public class ObjectPool : MonoBehaviour
{
    public GameObject ObjectToPool;
    public int AmountToPool;

    [SerializeField] private Transform _parent;
    [SerializeField] private bool _instantiateOnEmptyPool, _initializeExternally;

    private List<GameObject> _pooledObjects;
    
    void Start()
    {
        if(!_initializeExternally) Initialize();
    }

    public void Initialize()
    {
        _pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < AmountToPool; i++)
        {
            if (_parent) tmp = Instantiate(ObjectToPool, _parent);
            else tmp = Instantiate(ObjectToPool);
            tmp.SetActive(false);
            _pooledObjects.Add(tmp);
        }
    }

    //Para obtener un objeto de la pool
    public GameObject GetFromPool(bool returnActive = true)
    {
        for (int i = 0; i < AmountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                if(returnActive) _pooledObjects[i].SetActive(true);
                return _pooledObjects[i];
            }
        }
        if(!_instantiateOnEmptyPool) return null;

        var obj = Instantiate(ObjectToPool);
        _pooledObjects.Add(obj);
        AmountToPool ++;
        return obj;
    }

    //Devolver un objeto a la pool
    public bool ReturnToPool(GameObject pooledObject)
    {
        if (!_pooledObjects.Contains(pooledObject)) return false;

        pooledObject.SetActive(false);
        return true;
    }

    public int SizeOfPool()
    {
        int n = 0;

        foreach(var obj in _pooledObjects) if(!obj.activeInHierarchy) n++;

        return n;
    }


}
