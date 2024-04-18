using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossBehavior
{
    AfterSpawn, Normal,/* Weakness,*/ Escape, TutorialSpawn, TutorialEscape
}

public enum BossAttackType
{
    Beam, Bomb, Lasser, Misslie
}

public class BossController : MonoBehaviour, IDamageable
{
    public BossSO bossSO;
    public int hp { get; set; }

    Animator anim;

    public BossBehavior curBehavior;
    public bool isEnterHalfHP;
    [HideInInspector] public bool isDead;
    [Header("===== Tutorial Spawn Behavior =====")]
    [SerializeField] Vector3 tutorialSpawnOffset;
    [SerializeField] float tutorialSpawnSpeed;
    [Header("===== Tutorial Escape Behavior =====")]
    [SerializeField] float tutorialEscapeBossTime;
    float curTutorialEscapeTime;
    [Header("===== After Spawn Behavior =====")]
    [SerializeField] float alertTime;
    float curAlertTime;
    [Header("===== Normal Behavior =====")]
    bool isFire;
    [SerializeField] float normalXSpeed;
    [SerializeField] float normalYSpeed;
    [SerializeField] Vector3 normalOffset;
    Vector3 velocity;

    [Space(5f)]
    [Header("Attack Percentage")]
    public List<BossAttack> attackTypes = new List<BossAttack>();
    [Space(5f)]

    [SerializeField] float delayProjectile;
    [SerializeField] float secondDelayProjectile;
    float curProjectileDelay;

    [Header("- Missile")]

    [Header("Missile Visual")]
    [SerializeField] Transform visualSpawnPoint;
    [SerializeField] float missileVisualDelayPerCount;
    [SerializeField] GameObject missileVisual;

    [SerializeField] Transform spawnProjectilePos;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int missliePerMax;
    [SerializeField] float missileDelayPerCount;
    public float projectileSpeed;
    public float projectileDamage;
    public float warningTime;
    [Header("- Lasser")]

    [SerializeField] float countPerMax;
    [SerializeField] float delayPerCount;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform[] bulletSpawnPoint;
    [SerializeField] GameObject[] gunEmis;

    [SerializeField] float bulletDamage;
    [SerializeField] float normalBulletSpeed;

    [Header("- Bomb")]
    [SerializeField] GameObject bomb;
    [SerializeField] Transform bombSpawnPoint;
    [SerializeField] float bombCountPerMax;
    [SerializeField] float bombDelayPerCount;
    [SerializeField] float bombDamage;
    [SerializeField] float bombMinForce;
    [SerializeField] float bombMaxForce;

    [Header("- Beam")]
    [SerializeField] GameObject beam;
    [SerializeField] BoxCollider2D beamCollider;
    [SerializeField] float beamDamage;
    [SerializeField] float delayBeam;
    [SerializeField] float beamTime;

    [Header("- Attack Animation")]
    [SerializeField] float missileAnimation;
    [SerializeField] float lasserAnimation;
    [SerializeField] float bombAnimation;
    [SerializeField] float beamAnimation;

    [Header("===== Escape Behavior =====")]
    [SerializeField] float hurtTime;
    float curHurtTime;
    [SerializeField] float escapeTime;
    float curEscapeTime;

    [Header("===== Death Behavior =====")]
    public GameObject sparkParticle;
    public float dyingParticleTime;
    float curDyimgTime;
    public GameObject dyingParticle;
    public GameObject bigFlashParticle;

    private void Start()
    {
        anim = GetComponent<Animator>();
        hp = bossSO.maxHp;
        spawnProjectilePos = GameManager.Instance.bossSpawnPos;

        Beam beamScr = beamCollider.GetComponent<Beam>();
        beamScr.damage = beamDamage;

    }

    [System.Obsolete]
    private void Update()
    {
        if (!isDead)
        {
            switch (curBehavior)
            {
                case BossBehavior.TutorialSpawn:

                    Vector3 tutorialPos = CenterMove.instance.transform.position + tutorialSpawnOffset;
                    normalOffset.z = 0;

                    Vector3 tutorialIdle = Vector3.SmoothDamp(transform.position, tutorialPos, ref velocity, tutorialSpawnSpeed);
                    transform.position = tutorialIdle;

                    break;
                case BossBehavior.TutorialEscape:

                    curTutorialEscapeTime -= Time.deltaTime;

                    Vector3 tutorialEscapePos = CenterMove.instance.transform.position + tutorialSpawnOffset + new Vector3(10f, 0, 0);
                    normalOffset.z = 0;

                    Vector3 tutorialEscape = Vector3.SmoothDamp(transform.position, tutorialEscapePos, ref velocity, tutorialSpawnSpeed);
                    transform.position = tutorialEscape;

                    if (curTutorialEscapeTime < 0)
                    {
                        GameManager.Instance.SwitchState(GameState.Normal);
                        gameObject.SetActive(false);
                    }

                    break;
                case BossBehavior.AfterSpawn:

                    if (curAlertTime > 0)
                    {
                        curAlertTime -= Time.deltaTime;
                    }
                    else
                    {
                        UIManager.Instance.ExitCutScene();
                        SwitchBehavior(BossBehavior.Normal);
                    }

                    Vector3 afterPos = CenterMove.instance.transform.position + normalOffset;
                    normalOffset.z = 0;

                    transform.position = Vector3.SmoothDamp(transform.position, afterPos, ref velocity, normalXSpeed);

                    break;
                case BossBehavior.Normal:

                    Vector3 normalPos = CenterMove.instance.transform.position + normalOffset;
                    normalOffset.z = 0;

                    Vector3 smoothX = Vector3.SmoothDamp(transform.position, normalPos, ref velocity, normalXSpeed);
                    Vector3 smoothY = Vector3.SmoothDamp(transform.position, normalPos, ref velocity, normalYSpeed);

                    transform.position = new Vector3(smoothX.x, smoothY.y, smoothX.z);

                    curProjectileDelay -= Time.deltaTime;
                    if (curProjectileDelay < 0 && !isFire)
                    {

                        BossAttackType type = GetRandomValue(attackTypes);
                        switch (type)
                        {
                            case BossAttackType.Misslie:
                                StartCoroutine(SpawnMissile());
                                isFire = true;
                                break;
                            case BossAttackType.Beam:
                                StartCoroutine(SpawnBeam());
                                isFire = true;
                                break;
                            case BossAttackType.Bomb:
                                StartCoroutine(FireBomb());
                                isFire = true;
                                break;
                            case BossAttackType.Lasser:
                                StartCoroutine(FireLasser());
                                isFire = true;
                                break;
                        }
                    }

                    break;
                case BossBehavior.Escape:

                    if (curHurtTime > 0)
                    {
                        Vector3 EscapePos = CenterMove.instance.transform.position + normalOffset;
                        normalOffset.z = 0;

                        Vector3 EscapesmoothX = Vector3.SmoothDamp(transform.position, EscapePos, ref velocity, normalXSpeed);
                        Vector3 EscapesmoothY = Vector3.SmoothDamp(transform.position, EscapePos, ref velocity, normalYSpeed);

                        transform.position = new Vector3(EscapesmoothX.x, EscapesmoothY.y, EscapesmoothX.z);

                        curHurtTime -= Time.deltaTime;
                    }
                    else
                    {
                        curEscapeTime -= Time.deltaTime;
                        Vector3 EscapePos = CenterMove.instance.transform.position + normalOffset + new Vector3(10f, 0, 0);
                        normalOffset.z = 0;

                        Vector3 EscapesmoothX = Vector3.SmoothDamp(transform.position, EscapePos, ref velocity, normalXSpeed);
                        Vector3 EscapesmoothY = Vector3.SmoothDamp(transform.position, EscapePos, ref velocity, normalYSpeed);

                        transform.position = new Vector3(EscapesmoothX.x, EscapesmoothY.y, EscapesmoothX.z);

                        if (curEscapeTime < 0)
                        {
                            UIManager.Instance.ExitCutScene();
                            GameManager.Instance.SwitchState(GameState.AfterFirstBoss);
                            gameObject.SetActive(false);
                        }
                    }

                    break;
            }

        }
        else
        {
            Vector3 normalPos = CenterMove.instance.transform.position + normalOffset;
            normalOffset.z = 0;

            Vector3 smoothX = Vector3.SmoothDamp(transform.position, normalPos, ref velocity, normalXSpeed);
            Vector3 smoothY = Vector3.SmoothDamp(transform.position, normalPos, ref velocity, normalYSpeed);

            transform.position = new Vector3(smoothX.x, smoothY.y, smoothX.z);

            Rigidbody2D playerRB = transform.GetComponent<Rigidbody2D>();
            playerRB.bodyType = RigidbodyType2D.Kinematic;
            playerRB.simulated = false;

            curDyimgTime -= Time.deltaTime;
            dyingParticle.SetActive(true);
            if (curDyimgTime < 0 && !bigFlashParticle.activeSelf)
            {
                dyingParticle.SetActive(false);
                SoundManager.Instance.Pause("SmallExplosion");
                SoundManager.Instance.PlayOnShot("BigExplosion");
                ParticleSystem big = bigFlashParticle.GetComponent<ParticleSystem>();
                float duration = big.duration + .5f;
                Destroy(gameObject, duration);
                bigFlashParticle.SetActive(true);
                GameManager.Instance.SwitchState(GameState.AfterSecondBoss);
            }

        }
    }

    public void OnDestroy()
    {
        UIManager.Instance.ExitCutScene();
    }

    public void SwitchBehavior(BossBehavior behavior)
    {
        curBehavior = behavior;
        switch (curBehavior)
        {
            case BossBehavior.TutorialSpawn:
                break;
            case BossBehavior.TutorialEscape:
                curTutorialEscapeTime = tutorialEscapeBossTime;
                break;
            case BossBehavior.AfterSpawn:
                SoundManager.Instance.PlayOnShot("BossAlert");
                SoundManager.Instance.Pause("NormalBGM");
                SoundManager.Instance.Play("BossBGM");
                UIManager.Instance.EnterCutScene();
                curAlertTime = alertTime;
                isFire = false;
                curProjectileDelay = delayProjectile;
                break;
            case BossBehavior.Normal:
                //curDelayWeakSpot = delayWeakSpot;
                //weakSpot.ResetWeakSpotHP();
                break;
            case BossBehavior.Escape:
                StopAllCoroutines();
                beamCollider.enabled = false;
                beam.SetActive(false);
                curHurtTime = hurtTime;
                curEscapeTime = escapeTime;
                UIManager.Instance.EnterCutScene();
                SoundManager.Instance.Pause("BossBGM");
                SoundManager.Instance.Play("NormalBGM");

                break;
        }
    }

    [System.Obsolete]
    public IEnumerator TakeDamage()
    {
        hp--;

        GameManager.Instance.SpawnParticle(GameManager.Instance.slashParticle, transform.position, true);
        GameObject hitPar = GameManager.Instance.hitParticle;
        GameManager.Instance.SpawnParticle(hitPar, transform.position);

        float time = GameManager.Instance.shakeDuration;
        float mag = GameManager.Instance.shakeMagnitude;
        StartCoroutine(GameManager.Instance.SceneShake(time, mag));

        GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);
        yield return null;

        if (hp <= bossSO.maxHp / 2)
        {
            if (!isEnterHalfHP)
            {
                isEnterHalfHP = true;
                UIManager.Instance.EnterCutScene();
                SwitchBehavior(BossBehavior.Escape);
            }
            else
            {
                if (hp <= 0)
                {
                    Die();
                }
            }
            sparkParticle.SetActive(true);
        }

    }

    [System.Obsolete]
    public void Die()
    {
        beamCollider.enabled = false;
        beam.SetActive(false);

        SoundManager.Instance.Pause("BossBGM");
        SoundManager.Instance.Play("NormalBGM");
        UIManager.Instance.EnterCutScene();
        SoundManager.Instance.PlayOnShot("SmallExplosion");
        curDyimgTime = dyingParticleTime;
        PlayerManager.Instance.AddCoin(bossSO.dropCoin);
        GameManager.Instance.isBossClear = true;
        isDead = true;
    }

    public void FireMissile()
    {
        Vector3 pos = spawnProjectilePos.position;
        GameObject projectileObj = Instantiate(projectilePrefab, pos, Quaternion.identity);

        BossProjectile bossProjectile = projectileObj.GetComponent<BossProjectile>();
        bossProjectile.SetupMissile(this);

        SoundManager.Instance.PlayOnShot("MissileShot");
    }

    IEnumerator SpawnMissileVisual()
    {
        int fireCount = 0;
        while (fireCount < missliePerMax)
        {
            Vector3 pos = visualSpawnPoint.position;
            GameObject projectileObj = Instantiate(missileVisual, pos, Quaternion.Euler(0, 0, -90f));
            MissileVisual visual = projectileObj.GetComponent<MissileVisual>();
            visual.SetupVisual(this);
            SoundManager.Instance.PlayOnShot("MissileShot");
            fireCount++;
            yield return new WaitForSeconds(missileVisualDelayPerCount);
        }
    }

    IEnumerator SpawnMissile()
    {
        anim.Play("Missile Power Start");
        yield return new WaitForSeconds(missileAnimation);
        yield return StartCoroutine(SpawnMissileVisual());
        int fireCount = 0;
        while (fireCount < missliePerMax)
        {
            FireMissile();
            fireCount++;
            yield return new WaitForSeconds(missileDelayPerCount);
        }
        yield return null;
        anim.Play("Missile Power End");
        if (isEnterHalfHP) curProjectileDelay = secondDelayProjectile;
        else curProjectileDelay = delayProjectile;
        isFire = false;
    }

    IEnumerator FireLasser()
    {
        anim.Play("Gatling Start");
        yield return new WaitForSeconds(lasserAnimation);
        int fireCount = 0;
        while (fireCount < countPerMax)
        {
            SpawnLasser();
            fireCount++;
            yield return new WaitForSeconds(delayPerCount);

            foreach (GameObject g in gunEmis) g.SetActive(false);
        }
        yield return null;
        anim.Play("Gatling End");
        if (isEnterHalfHP) curProjectileDelay = secondDelayProjectile;
        else curProjectileDelay = delayProjectile;
        isFire = false;
    }

    public void SpawnLasser()
    {
        int ran = UnityEngine.Random.Range(0, bulletSpawnPoint.Length);
        Vector3 pos = bulletSpawnPoint[ran].position;
        gunEmis[ran].SetActive(true);
        GameObject bulletObj = Instantiate(bullet, pos, Quaternion.identity);
        EnemyBullet ebullet = bulletObj.GetComponent<EnemyBullet>();
        ebullet.damage = bulletDamage;
        ebullet.speed = normalBulletSpeed;
        ebullet.controlDir = true;
        ebullet.dir = PlayerManager.Instance.transform.position - bulletSpawnPoint[ran].position;

        Vector3 dir = bulletSpawnPoint[ran].position - PlayerManager.Instance.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bulletObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        SoundManager.Instance.PlayOnShot("LaserShot");
    }

    //public void SwitchWeakSpotPos()
    //{
    //    if (weakSpot != null)
    //    {
    //        if (weakSpot.curPos == null)
    //        {
    //            weakSpot.curPos = pos1;
    //            weakSpot.transform.position = pos1.transform.position;
    //        }
    //        else
    //        {
    //            if (weakSpot.curPos == pos1)
    //            {
    //                weakSpot.curPos = pos2;
    //                weakSpot.transform.position = pos2.transform.position;
    //            }
    //            else if (weakSpot.curPos == pos2)
    //            {
    //                weakSpot.curPos = pos1;
    //                weakSpot.transform.position = pos1.transform.position;
    //            }
    //        }
    //    }
    //}

    IEnumerator FireBomb()
    {
        anim.Play("Bouncing Ball Start");
        yield return new WaitForSeconds(bombAnimation);

        int fireCount = 0;
        while (fireCount < bombCountPerMax)
        {
            SoundManager.instance.PlayOnShot("BossBB");
            SpawnBomb();
            fireCount++;
            yield return new WaitForSeconds(bombDelayPerCount);
        }
        yield return null;
        anim.Play("Bouncing Ball End");
        if (isEnterHalfHP) curProjectileDelay = secondDelayProjectile;
        else curProjectileDelay = delayProjectile;
        isFire = false;
    }

    public void SpawnBomb()
    {
        Vector3 pos = bombSpawnPoint.position;
        GameObject bombObj = Instantiate(bomb, pos, Quaternion.identity);
        Bomb bombScr = bombObj.GetComponent<Bomb>();
        bombScr.damage = bulletDamage;
        float rand = UnityEngine.Random.Range(bombMinForce, bombMaxForce);
        bombScr.force = rand;
        //float rand = Random.Range(minYBounce, maxYBounce);
        //bombScr.upForce = rand;

        //SoundManager.Instance.PlayOnShot("LaserShot");
    }

    public IEnumerator SpawnBeam()
    {
        anim.Play("Particle Cannon Start");
        yield return new WaitForSeconds(beamAnimation);

        beam.SetActive(true);
        beamCollider.enabled = false;
        SoundManager.Instance.PlayOnShot("BossBeam");
        yield return new WaitForSecondsRealtime(delayBeam);
        beamCollider.enabled = true;

        float mag = GameManager.Instance.shakeMagnitude;
        //GameManager.Instance.SceneShake(beamTime, mag);
        yield return StartCoroutine(GameManager.Instance.SceneShake(beamTime, mag));
        anim.Play("Particle Cannon End");
        beamCollider.enabled = false;
        beam.SetActive(false);
        if (isEnterHalfHP) curProjectileDelay = secondDelayProjectile;
        else curProjectileDelay = delayProjectile;
        isFire = false;
    }

    BossAttackType GetRandomValue(List<BossAttack> bossAttack)
    {
        BossAttackType output = BossAttackType.Misslie;
        int totalWeight = 0;

        foreach (BossAttack boss in bossAttack)
        {
            totalWeight += boss.weight;
        }

        int rndWeightVal = UnityEngine.Random.Range(0, totalWeight + 1);

        int processedWeight = 0;
        foreach (BossAttack boss in bossAttack)
        {
            processedWeight += boss.weight;
            if (rndWeightVal <= processedWeight)
            {
                output = boss.type;
                break;
            }
        }
        return output;
    }

}

[System.Serializable]
public class BossAttack
{
    public BossAttackType type;
    public int weight;
}
