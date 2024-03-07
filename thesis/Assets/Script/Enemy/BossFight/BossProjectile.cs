using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissileState
{
    Warning, Fire
}

public class BossProjectile : MonoBehaviour
{
    public float speed;
    public float damage;

    public BossController boss;
    MissileState state;
    float curWarningTime;

    List<EnemyBullet> allBullets = new List<EnemyBullet>();
    List<GameObject> allWarning = new List<GameObject>();

    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        if (state == MissileState.Fire)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
        else if (state == MissileState.Warning)
        {
            transform.position = GameManager.Instance.bossSpawnPos.position;
            curWarningTime -= Time.deltaTime;
            if (curWarningTime < 0)
            {
                SwitchState(MissileState.Fire);
            }
        }
    }

    public void SwitchState(MissileState _state)
    {
        state = _state;
        switch (_state)
        {
            case MissileState.Warning:
                SpawnWarning();
                curWarningTime = boss.warningTime;
                break;
            case MissileState.Fire:
                ClearWarning();
                break;
        }
    }

    void SpawnWarning()
    {
        if (allBullets.Count > 0)
        {
            for (int i = 0; i < allBullets.Count; i++)
            {
                GameObject go = Instantiate(GameManager.Instance.warningObj, GameManager.Instance.warningMissileSpawnPoint);
                Vector3 spawnPos = GameManager.Instance.warningMissileSpawnPoint.position;
                spawnPos.y = allBullets[i].transform.position.y;
                go.transform.position = spawnPos;
                allWarning.Add(go);
                Debug.Log("Spawn Warning");
            }
        }
    }

    void ClearWarning()
    {
        if (allWarning.Count > 0)
        {
            foreach (GameObject go in allWarning)
            {
                Destroy(go);
            }
            allWarning.Clear();
            Debug.Log("Clear Warning");
        }
    }

    public void SetupMissile(BossController _boss)
    {
        speed = _boss.projectileSpeed;
        damage = _boss.projectileDamage;
        boss = _boss;
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<EnemyBullet>(out EnemyBullet bullet))
                {
                    allBullets.Add(bullet);
                }
            }

            if (allBullets.Count > 0)
            {
                foreach (EnemyBullet bullet in allBullets)
                {
                    bullet.damage = damage;
                }
            }

        }

        SwitchState(MissileState.Warning);
    }

}
