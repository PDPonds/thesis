using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossBehavior
{
    AfterSpawn, Normal, Weakness
}

public class BossController : MonoBehaviour, IDamageable
{
    public BossSO bossSO;
    public int hp { get; set; }

    Animator anim;

    public BossBehavior curBehavior;
    bool isDead;
    [Header("===== After Spawn Behavior =====")]
    [SerializeField] float alertTime;
    float curAlertTime;
    [Header("===== Normal Behavior =====")]
    [Header("- WeakSpot")]
    [SerializeField] float normalXSpeed;
    [SerializeField] float normalYSpeed;

    [SerializeField] WeakSpot weakSpot;
    [SerializeField] Vector3 normalOffset;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;
    Vector3 velocity;
    [SerializeField] float delayWeakSpot;
    float curDelayWeakSpot;

    [Header("- Missile")]
    [SerializeField] Transform spawnProjectilePos;
    [SerializeField] float delayProjectile;
    float curProjectileDelay;
    [SerializeField] GameObject[] projectilePrefabs;
    public float projectileSpeed;
    public float projectileDamage;
    public float warningTime;
    [Header("- Lasser")]
    [SerializeField] float countPerMax;
    [SerializeField] float delayPerCount;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform[] bulletSpawnPoint;
    [SerializeField] float bulletDamage;
    [SerializeField] float normalBulletSpeed;

    [Header("===== Weakness Behavior =====")]
    [SerializeField] float weaknessSpeed;
    [SerializeField] Vector3 weaknessOffset;
    [SerializeField] float weaknessTime;
    float curWeaknessTime;
    [Header("===== Death Behavior =====")]
    [SerializeField] Vector3 deathOffset;

    private void Start()
    {
        anim = GetComponent<Animator>();
        hp = bossSO.maxHp;
        spawnProjectilePos = GameManager.Instance.bossSpawnPos;
        curProjectileDelay = delayProjectile;
        SwitchBehavior(BossBehavior.AfterSpawn);
    }

    private void Update()
    {
        if (!isDead)
        {
            switch (curBehavior)
            {
                case BossBehavior.AfterSpawn:

                    if (curAlertTime > 0)
                    {
                        curAlertTime -= Time.deltaTime;
                    }
                    else
                    {
                        SwitchBehavior(BossBehavior.Normal);
                    }

                    Vector3 afterPos = PlayerManager.Instance.transform.position + normalOffset;
                    normalOffset.z = 0;

                    transform.position = Vector3.SmoothDamp(transform.position, afterPos, ref velocity, normalXSpeed);

                    weakSpot.gameObject.SetActive(false);

                    break;
                case BossBehavior.Normal:
                    if (curDelayWeakSpot > 0)
                    {
                        curDelayWeakSpot -= Time.deltaTime;
                        if (weakSpot != null) weakSpot.gameObject.SetActive(false);
                    }
                    else
                    {
                        weakSpot.gameObject.SetActive(true);
                    }

                    Vector3 normalPos = PlayerManager.Instance.transform.position + normalOffset;
                    normalOffset.z = 0;

                    Vector3 smoothX = Vector3.SmoothDamp(transform.position, normalPos, ref velocity, normalXSpeed);
                    Vector3 smoothY = Vector3.SmoothDamp(transform.position, normalPos, ref velocity, normalYSpeed);

                    transform.position = new Vector3(smoothX.x, smoothY.y, smoothX.z);

                    curProjectileDelay -= Time.deltaTime;
                    if (curProjectileDelay < 0)
                    {
                        int ran = Random.Range(0, 2);
                        if (ran == 0)
                        {
                            SpawnMissile();
                            curProjectileDelay = delayProjectile;
                        }
                        else
                        {
                            StartCoroutine(FireLasser());
                            curProjectileDelay = delayProjectile;
                        }

                    }

                    break;
                case BossBehavior.Weakness:

                    weakSpot.gameObject.SetActive(false);
                    float weaknessYPos = Camera.main.transform.position.y + weaknessOffset.y;
                    float weaknessXPos = PlayerManager.Instance.transform.position.x + weaknessOffset.x;
                    Vector3 weaknessPos = new Vector3(weaknessXPos, weaknessYPos, 0);
                    transform.position = Vector2.MoveTowards(transform.position, weaknessPos, Time.deltaTime * weaknessSpeed);
                    curWeaknessTime -= Time.deltaTime;
                    if (curWeaknessTime < 0)
                    {
                        SwitchBehavior(BossBehavior.Normal);
                    }

                    break;
            }

        }
        else
        {
            Vector3 deathPos = transform.position + deathOffset;
            deathPos.z = 0;
            deathPos.y = deathOffset.y;
            transform.position = Vector2.MoveTowards(transform.position, deathPos, Time.deltaTime * weaknessSpeed);
            Rigidbody2D playerRB = transform.GetComponent<Rigidbody2D>();
            playerRB.bodyType = RigidbodyType2D.Kinematic;
            playerRB.simulated = false;
            Rigidbody2D weakspotRB = weakSpot.GetComponent<Rigidbody2D>();
            weakspotRB.bodyType = RigidbodyType2D.Kinematic;
            weakspotRB.simulated = false;
            weakSpot.gameObject.SetActive(false);
        }
    }

    public void SwitchBehavior(BossBehavior behavior)
    {
        curBehavior = behavior;
        switch (curBehavior)
        {
            case BossBehavior.AfterSpawn:
                SoundManager.Instance.PlayOnShot("BossAlert");
                SoundManager.Instance.Pause("NormalBGM");
                SoundManager.Instance.Play("BossBGM");
                curAlertTime = alertTime;
                break;
            case BossBehavior.Normal:
                curDelayWeakSpot = delayWeakSpot;
                weakSpot.ResetWeakSpotHP();
                break;
            case BossBehavior.Weakness:
                curWeaknessTime = weaknessTime;
                break;
        }
    }

    public IEnumerator TakeDamage()
    {
        if (curBehavior == BossBehavior.Weakness)
        {
            hp -= 2;
            GameManager.Instance.SpawnParticle(GameManager.Instance.slashParticle, transform.position, true);
        }
        else if (curBehavior == BossBehavior.Normal)
        {
            hp--;
            GameManager.Instance.SpawnParticle(GameManager.Instance.slashParticle, transform.position, true, new Vector3(0.5f, 0.5f, 0.5f));
        }

        anim.Play("Hurt");
        float time = GameManager.Instance.shakeDuration;
        float mag = GameManager.Instance.shakeMagnitude;
        StartCoroutine(GameManager.Instance.SceneShake(time, mag));

        GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);
        yield return null;

        if (hp <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        SoundManager.Instance.PlayOnShot("Explosive");
        SoundManager.Instance.Pause("BossBGM");
        SoundManager.Instance.Play("NormalBGM");
        GameManager.Instance.state = GameState.Normal;
        GameManager.Instance.hitScore += bossSO.dropScore;
        PlayerManager.Instance.AddCoin(bossSO.dropCoin);
        GameManager.Instance.isBossClear = true;
        isDead = true;
        Destroy(gameObject, 5f);
    }

    public void SpawnMissile()
    {
        int index = Random.Range(0, projectilePrefabs.Length);
        Vector3 pos = spawnProjectilePos.position;
        GameObject projectileObj = Instantiate(projectilePrefabs[index], pos, Quaternion.identity);

        BossProjectile bossProjectile = projectileObj.GetComponent<BossProjectile>();
        bossProjectile.SetupMissile(this);

        SoundManager.Instance.PlayOnShot("MissileShot");
    }

    IEnumerator FireLasser()
    {
        int fireCount = 0;
        while (fireCount < countPerMax)
        {
            SpawnLasser();
            fireCount++;
            yield return new WaitForSeconds(delayPerCount);

        }
    }

    public void SpawnLasser()
    {
        int ran = Random.Range(0, bulletSpawnPoint.Length);
        Vector3 pos = bulletSpawnPoint[ran].position;
        GameObject bulletObj = Instantiate(bullet, pos, Quaternion.identity);
        EnemyBullet ebullet = bulletObj.GetComponent<EnemyBullet>();
        ebullet.damage = bulletDamage;
        ebullet.speed = normalBulletSpeed;
        SoundManager.Instance.PlayOnShot("LaserShot");
    }

    public void SwitchWeakSpotPos()
    {
        if (weakSpot != null)
        {
            if (weakSpot.curPos == null)
            {
                weakSpot.curPos = pos1;
                weakSpot.transform.position = pos1.transform.position;
            }
            else
            {
                if (weakSpot.curPos == pos1)
                {
                    weakSpot.curPos = pos2;
                    weakSpot.transform.position = pos2.transform.position;
                }
                else if (weakSpot.curPos == pos2)
                {
                    weakSpot.curPos = pos1;
                    weakSpot.transform.position = pos1.transform.position;
                }
            }
        }
    }

}
