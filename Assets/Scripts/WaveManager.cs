using UnityEngine;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public int initialEnemiesPerWave = 5;
    public float timeBetweenWaves = 5.0f;
    public float spawnIntervalDecreaseRate = 0.1f;

    [SerializeField] int _currentWave = 0;
    private int _enemiesPerWave;
    private bool _waveInProgress = false;
    private EnemySpawnManager _enemySpawnManager;
    private bool _isStartingNextWave = false;
    public int spawnMultiplier = 1;

    public TextMeshProUGUI waveText;

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

    private void StartWave()
    {
        Debug.Log("Starting wave " + _currentWave);
        _currentWave++;
        _waveInProgress = true;
        _enemySpawnManager.StartSpawning(_enemiesPerWave, spawnIntervalDecreaseRate * _currentWave);
    }

    public void EndWave()
    {
        _waveInProgress = false;
        _enemiesPerWave += _currentWave; // Increase enemies per wave
        IncreaseSpawnMultiplier();
    }

    private void IncreaseSpawnMultiplier()
    {
        if (_currentWave % 3 == 0)
        {
            spawnMultiplier++;
        }
    }
}