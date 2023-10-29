using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase encargada de spawnear los enemigos por oleadas, y manejar el object pooling de estos
public class ShooterEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameInfo _gameInfo; //info del juego para saber que enemigo spawnear
    //prefabs de cada tipo de enemigo
    [SerializeField] private ShooterEnemy _hedgehogPrefab, _monkeyPrefab, _squirrelPrefab;
    [SerializeField] private int _initialEnemiesPerWave; //numero inicial de enemigos por oleada
    [SerializeField] private int _enemiesPerWaveIncrement; //incremento de n. de enemigos por oleada
    [SerializeField] private float _initialSecondsPerWave; //segundos iniciales entre 2 oleadas
    [SerializeField] private float _secondsPerWaveIncrement; //incremento de los segundos entre oleadas
    //posicion minima y maxima que definen los limites del area de spawn de enemigos
    [SerializeField] private Vector2 _minSpawnCoords, _maxSpawnCoords; 
    //al verificar si una posicion es valida para spawnear, estas capas se ignorarán
    //al calcular el overlap de posibles objetos que impidan spawnear en esa posicion
    [SerializeField] private LayerMask _ignoreLayersForSpawnOverlap;

    private ObjectPool _objectPool; //referencia al object pool
    private ShooterPlayerManager _playerManager; //ref al manager del jugador para saber si esta vivo

    private int enemiesPerWave = 0;
    private float secondsPerWave = 0;

    private void Awake()
    {
        _objectPool = GetComponent<ObjectPool>();
        _playerManager = FindAnyObjectByType<ShooterPlayerManager>();

        //Segun el enemigo electo en la tragaperras, spawnear el prefab correspondiente
        switch(_gameInfo.NPC)
        {
            case NPC.Hedgehog:
                _objectPool.ObjectToPool = _hedgehogPrefab.gameObject;
                _initialEnemiesPerWave = 6;
                _enemiesPerWaveIncrement = 2;
                _initialSecondsPerWave = 15;
                break;
            case NPC.Monkey:
                _objectPool.ObjectToPool = _monkeyPrefab.gameObject;
                _initialEnemiesPerWave = 3;
                _enemiesPerWaveIncrement = 2;
                _initialSecondsPerWave = 20;
                break;
            case NPC.Squirrel:
                _objectPool.ObjectToPool = _squirrelPrefab.gameObject;
                _initialEnemiesPerWave = 3;
                _enemiesPerWaveIncrement = 1;
                _initialSecondsPerWave = 25;
                break;
        }
    }

    private IEnumerator Start()
    {
        //se inicializa el object pool a mano, para pasarle antes de iniciarlo el prefab adecuado
        _objectPool.Initialize();

        enemiesPerWave = _initialEnemiesPerWave;
        secondsPerWave = _initialSecondsPerWave;

        //ciclo que se mantiene hasta que el jugador muera, para no spawnear más enemigos si esto pasa
        while(_playerManager.IsAlive)
        { 
            //si la object pool esta vacia o no hay suficientes objetos disponibles,
            //se espera el tiempo de otra oleada y  se vuelve  comprobar
            if(_objectPool.SizeOfPool() < enemiesPerWave)
            {
                yield return new WaitForSeconds(secondsPerWave);
                continue;
            }

            //si no, se spawnean todos los enemigos correspondientes
            for (int i = 0; i < enemiesPerWave; i++)
            {   
                Vector2 spawnPosition = GetSpawnPoint(); //Se obtiene una posicion valida de spawneo
                var enemy = SpawnEnemy(spawnPosition); //Se spawnea al enemigo en la posicion electa
                //nos suscribimos al evento de muerte del enemigo para devolverlo a la pool cuando muera
                enemy?.GetComponent<ShooterEnemyManager>().EnemyDie.AddListener(OnEnemyDie);
            }

            //se incrementa el numero de enemigos y segundos entre oleadas segun corresponda
            enemiesPerWave += _enemiesPerWaveIncrement;
            secondsPerWave += _secondsPerWaveIncrement;

            yield return new WaitForSeconds(secondsPerWave); //se espera hasta la siguiente oleada
        }
    }

    //Funcion que spawnea un enemigo en la posicion otorgada
    private ShooterEnemy SpawnEnemy(Vector2 position)
    {
        //saca al objeto de la pool, pero desactivado
        var enemy = _objectPool.GetFromPool(false)?.GetComponent<ShooterEnemy>();
       
        //si es null, se propaga (esto no dara error, solo no se spawneara nada)
        if (enemy == null) return null;

        //encuentra todos los componentes reseteables del enemigo, y los resetea
        var resets = enemy.GetComponentsInChildren<ISSpawnableEnemy>(true);
        foreach (var reset in resets) reset.Reset();

        //activa al enemigo, lo mueve a la posicion otorgada y lo devuelve
        enemy.gameObject.SetActive(true);
        enemy.transform.position = position;
        return enemy;
    }

    //funcion que devuelve una posicion de spawn valida dentro de los limites del mapa
    //pero fuera del campo de vista de la camara, para que no spawneen enemigos visiblemente
    private Vector2 GetSpawnPoint()
    {
        //crea variables para guardar el punto y saber si es valido
        Vector2 point = Vector2.zero;
        bool isValidPoint = false;

        //ciclo que se repite mientras el punto obtenido no sea valido
        while(!isValidPoint)
        {
            isValidPoint = true;
            
            //elegimos unas coordenadas random dentro de los limites del mapa
            point.x = Random.Range(_minSpawnCoords.x, _maxSpawnCoords.x);
            point.y = Random.Range(_minSpawnCoords.y, _maxSpawnCoords.y);

            //comprobamos si esta dentro del espacio de la camara (solo spawneamos por fuera de la camara)
            Vector3 camPoint = Camera.main.WorldToViewportPoint(point);
            if(camPoint.x >= 0 && camPoint.x < 1 && camPoint.y >= 0 && camPoint.y < 1)
                isValidPoint = false;

            //comprobamos si el punto no estaria colisionando con otro objeto
            else if(Physics2D.OverlapCircle(point, 0.5f, ~_ignoreLayersForSpawnOverlap) != null)
                isValidPoint = false;
        }
            return point; //devolvemos el punto cuando comprobemos que es valido
    }

    //cuando un enemigo spawneado muera se devuelve a la pool despues de un tiempo de espera,
    //para que se vea la animacion de muerte por un momento
    private void OnEnemyDie(ShooterEnemy enemy)
    {
        enemy.GetComponent<ShooterEnemyManager>().EnemyDie.RemoveListener(OnEnemyDie);

        StartCoroutine(ReturnEnemyAfterDeadTime(enemy));
    }

    //espera el tiempo correspondiente y devuelve al enemigo a la pool
    private IEnumerator ReturnEnemyAfterDeadTime(ShooterEnemy enemy)
    {
        yield return new WaitForSeconds(enemy.Stats.DeadTime);
        _objectPool.ReturnToPool(enemy.gameObject);
    }

}
