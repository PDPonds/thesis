using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossBehavior
{
    Normal, Weakness
}

public class BossController : MonoBehaviour, IDamageable
{
    public BossSO bossSO;
    public int hp { get; set; }

    Animator anim;

    public BossBehavior curBehavior = BossBehavior.Normal;
    bool isDead;

    [Header("===== Normal Behavior =====")]
    [Header("- WeakSpot")]
    [SerializeField] float normalSpeed;
    [SerializeField] WeakSpot weakSpot;
    [SerializeField] Vector3 normalOffset;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;
    Vector3 velocity;
    [Header("- Missile")]
    [SerializeField] Transform spawnProjectilePos;
    [SerializeField] float delayProjectile;
    float curProjectileDelay;
    [SerializeField] GameObject[] projectilePrefabs;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileDamage;
    [Header("- Lasser")]
    [SerializeField] float countPerMax;
    [SerializeField] float delayPerCount;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawnPoint;
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
    }

    private void Update()
    {
        if (!isDead)
        {
            switch (curBehavior)
            {
                case BossBehavior.Normal:

                    Vector3 normalPos = PlayerManager.Instance.transform.position + normalOffset;
                    normalOffset.z = 0;
                    transform.position =
                        Vector3.SmoothDamp(transform.position, normalPos, ref velocity, normalSpeed); ;

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
                        }

                    }

                    break;
                case BossBehavior.Weakness:

                    Vector3 weaknessPos = Camera.main.transform.position + weaknessOffset;
                    weaknessPos.z = 0;
                    weaknessPos.y = weaknessOffset.y;
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
            Rigidbody2D weakspotRB = transform.GetChild(0).GetComponent<Rigidbody2D>();
            weakspotRB.bodyType = RigidbodyType2D.Kinematic;
            playerRB.simulated = false;

        }
    }

    public void SwitchBehavior(BossBehavior behavior)
    {
        curBehavior = behavior;
        switch (curBehavior)
        {
            case BossBehavior.Normal:
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
            hp--;
            anim.Play("Hurt");
            float time = GameManager.Instance.shakeDuration;
            float mag = GameManager.Instance.shakeMagnitude;
            StartCoroutine(GameManager.Instance.SceneShake(time, mag));
            GameManager.Instance.SpawnParticle(GameManager.Instance.slashParticle, transform.position, true);

            GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);
            yield return null;

            if (hp <= 0)
            {
                Die();
            }
        }

    }

    public void Die()
    {
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
        bossProjectile.speed = projectileSpeed;
        bossProjectile.damage = projectileDamage;
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
        curProjectileDelay = delayProjectile;

    }

    public void SpawnLasser()
    {
        Vector3 pos = bulletSpawnPoint.position;
        GameObject bulletObj = Instantiate(bullet, pos, Quaternion.identity);
        EnemyBullet ebullet = bulletObj.GetComponent<EnemyBullet>();
        ebullet.damage = bulletDamage;
        ebullet.speed = normalBulletSpeed;
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
