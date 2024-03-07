using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public enum GameState
{
    Normal, BossFight
}

public class GameManager : MonoBehaviour
{
    public GameObject soundManager;
    public static GameManager Instance;

    [Header("===== Game =====")]
    public GameState state = GameState.Normal;
    public Transform CenterPoint;
    public float maxNormalSpeed;
    public float minNormalSpeed;

    public float currentSpeed;
    public Transform DeadPoint;

    public int maxRevivePerGame;

    [Header("- Frame Stop")]
    public float frameStopDuration;
    bool waiting;

    [Header("- Camera Shake")]
    public float shakeDuration;
    public float shakeMagnitude;

    [Header("- Game Particle")]
    public GameObject getCoinParticle;
    public GameObject hitParticle;
    public GameObject healParticle;
    public GameObject jumpParticle;
    public GameObject slashParticle;
    public GameObject smokeParticle;
    public GameObject weakspotParticle;

    [Header("- Score")]
    public float scoreMul;
    [HideInInspector] public int currentScore;
    [HideInInspector] public int hitScore;
    [Space(10f)]


    [Header("===== Player =====")]
    public Transform Camera;
    public Transform Player;

    [Header("- Spawn Floor")]
    [Header("Normal State")]
    public GameObject[] floorPrefabs;
    public Transform DestroyGroundAndEnemy;
    public float xOffset;
    [SerializeField] Vector3 lastEndPos;
    [Header("Boss State")]
    public GameObject boss;
    public Transform bossSpawnPos;
    public GameObject[] bossFloorPrefabs;
    [HideInInspector] public bool isBossClear;
    [HideInInspector] public GameObject curBoss;
    [Header("===== Coin =====")]
    public GameObject coin1Prefab;

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

    public void SpawnParticle(GameObject particle, Vector3 pos, bool randomRot, Vector3 scale)
    {
        if (randomRot)
        {
            float rotZ = Random.Range(-90f, 90f);
            Vector3 rot = new Vector3(0, 0, rotZ);
            Vector3 rot2 = new Vector3(0, 0, rotZ + 90f);
            GameObject particleObj = Instantiate(particle, pos, Quaternion.Euler(rot));
            GameObject particleObj2 = Instantiate(particle, pos, Quaternion.Euler(rot2));
            particleObj.transform.localScale = scale;
            particleObj2.transform.localScale = scale;
        }
        else
        {
            GameObject particleObj = Instantiate(particle, pos, Quaternion.identity);
            particleObj.transform.localScale = scale;
        }
    }

    public void GenerateFloor()
    {
        GameObject floor = new GameObject();
        if (state == GameState.Normal)
        {
            int floorIndex = Random.Range(0, floorPrefabs.Length);
            floor = floorPrefabs[floorIndex];
        }
        else if (state == GameState.BossFight)
        {
            int floorIndex = Random.Range(0, bossFloorPrefabs.Length);
            floor = bossFloorPrefabs[floorIndex];
        }

        GameObject buildObj = Instantiate(floor);
        Building currentBuilding = buildObj.GetComponent<Building>();

        Vector3 offset = buildObj.transform.position - currentBuilding.startPos.position;
        //Vector3 speedOffset = new Vector3(currentSpeed * xOffset, 0, 0);
        buildObj.transform.position = lastEndPos + offset /*+ speedOffset*/;

        lastEndPos = currentBuilding.endPos.position;

    }

    public void SpawnCoin(Vector2 capPos, int amount)
    {
        if (amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                float x = Random.Range(-2, 2);
                float y = Random.Range(-2, 2);
                Vector2 offset = new Vector2(x, y);
                Vector2 pos = capPos + offset;
                GameObject coinObj = Instantiate(coin1Prefab, pos, Quaternion.identity);
                Coin coin = coinObj.transform.GetChild(0).GetComponent<Coin>();
                coin.isDropFormCapsule = true;
            }
        }
    }

    public void SwitchState(GameState state)
    {
        this.state = state;
        switch (state)
        {
            case GameState.Normal:
                break;
            case GameState.BossFight:
                break;
        }
    }

    public void SpawnBoss()
    {
        if (curBoss == null && !isBossClear)
        {
            Vector3 pos = bossSpawnPos.position;
            GameObject bossObj = Instantiate(boss, pos, Quaternion.identity);
            curBoss = bossObj;
        }

    }

}
