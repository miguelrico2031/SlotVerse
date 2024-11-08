using UnityEngine;

public class RacingEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _monkey;
    [SerializeField] private GameObject _squirrel;
    [SerializeField] private GameObject _hedgehog;
    [SerializeField] private GameInfo _gameInfo;

    private GameObject _enemyPrefab;

    private GameObject _instantiatedEnemy;

    private bool _hasSpawned = false;

    private void Awake()
    {
        switch (_gameInfo.NPC)
        {
            case NPC.Hedgehog:
                _enemyPrefab = _hedgehog;
                break;

            case NPC.Monkey:
                _enemyPrefab = _monkey; 
                break;

            case NPC.Squirrel:
                _enemyPrefab = _squirrel;
                break;
        }
    }

    public void SpawnEnemy()
    {
        if (_hasSpawned) return;

        _instantiatedEnemy = Instantiate(_enemyPrefab, transform.position, transform.rotation, transform);

        _hasSpawned = true;
    }
}