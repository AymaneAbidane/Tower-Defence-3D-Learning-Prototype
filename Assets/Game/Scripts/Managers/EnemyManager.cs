using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private WaveDetails currentWave;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnCooldown;

    [Header("Enemy Prefabs")]
    [SerializeField] private Enemy basicEnemyPrefab;
    [SerializeField] private Enemy fastEnemyPrefab;


    private List<Enemy> enemiesToCreate = new();
    private float spawnTime;

    private void Start()
    {
        enemiesToCreate = NewEnemyWave();
    }

    private void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0 && enemiesToCreate.Count > 0)
        {
            CreateEnemy();
            spawnTime = spawnCooldown;
        }
    }

    private Enemy GetRandomEnemy()
    {
        Enemy chosenEnemy = enemiesToCreate[UnityEngine.Random.Range(0, enemiesToCreate.Count)];
        enemiesToCreate.Remove(chosenEnemy);
        return chosenEnemy;
    }

    private void CreateEnemy()
    {
        Enemy randomEnemy = GetRandomEnemy();
        Instantiate(randomEnemy, spawnPoint.position, Quaternion.identity);
    }

    private List<Enemy> NewEnemyWave()
    {
        List<Enemy> newEnemyList = new();
        for (int i = 0; i < currentWave.basicEnemy; i++)
        {
            newEnemyList.Add(basicEnemyPrefab);
        }
        for (int i = 0; i < currentWave.fastEnemy; i++)
        {
            newEnemyList.Add(fastEnemyPrefab);
        }

        return newEnemyList;
    }
}

[Serializable]
public struct WaveDetails
{
    public int basicEnemy;
    public int fastEnemy;
}
