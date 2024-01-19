using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Auto_Singleton<GameManager>
{
    [Header("===== Game =====")]
    public Transform CenterPoint;
    public float maxSpeed;
    public float minSpeed;
    /*[HideInInspector]*/ public float currentSpeed;
    public Transform DeadPoint;

    [Header("- Frame Stop")]
    public float frameStopDuration;
    bool waiting;

    [Header("- Camera Shake")]
    public float shakeDuration;
    public float shakeMagnitude;

    [Header("- Game Particle")]
    public GameObject hitParticle;
    public GameObject healParticle;
    public GameObject jumpParticle;

    [Header("- Score")]
    public float scoreMul;
    [HideInInspector] public int currentScore;
    [Space(10f)]

    [Header("===== Player =====")]
    public Transform Camera;
    public Transform Player;
    [Space(10f)]

    [Header("- Spawn Floor")]
    public GameObject[] floorPrefabs;
    public Transform DestroyGroundAndEnemy;

    [Header("- Spawn Floor")]
    public GameObject[] enemyPrefabs;
    [SerializeField] float offset;
    float currentXOffset;

    private void Awake()
    {
        for (int i = 0; i < 2; i++)
        {
            GenerateFloor();
        }

    }

    private void Update()
    {
        float score = Player.position.x - transform.position.x;
        currentScore = (int)score * (int)scoreMul;

        if (currentScore / 1000 > minSpeed)
        {
            float speed = currentScore / 1000;
            currentSpeed = speed;
        }
        else if (currentScore / 1000 < minSpeed)
        {
            currentSpeed = minSpeed;
        }
        else if (currentScore / 1000 > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }
    }

    public void StopFrame(float duratuin)
    {
        if (waiting) return;
        Time.timeScale = 0f;
        StartCoroutine(Wait(frameStopDuration));
    }

    IEnumerator Wait(float duratuin)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duratuin);
        Time.timeScale = 1f;
        waiting = false;
    }

    public IEnumerator SceneShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Camera.localPosition = originalPos;
    }

    public void SpawnParticle(GameObject particle, Vector3 pos)
    {
        GameObject particleObj = Instantiate(particle, pos, Quaternion.identity);
    }

    public void GenerateFloor()
    {
        int floorIndex = Random.Range(0, GameManager.Instance.floorPrefabs.Length);
        GameObject floorPrefab = GameManager.Instance.floorPrefabs[floorIndex];
        Vector3 spawnPoint = new Vector3(currentXOffset + offset, 0, 0);

        GameObject floorObj = Instantiate(floorPrefab, spawnPoint, Quaternion.identity);
        //SpawnEnemy spawnEnemy = floorObj.GetComponent<SpawnEnemy>();
        //spawnEnemy.GenerateEnemy();
        currentXOffset += offset;

    }

}
