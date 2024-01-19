using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Transform[] enemySpawnPoint;

    public void GenerateEnemy()
    {
        int enemyCount = Random.Range(0, enemySpawnPoint.Length);

        for (int i = 0; i < enemyCount; i++)
        {
            int enemyIndex = Random.Range(0, GameManager.Instance.enemyPrefabs.Length);
            GameObject enemyPrefab = GameManager.Instance.enemyPrefabs[enemyIndex];

            int spawnIndex = Random.Range(0, enemySpawnPoint.Length);
            Transform spawn = enemySpawnPoint[spawnIndex];

            GameObject enemyObj = Instantiate(enemyPrefab, spawn.position, Quaternion.identity); ;

        }

    }

}
