namespace ManagementScripts
{
    using TMPro;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class EnemySpawnManager : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI enemiesLeftText;

        [Header("Spawn Settings")]
        public GameObject[] spawnPoints;
        public GameObject[] enemyPrefabs;
        public float initialSpawnInterval = 2.0f;
        public float spawnIntervalRandomness = 0.5f;
        public float yOffsetRandomnessStart;
        public float yOffsetRandomnessEnd;
        public int initialMultiplier;
        public int enemyDifficulty = 1;

        private float _currentSpawnInterval;
        private int _enemiesToSpawn;
        private int _enemiesSpawned;

        private WaveManager _waveManager;
        private float _nextSpawnTime;
        private int _enemiesDefeated;
        private bool _allEnemiesSpawned;

        private void Start()
        {
            _waveManager = FindObjectOfType<WaveManager>();
            _currentSpawnInterval = initialSpawnInterval;
            initialMultiplier = _waveManager.spawnMultiplier;
        }

        private int CalculateEnemyCount(int enemiesToSpawn, int enemiesDefeated)
        {
            int enemiesLeft = enemiesToSpawn - enemiesDefeated;
            if(enemiesLeft < 0)
            {
                enemiesLeft = 0;
            }
            return enemiesLeft;
        }

        private void Update()
        {
            enemiesLeftText.text = "Enemies: " + CalculateEnemyCount(_enemiesToSpawn, _enemiesDefeated);
            
            if (_enemiesToSpawn > 0 && Time.time >= _nextSpawnTime)
            {
                // Randomly spawn enemies within the initial multiplier
                initialMultiplier = _waveManager.spawnMultiplier;
                int x = Random.Range(1, initialMultiplier + 1);

                while (x > 0)
                {
                    SpawnEnemy();
                    x--;
                }

                _nextSpawnTime = Time.time + Random.Range(_currentSpawnInterval - spawnIntervalRandomness,
                    _currentSpawnInterval + spawnIntervalRandomness);
            }
        }

        public void StartSpawning(int enemiesToSpawn, float spawnIntervalDecrease)
        {
            _enemiesToSpawn = enemiesToSpawn;
            _enemiesSpawned = 0;
            _enemiesDefeated = 0;
            _allEnemiesSpawned = false;
            _currentSpawnInterval = Mathf.Max(0.1f, initialSpawnInterval - spawnIntervalDecrease);
            _nextSpawnTime = Time.time + _currentSpawnInterval;
        }

        public bool AllEnemiesDefeated()
        {
            return _allEnemiesSpawned && _enemiesDefeated >= _enemiesSpawned;
        }

        private void SpawnEnemy()
        {
            if (_allEnemiesSpawned)
            {
                return;
            }

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            int enemyIndex = Random.Range(0, enemyDifficulty);

            Vector2 spawnPosition = spawnPoints[spawnPointIndex].transform.position;
            float yOffset = Random.Range(yOffsetRandomnessStart, yOffsetRandomnessEnd);
            spawnPosition.y += yOffset;

            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
            enemy.GetComponent<SpriteRenderer>().sortingOrder = spawnPointIndex;
            enemy.GetComponent<EnemyManager>().OnEnemyDefeated += HandleEnemyDefeated;

            _enemiesSpawned++;
            if (_enemiesSpawned >= _enemiesToSpawn)
            {
                _allEnemiesSpawned = true;
            }
        }

        private void HandleEnemyDefeated()
        {
            _enemiesDefeated++;
            if (_allEnemiesSpawned && _enemiesDefeated >= _enemiesSpawned)
            {
                FindObjectOfType<WaveManager>().EndWave();
            }
        }
    }
}
