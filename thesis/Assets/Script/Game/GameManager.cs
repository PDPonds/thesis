using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Auto_Singleton<GameManager>
{
    [Header("Game")]
    public Transform CenterPoint;
    public float currentSpeed;
    public Transform DeadPoint;

    public float frameStopDuration;
    bool waiting;

    public float shakeDuration;
    public float shakeMagnitude;

    [Header("Player")]
    public Transform Camera;
    public Transform Player;
    public Transform DestroyGroundAndEnemy;

    [Header("Floor")]
    public GameObject floorPrefabs;
    public float minFloorSize;
    public float maxFloorSize;

    [Header("Spawn Floor")]
    public Transform[] spawnPoints;


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


}
