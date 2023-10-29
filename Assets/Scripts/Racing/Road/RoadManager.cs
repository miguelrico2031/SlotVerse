using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoadManager : MonoBehaviour
{
    public static RoadManager Instance;

    public UnityEvent<Vector3> ResetCoordinates;

    [SerializeField] private int _initialSpawnedRoads;
    [SerializeField] private GameInfo _gameInfo;

    [SerializeField] private Material _futuristicMaterial;
    [SerializeField] private Material _halloweenMaterial;
    [SerializeField] private Material _beachMaterial;

    private Camera _cam;
    private RoadSpawner _spawner;
    private List<Road> _roads;
    private Road _currentRoad;
    private GameObject _floor;


    private void Awake()
    {
        _cam = Camera.main;

        if (!Instance) Instance = this;
        else Destroy(this);

        _spawner = GetComponent<RoadSpawner>();
        _roads = new List<Road>();
        _currentRoad = null;
        _floor = GameObject.FindGameObjectWithTag("Floor");

        switch (_gameInfo.Setting)
        {
            case Setting.Futuristic:
                _cam.backgroundColor = new Color(0.368f, 0.227f, 0.592f);
                _floor.GetComponent<Renderer>().material = _futuristicMaterial;
                break;

            case Setting.Halloween:
                _cam.backgroundColor = new Color(0.368f, 0.290f, 0.372f);
                _floor.GetComponent<Renderer>().material = _halloweenMaterial;
                break;

            case Setting.Beach:
                _cam.backgroundColor = new Color(0.239f, 0.666f, 0.862f);
                _floor.GetComponent<Renderer>().material = _beachMaterial;
                break;
        }
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

        for (int i = 0; i < _initialSpawnedRoads; i++)
        {
            var newRoad = SpawnNextRoad();
            int previousIndex = _roads.Count-2;
            if(previousIndex >= 0)
            {
                newRoad.PreviousRoad = _roads[previousIndex];
            }

            if (_roads.Count < 3) continue;

            var spawner = newRoad.GetComponentInChildren<RacingEnemySpawner>();

            if (spawner != null)
            {
                spawner.SpawnEnemy();
            }
        }

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

        //Selects the road tile located 10 tiles behind the actual player position
        //Using an arbitrary value of 10 to avoid removing the tile the same instant it is exited, thus giving the player
        //and camera room to leave it behind and avoid visual issues (or falling down to the void)
        if (newRoadIndex - 10 < 0) return;

        //road.RoadEnter.RemoveListener(OnRoadEnter);

        DestroyFirstRoad();
        
        Road newRoad = SpawnNextRoad();

        newRoad.PreviousRoad = _roads[newRoadIndex - 1];


        //comprobaar en cual carretera tenemos que spawnear
        int tempIndex = 2;
        //indice hardcodeado + indice de carreter donde esta el jugador = indice de la carretera donde hay q spawnear enemigo(s)
        int spawnIndex = tempIndex + newRoadIndex;

        if (spawnIndex < _roads.Count)
        {
            var spawner = _roads[spawnIndex].GetComponentInChildren<RacingEnemySpawner>();

            if (spawner != null)
            {
                spawner.SpawnEnemy();
            }
        }
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
        foreach (var r in _roads) r.transform.Translate(pos);
        _spawner.ResetSpawnPosition();

        ResetCoordinates.Invoke(pos);
    }
}
