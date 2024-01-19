using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFloor : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    private void Awake()
    {
        float x = GameManager.Instance.Player.position.x + offset.x;
        float z = GameManager.Instance.Player.position.z + offset.z;
        Vector3 pos = new Vector3(x, 0, z);
        transform.position = pos;

        GenerateFloor();
    }

    private void Update()
    {
        float x = GameManager.Instance.Player.position.x + offset.x;
        float z = GameManager.Instance.Player.position.z + offset.z;
        Vector3 pos = new Vector3(x, 0, z);
        transform.position = pos;
    }

    public void GenerateFloor()
    {
        int floorIndex = Random.Range(0, GameManager.Instance.floorPrefabs.Length);
        GameObject floorPrefab = GameManager.Instance.floorPrefabs[floorIndex];
        Vector3 spawnPoint = GameManager.Instance.SpawnGround.position;

        GameObject floorObj = Instantiate(floorPrefab, spawnPoint, Quaternion.identity);
        SpawnEnemy spawnEnemy = floorObj.GetComponent<SpawnEnemy>();
        spawnEnemy.GenerateEnemy();
    }
}
