using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossBehavior
{
    None, Projectile, SpawnUnit, Crash
}

public class BossController : MonoBehaviour, IDamageable
{
    public BossSO bossSO;
    public int hp { get; set; }

    Animator anim;

    public float delayForNextBehavior;
    float curDelayNextBehavior;

    public BossBehavior curBehavior = BossBehavior.None;

    [Header("===== None Behavior =====")]
    [SerializeField] Vector3 nonePos;
    [Header("===== SpawnUnit Behavior =====")]
    [SerializeField] GameObject unitPrefab;
    [Header("===== Projectile Behavior =====")]
    [SerializeField] GameObject projectilePrefab;
    [Header("===== Crash Behavior =====")]
    [SerializeField] GameObject warning;
    [SerializeField] float chargeTime;
    float curChargeTime;
    float curCrashTime;
    float curAfterCrashTime;
    bool isCrash;
    bool crashAlready;
    [SerializeField] float crashSpeed;
    [SerializeField] float crashDamage;
    [SerializeField] float crashTime;
    [SerializeField] float afterCrashTime;
    [SerializeField] Vector3 beforeCrash;
    [SerializeField] Vector3 targetCrash;
    [SerializeField] Vector3 afterCrash;

    private void Start()
    {
        anim = GetComponent<Animator>();
        hp = bossSO.maxHp;
    }

    private void Update()
    {
        switch (curBehavior)
        {
            case BossBehavior.None:

                warning.SetActive(false);
                Vector3 curPos = Camera.main.transform.position + nonePos;
                transform.position = Vector2.Lerp(transform.position, curPos, Time.deltaTime);
                curDelayNextBehavior -= Time.deltaTime;
                if (curDelayNextBehavior < 0)
                {
                    SwitchBehavior(BossBehavior.Crash);
                }

                break;
            case BossBehavior.SpawnUnit:
                break;
            case BossBehavior.Projectile:
                break;
            case BossBehavior.Crash:

                if (!crashAlready)
                {
                    if (!isCrash)
                    {
                        warning.SetActive(true);
                        Vector3 beforePos = Camera.main.transform.position + beforeCrash;
                        transform.position = Vector2.Lerp(transform.position, beforePos, Time.deltaTime * crashSpeed);
                        curChargeTime -= Time.deltaTime;
                        if (curChargeTime < 0)
                        {
                            warning.SetActive(false);
                            curCrashTime = crashTime;
                            isCrash = true;
                        }
                    }
                    else
                    {
                        Vector3 crashPos = Camera.main.transform.position + targetCrash;
                        transform.position = Vector2.Lerp(transform.position, crashPos, Time.deltaTime * crashSpeed);
                        curCrashTime -= Time.deltaTime;
                        if (curCrashTime < 0)
                        {
                            curAfterCrashTime = afterCrashTime;
                            crashAlready = true;
                        }
                    }
                }
                else
                {
                    Vector3 afterPos = Camera.main.transform.position + afterCrash;
                    transform.position = Vector2.Lerp(transform.position, afterPos, Time.deltaTime);
                    curAfterCrashTime -= Time.deltaTime;
                    if (curAfterCrashTime < 0)
                    {
                        SwitchBehavior(BossBehavior.None);
                    }
                }

                break;
        }
    }

    void SwitchBehavior(BossBehavior behavior)
    {
        curBehavior = behavior;
        switch (curBehavior)
        {
            case BossBehavior.None:
                curDelayNextBehavior = delayForNextBehavior;
                break;
            case BossBehavior.SpawnUnit:
                break;
            case BossBehavior.Projectile:
                break;
            case BossBehavior.Crash:

                isCrash = false;
                crashAlready = false;
                curChargeTime = chargeTime;

                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.transform.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
            {
                if (!playerManager.noDamage)
                {
                    GameObject hitPar = GameManager.Instance.hitParticle;
                    GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                    StartCoroutine(playerManager.TakeDamage(crashDamage));
                }
            }
        }
    }

    public IEnumerator TakeDamage()
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

    public void Die()
    {
        GameManager.Instance.hitScore += bossSO.dropScore;
        PlayerManager.Instance.AddCoin(bossSO.dropCoin);
        Destroy(gameObject);
    }

}
