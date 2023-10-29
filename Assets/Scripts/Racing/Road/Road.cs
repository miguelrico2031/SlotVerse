using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Road : MonoBehaviour
{

    public Road PreviousRoad;
    public UnityEvent<Road> RoadEnter;
    public RoadTileData RoadTileData;

    //[SerializeField] private int _reverseRoadDamage = 50;

    private bool _playerEntered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent<CarManager>(out var carManager))
        {
            return;
        }

        if (PreviousRoad == null) 
        {
            RoadEnter.Invoke(this);
        }

        else if (carManager.CurrentRoad != PreviousRoad) //el coche va en dir contraria!
        {
            //carManager.TakeDamage(_reverseRoadDamage);
            //Debug.Log("Estás yendo del revés");
        }

        else if (!_playerEntered) RoadEnter.Invoke(this);

       _playerEntered = true;

        carManager.CurrentRoad = this;
    }
}
