using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundAndObstacle : MonoBehaviour
{
    public Transform[] spawnPoint;

    public GameObject[] groundPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] obstaclePrefabs;

    public float moveSpeed;
    public float spawnTime;

    float currentSpawnTime;

    private void Update()
    {
        currentSpawnTime -= Time.deltaTime;

        if (currentSpawnTime < 0)
        {
            Spawn();
            currentSpawnTime = spawnTime;
        }

    }

    void Spawn()
    {
        int spawnIndex = Random.Range(0, spawnPoint.Length);
        Transform spawnPos = spawnPoint[spawnIndex];

        int groundIndex = Random.Range(0, groundPrefabs.Length);
        GameObject groundObj = groundPrefabs[groundIndex];
        GameObject ground = Instantiate(groundObj, spawnPos.position, Quaternion.identity);



        MoveGroundAndEnemy moveGroundAndEnemy = ground.GetComponent<MoveGroundAndEnemy>();
        moveGroundAndEnemy.moveSpeed = moveSpeed;

    }

}
