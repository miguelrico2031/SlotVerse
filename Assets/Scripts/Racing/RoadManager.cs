using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoadManager : MonoBehaviour
{
    public static RoadManager Instance;

    public UnityEvent<Vector3> ResetCoordinates;

    [SerializeField] private int _initialSpawnedRoads;

    private RoadSpawner _spawner;
    private List<Road> _roads;
    private Road _currentRoad;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);

        _spawner = GetComponent<RoadSpawner>();
        _roads = new List<Road>();
        _currentRoad = null;
    }

    private void Start()
    {
        SpawnInitialRoads();
    }

    private void SpawnInitialRoads()
    {
        //Generates the starting track, destroying first the road (if it exists [debugging reasons])
        foreach(var r in _roads) Destroy(r.gameObject);
        _roads = new List<Road>();
        _currentRoad = null;

        for (int i = 0; i < _initialSpawnedRoads; i++) SpawnNextRoad();

        _currentRoad = _roads[0];
    }

    private void Update()
    {
        //Debugging "cheat codes"
        if (Input.GetKeyDown(KeyCode.T)) SpawnInitialRoads();
        if (Input.GetKeyDown(KeyCode.Y)) OnRoadEnter(_roads[2]);

    }

    private void OnRoadEnter(Road road)
    {
        if (road == _currentRoad) return;

        _currentRoad = road;

        int newRoadIndex = _roads.IndexOf(road);

        //Selects the road tile located 2 tiles behind the actual player position
        //Using an arbitrary value of 2 to avoid removing the tile the same instant it is exited, thus giving the player
        //and camera room to leave it behind and avoid visual issues (or falling down to the void)
        if (newRoadIndex - 2 < 0) return;

        road.RoadEnter.RemoveListener(OnRoadEnter);

        DestroyFirstRoad();

        SpawnNextRoad();
    }

    private Road SpawnNextRoad()
    {
        var road = _spawner.SpawnTile();
        _roads.Add(road);

        road.RoadEnter.AddListener(OnRoadEnter);

        //Check distance to origin

        if(road.transform.position.magnitude > 600f) ResetTilesCoordinates(-road.transform.position);

        return road;
    }

    private void DestroyFirstRoad()
    {
        var firstRoad = _roads[0];
        _roads.RemoveAt(0);
        Destroy(firstRoad.gameObject);
    }

    private void ResetTilesCoordinates(Vector3 pos)
    {
        Debug.Log("resetau");
        foreach (var r in _roads) r.transform.Translate(pos);
        _spawner.ResetSpawnPosition();

        ResetCoordinates.Invoke(pos);
    }
}
