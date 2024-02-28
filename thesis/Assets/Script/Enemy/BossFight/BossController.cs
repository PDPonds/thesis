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
    [SerializeField] float normalSpeed;
    [SerializeField] WeakSpot weakSpot;
    [SerializeField] Vector3 normalOffset;
    Vector3 velocity;
    [Header("- Projectile")]
    [SerializeField] Transform spawnProjectilePos;
    [SerializeField] float delayProjectile;
    float curProjectileDelay;
    [SerializeField] GameObject[] projectilePrefabs;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileDamage;

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
                        SpawnProjectile();
                        curProjectileDelay = delayProjectile;
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

    public void SpawnProjectile()
    {
        int index = Random.Range(0, projectilePrefabs.Length);
        Vector3 pos = spawnProjectilePos.position;
        GameObject projectileObj = Instantiate(projectilePrefabs[index], pos, Quaternion.identity);
        BossProjectile bossProjectile = projectileObj.GetComponent<BossProjectile>();
        bossProjectile.speed = projectileSpeed;
        bossProjectile.damage = projectileDamage;
    }

}
