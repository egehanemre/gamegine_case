using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] enemyPrefabs;

    public float spawnInterval = 2.0f;
    public float spawnIntervalRandomness = 0.5f;

    [SerializeField] public float yOffsetRandomnessStart;
    [SerializeField] public float yOffsetRandomnessEnd;
    
    private float _nextSpawnTime;

    private void Start()
    {
        _nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            SpawnEnemies();
            _nextSpawnTime = Time.time + Random.Range(spawnInterval - spawnIntervalRandomness, spawnInterval + spawnIntervalRandomness);
        }
    }

    private void SpawnEnemies()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        Vector2 spawnPosition = spawnPoints[spawnPointIndex].transform.position;
        
        float yOffset = Random.Range(yOffsetRandomnessStart, yOffsetRandomnessEnd);;
        spawnPosition.y += yOffset;

        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
        enemy.GetComponent<SpriteRenderer>().sortingOrder = spawnPointIndex;
    }
}