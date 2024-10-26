using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] enemyPrefabs;

    public float spawnInterval = 2.0f;
    public float spawnIntervalRandomness = 0.5f;

    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemies();
            nextSpawnTime = Time.time + Random.Range(spawnInterval - spawnIntervalRandomness, spawnInterval + spawnIntervalRandomness);
        }
    }

    private void SpawnEnemies()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        Vector2 spawnPosition = spawnPoints[spawnPointIndex].transform.position;
        
        float yOffset = Random.Range(0.05f, 0.45f);;
        spawnPosition.y += yOffset;

        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
        enemy.GetComponent<SpriteRenderer>().sortingOrder = spawnPointIndex;
    }
}