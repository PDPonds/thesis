using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("===== Game =====")]
    public Transform CenterPoint;
    public float maxNormalSpeed;
    public float minNormalSpeed;

    public float momentumMul;
    public float momentumTime;
    [HideInInspector] public float currentMomentumTime;
    [HideInInspector] public bool isMomentum;

    public float currentSpeed;
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
    public GameObject slashParticle;

    [Header("- Score")]
    public float scoreMul;
    [HideInInspector] public int currentScore;
    [HideInInspector] public int hitScore;
    [Space(10f)]


    [Header("===== Player =====")]
    public Transform Camera;
    public Transform Player;

    [Header("- Spawn Floor")]
    public GameObject[] floorPrefabs;
    public Transform DestroyGroundAndEnemy;
    public GameObject[] enemyPrefabs;
    public float xOffset;

    [SerializeField] Vector3 lastEndPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float dis = Player.position.x - transform.position.x;
        float disScore = dis * scoreMul;
        float totalScore = disScore + hitScore;
        if (totalScore > currentScore) currentScore = (int)totalScore;

        //if (!isMomentum)
        //{
        //    if (currentScore / 1000 > minNormalSpeed)
        //    {
        //        float speed = currentScore / 1000;
        //        currentSpeed = speed;
        //    }
        //    else if (currentScore / 1000 < minNormalSpeed)
        //    {
        //        currentSpeed = minNormalSpeed;
        //    }
        //    else if (currentScore / 1000 > maxNormalSpeed)
        //    {
        //        currentSpeed = maxNormalSpeed;
        //    }
        //}
        //else
        //{
        //    if (currentScore / 1000 > minNormalSpeed)
        //    {
        //        float speed = currentScore / 1000;
        //        float targetSpeed = speed + momentumMul;
        //        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime);
        //    }
        //    else if (currentScore / 1000 < minNormalSpeed)
        //    {
        //        float targetSpeed = minNormalSpeed + momentumMul;
        //        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime);
        //    }
        //    else if (currentScore / 1000 > maxNormalSpeed)
        //    {
        //        float targetSpeed = maxNormalSpeed + momentumMul;
        //        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime);
        //    }
        //}


        if (currentMomentumTime >= momentumTime)
        {
            isMomentum = true;
        }
        else
        {
            isMomentum = false;
            currentMomentumTime += Time.deltaTime;
        }

        float playerAndSpawnPoint = Vector3.Distance(PlayerManager.Instance.transform.position,
            lastEndPos);
        if (playerAndSpawnPoint < 30f)
        {
            GenerateFloor();
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

    public void SpawnParticle(GameObject particle, Vector3 pos, bool randomRot)
    {
        if (randomRot)
        {
            float rotZ = Random.Range(-90f, 90f);
            Vector3 rot = new Vector3(0, 0, rotZ);
            Vector3 rot2 = new Vector3(0, 0, rotZ + 90f);
            GameObject particleObj = Instantiate(particle, pos, Quaternion.Euler(rot));
            GameObject particleObj2 = Instantiate(particle, pos, Quaternion.Euler(rot2));
        }
        else
        {
            GameObject particleObj = Instantiate(particle, pos, Quaternion.identity);
        }
    }

    public void GenerateFloor()
    {
        int floorIndex = Random.Range(0, floorPrefabs.Length);

        GameObject buildPrefab = floorPrefabs[floorIndex];

        GameObject buildObj = Instantiate(buildPrefab);
        Building currentBuilding = buildObj.GetComponent<Building>();

        Vector3 offset = buildObj.transform.position - currentBuilding.startPos.position;
        Vector3 speedOffset = new Vector3(currentSpeed * xOffset, 0, 0);
        buildObj.transform.position = lastEndPos + offset + speedOffset;

        lastEndPos = currentBuilding.endPos.position;

    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + new Vector3(30, 0, 0);
        Gizmos.DrawSphere(pos, 0.1f);

    }

}
