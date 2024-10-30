namespace ManagementScripts
{
    using UnityEngine;
    using System.Collections;
    using TMPro;

    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Settings")]
        [SerializeField] private int initialEnemiesPerWave = 5;
        [SerializeField] private float timeBetweenWaves = 5.0f;
        [SerializeField] private float spawnIntervalDecreaseRate = 0.1f;
        public int spawnMultiplier = 1;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI waveText;

        private int _currentWave;
        private int _enemiesPerWave;
        private bool _waveInProgress;
        private bool _isStartingNextWave;
        private EnemySpawnManager _enemySpawnManager;

        private void Start()
        {
            _enemySpawnManager = GetComponent<EnemySpawnManager>();
            _enemiesPerWave = initialEnemiesPerWave;
            StartWave();
        }

        private void Update()
        {
            if (!_waveInProgress && _enemySpawnManager.AllEnemiesDefeated() && !_isStartingNextWave)
            {
                waveText.text = "Wave: " + _currentWave;
                StartCoroutine(StartNextWaveAfterDelay());
            }
        }

        private IEnumerator StartNextWaveAfterDelay()
        {
            _isStartingNextWave = true;
            yield return new WaitForSeconds(timeBetweenWaves);
            StartWave();
            _isStartingNextWave = false;
        }

        public void EndWave()
        {
            _waveInProgress = false;
            _enemiesPerWave += _currentWave; // Increase enemies per wave
            IncreaseSpawnMultiplier();
            IncreaseEnemyDifficulty();
        }

        private void StartWave()
        {
            Debug.Log("Starting wave " + _currentWave);
            _currentWave++;
            _waveInProgress = true;
            _enemySpawnManager.StartSpawning(_enemiesPerWave, spawnIntervalDecreaseRate * _currentWave);
        }

        private void IncreaseSpawnMultiplier()
        {
            if (_currentWave % 3 == 0)
            {
                spawnMultiplier++;
            }
        }
        
        private void IncreaseEnemyDifficulty()
        {
            if (_currentWave % 2 == 0)
            {
                if (_enemySpawnManager.enemyDifficulty < _enemySpawnManager.enemyPrefabs.Length)
                {
                    _enemySpawnManager.enemyDifficulty++;
                }
            }
        }
    }
}
