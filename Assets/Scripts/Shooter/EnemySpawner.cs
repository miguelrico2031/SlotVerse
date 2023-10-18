    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameInfo _gameInfo;
    [SerializeField] private ShooterEnemy _hedgehogPrefab, _monkeyPrefab, _squirrelPrefab;
    [SerializeField] private int _initialEnemiesPerWave;
    [SerializeField] private int _enemiesPerWaveIncrement;
    [SerializeField] private float _initialSecondsPerWave;
    [SerializeField] private float _secondsPerWaveIncrement;
    [SerializeField] private Vector2 _minSpawnCoords, _maxSpawnCoords;
    [SerializeField] private LayerMask _ignoreLayersForSpawnOverlap;

    private ObjectPool _objectPool;
    private ShooterPlayerManager _playerManager;

    private int enemiesPerWave = 0;
    private float secondsPerWave = 0;

    private void Awake()
    {
        _objectPool = GetComponent<ObjectPool>();
        _playerManager = FindAnyObjectByType<ShooterPlayerManager>();

        switch(_gameInfo.NPC)
        {
            case NPC.Hedgehog: _objectPool.ObjectToPool = _hedgehogPrefab.gameObject; break;
            case NPC.Monkey: _objectPool.ObjectToPool = _monkeyPrefab.gameObject; break;
            case NPC.Squirrel: _objectPool.ObjectToPool = _squirrelPrefab.gameObject; break;
        }
    }

    private IEnumerator Start()
    {
        _objectPool.Initialize();

        enemiesPerWave = _initialEnemiesPerWave;
        secondsPerWave = _initialSecondsPerWave;

        while(_playerManager.IsAlive)
        { 
            if(_objectPool.SizeOfPool() < enemiesPerWave)
            {
                yield return new WaitForSeconds(secondsPerWave);
                continue;
            }

            for (int i = 0; i < enemiesPerWave; i++)
            {
                Vector2 spawnPosition = GetSpawnPoint();
                var enemy = SpawnEnemy(spawnPosition);
                enemy?.GetComponent<ShooterEnemyManager>().EnemyDie.AddListener(OnEnemyDie);
            }

            enemiesPerWave += _enemiesPerWaveIncrement;
            secondsPerWave += _secondsPerWaveIncrement;

            yield return new WaitForSeconds(secondsPerWave);
        }
    }


    private ShooterEnemy SpawnEnemy(Vector2 position)
    {
        var enemy = _objectPool.GetFromPool(false)?.GetComponent<ShooterEnemy>();
       
        if (enemy == null) return null;

        var resets = enemy.GetComponentsInChildren<ISpawnableEnemy>(true);
        foreach (var reset in resets) reset.Reset();

        enemy.gameObject.SetActive(true);
        enemy.transform.position = position;
        return enemy;
    }

    private Vector2 GetSpawnPoint()
    {
        Vector2 point = Vector2.zero;
        bool isValidPoint = false;

        while(!isValidPoint)
        {
            isValidPoint = true;
            
            point.x = Random.Range(_minSpawnCoords.x, _maxSpawnCoords.x);
            point.y = Random.Range(_minSpawnCoords.y, _maxSpawnCoords.y);

            //comprobar si esta dentro del espacio de la camara (solo spawneamos por fuera de la camara)
            Vector3 camPoint = Camera.main.WorldToViewportPoint(point);
            if(camPoint.x >= 0 && camPoint.x < 1 && camPoint.y >= 0 && camPoint.y < 1)
                isValidPoint = false;

            //comprobar si el punto no estaria colisionando con otro objeto
            else if(Physics2D.OverlapCircle(point, 0.5f, ~_ignoreLayersForSpawnOverlap) != null)
                isValidPoint = false;
        }
            return point;
    }

    private void OnEnemyDie(ShooterEnemy enemy)
    {
        enemy.GetComponent<ShooterEnemyManager>().EnemyDie.RemoveListener(OnEnemyDie);

        StartCoroutine(ReturnEnemyAfterDeadTime(enemy));
    }

    private IEnumerator ReturnEnemyAfterDeadTime(ShooterEnemy enemy)
    {
        yield return new WaitForSeconds(enemy.Stats.DeadTime);
        _objectPool.ReturnToPool(enemy.gameObject);
    }

}
