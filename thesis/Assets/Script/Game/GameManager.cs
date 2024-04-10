using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    BeforeGameStart, Normal, BeforeFirstBoss, BeforeSecondBoss, BossFight,
    AfterFirstBoss, AfterSecondBoss
}

public enum MomentumAction
{
    None, Dash, Jump, Slide
}

public class GameManager : MonoBehaviour
{
    public GameObject soundManager;
    public static GameManager Instance;

    [Header("===== Game =====")]
    public GameState state = GameState.Normal;
    GameState lastState;

    public Transform CenterPoint;

    public float currentSpeed;
    [Header("- Momentum")]
    public float minSpeed;
    public float maxSpeed;
    public float targetMomentum;
    [SerializeField] GameObject momentumEffect;
    public float decreaseSpeedMul;
    [HideInInspector] public MomentumAction lastAction = MomentumAction.None;
    public float resetMomentumTime;
    [HideInInspector] public float curResetMomentumTime;
    [Header("- Momentum Multiplie per Action")]
    public float dashMulSpeed;
    public float jumpMulSpeed;
    public float slideMulSpeed;

    [Header("- Dead")]
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
    public GameObject counterAttackParticle;
    public GameObject dashParticle;
    public GameObject missileExplosion;
    public GameObject mineExplosion;

    [Header("===== Player =====")]
    public Transform Camera;
    public Transform Player;

    [Header("- Spawn Floor")]
    [Header("Dialog State")]
    public GameObject[] beforeGameStartFloorPrefabs;

    [Header("Normal State")]
    public Map tutorailMap;
    public Map[] normalMap;

    Map curMap;

    public Transform DestroyGroundAndEnemy;
    public float xOffset;
    [SerializeField] Vector3 lastEndPos;
    public int curFloorIndex;

    [Header("Boss State")]
    public GameObject boss;
    public Transform bossSpawnPos;
    public GameObject[] bossFloorPrefabs;
    [HideInInspector] public bool isBossClear;
    [HideInInspector] public GameObject curBoss;
    [Header("===== Coin =====")]
    public GameObject coin1Prefab;
    [Header("===== Boss =====")]
    public Transform warningMissileSpawnPoint;
    public GameObject warningObj;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SwitchState(GameState.BeforeGameStart);
        if (PlayerManager.passTutorial) curMap = normalMap[Random.Range(0, normalMap.Length)];
        else curMap = tutorailMap;
    }

    private void Update()
    {

        float playerAndSpawnPoint = Vector3.Distance(PlayerManager.Instance.transform.position,
            lastEndPos);
        if (playerAndSpawnPoint < 30f)
        {
            GenerateFloor();
        }

        #region Momentum

        ResetLastMomentum();

        if (CheckInTargetMomentum())
        {
            momentumEffect.SetActive(true);
        }
        else
        {
            momentumEffect.SetActive(false);
        }

        #endregion

    }

    public bool CheckInTargetMomentum()
    {
        return currentSpeed >= targetMomentum;
    }

    public void AddMomentum(MomentumAction action, float speed)
    {
        if (lastAction != action)
        {
            lastAction = action;
            currentSpeed += speed;

            if (currentSpeed > maxSpeed)
                currentSpeed = maxSpeed;
        }

    }

    void ResetLastMomentum()
    {
        if (lastAction != MomentumAction.None)
        {
            if (currentSpeed > minSpeed)
            {
                curResetMomentumTime -= Time.deltaTime;
                if (curResetMomentumTime < 0)
                {
                    curResetMomentumTime = resetMomentumTime;
                    lastAction = MomentumAction.None;
                }
            }
            else if (currentSpeed < minSpeed)
            {
                currentSpeed = minSpeed;
            }
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

    public GameObject SpawnParticle(GameObject particle, Vector3 pos)
    {
        GameObject particleObj = Instantiate(particle, pos, Quaternion.identity);
        return particleObj;
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

    public void SpawnParticle(GameObject particle, Vector3 pos, Transform parent)
    {
        GameObject newParticle = SpawnParticle(particle, pos);
        newParticle.transform.SetParent(parent);
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
            if (curFloorIndex == curMap.floors.Count) curFloorIndex = 0;
            floor = curMap.floors[curFloorIndex];
            curFloorIndex++;
        }
        else if (state == GameState.BossFight)
        {
            int floorIndex = Random.Range(0, bossFloorPrefabs.Length);
            floor = bossFloorPrefabs[floorIndex];
        }
        else if (state == GameState.BeforeGameStart ||
            state == GameState.BeforeFirstBoss ||
            state == GameState.BeforeSecondBoss ||
            state == GameState.AfterFirstBoss ||
            state == GameState.AfterSecondBoss)
        {
            int floorIndex = Random.Range(0, beforeGameStartFloorPrefabs.Length);
            floor = beforeGameStartFloorPrefabs[floorIndex];
        }

        GameObject buildObj = Instantiate(floor);
        Building currentBuilding = buildObj.GetComponent<Building>();

        Vector3 offset = buildObj.transform.position - currentBuilding.startPos.position;

        buildObj.transform.position = lastEndPos + offset;

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

                if(lastState == GameState.BeforeGameStart)
                {
                    UIManager.Instance.EnableProgressPoint(1);
                }
                else if(lastState == GameState.AfterFirstBoss)
                {
                    UIManager.Instance.EnableProgressPoint(3);
                }

                break;
            case GameState.BossFight:
                break;
            case GameState.BeforeGameStart:
                DialogueManager.Instance.StrartDialog(DialogueManager.Instance.dialogs);
                UIManager.Instance.EnableProgressPoint(0);
                lastState = GameState.BeforeGameStart;
                break;
            case GameState.BeforeFirstBoss:
                DialogueManager.Instance.StrartDialog(DialogueManager.Instance.beforeFirstBossDialogs);
                UIManager.Instance.EnableProgressPoint(2);
                break;
            case GameState.BeforeSecondBoss:
                DialogueManager.Instance.StrartDialog(DialogueManager.Instance.beforeSecondBossDialogs);
                UIManager.Instance.EnableProgressPoint(4);
                break;
            case GameState.AfterFirstBoss:
                DialogueManager.Instance.StrartDialog(DialogueManager.Instance.afterFirstBossDialogs);
                lastState = GameState.AfterFirstBoss;
                break;
            case GameState.AfterSecondBoss:
                DialogueManager.Instance.StrartDialog(DialogueManager.Instance.afterSecondBossDialogs);
                break;
        }
    }

    public void SpawnBoss()
    {
        if (state == GameState.BossFight)
        {
            if (curBoss == null && !isBossClear)
            {
                Vector3 pos = bossSpawnPos.position;
                GameObject bossObj = Instantiate(boss, pos, Quaternion.identity);
                curBoss = bossObj;
            }
            else if (curBoss != null && !isBossClear && !curBoss.activeSelf)
            {
                curBoss.transform.position = bossSpawnPos.position;
                curBoss.gameObject.SetActive(true);
                BossController boss = curBoss.GetComponent<BossController>();
                boss.hp = boss.bossSO.maxHp;
                boss.sparkParticle.SetActive(false);
                boss.SwitchBehavior(BossBehavior.AfterSpawn);
            }
        }
    }

}
