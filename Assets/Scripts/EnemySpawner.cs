using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private IntVariables _enemyCount;
    [SerializeField] private int _waveIncrement; //Qt of new enemy per spawn
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _waveTimer, _spawnIncrementTimer; //time in between each spawn wave / Time in between each spawnincrement ++
    private float _lastWave, _lastSpawnIncrement;
    [SerializeField] private BoolVariables _pause;

    private void Awake()
    {
        _lastWave = _lastSpawnIncrement = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_pause.value) TimerUpdate();
    }

    private void TimerUpdate()
    {
        if(Time.timeSinceLevelLoad > _lastWave)
        {
            _lastWave = Time.timeSinceLevelLoad + _waveTimer;
            for (int i = 0; i < _waveIncrement; i++)
            {
                SpawnTriangle();
            }
        }
        if (Time.timeSinceLevelLoad > _lastSpawnIncrement)
        {
            _lastSpawnIncrement = Time.timeSinceLevelLoad + _spawnIncrementTimer;
            _waveIncrement++;
        }
    }

    private void SpawnTriangle()
    {
        int where = Random.Range(1,5);
        Vector2 pos = SpawnCoord(where);
        Instantiate(_enemyPrefab, pos, Quaternion.identity);
    }

    private Vector2 SpawnCoord(int where)
    {
        Vector2 pos = Vector2.zero;
        float x, y;
        x = y = 0;
        if (where == 1)
        {
            x = Random.Range(-30f,40f);
            y = Random.Range(15f, 20f);
        }
        else if (where ==2)
        {
            x = Random.Range(30f, 40f);
            y = Random.Range(-20f, 15f);
        }
        else if (where == 3)
        {
            x = Random.Range(-40f, 30f);
            y = Random.Range(-20f, -15f);
        }
        else if (where == 4)
        {
            x = Random.Range(-40f, -30f);
            y = Random.Range(-15f, 20f);
        }
        pos.x = x;
        pos.y = y;
        return pos;
    }
}
