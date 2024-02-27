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

    private void Start()
    {
        anim = GetComponent<Animator>();
        hp = bossSO.maxHp;
        curProjectileDelay = delayProjectile;
    }

    private void Update()
    {
        switch (curBehavior)
        {
            case BossBehavior.Normal:

                Vector3 normalPos = PlayerManager.Instance.transform.position + normalOffset;
                normalOffset.z = 0;
                transform.position =
                    Vector3.SmoothDamp(transform.position, normalPos, ref velocity, normalSpeed); ;

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
        Destroy(gameObject);
    }

    public void SpawnProjectile()
    {
        int index = Random.Range(0, projectilePrefabs.Length);
        Vector3 pos = spawnProjectilePos.position;
        GameObject projectileObj = Instantiate(projectilePrefabs[index], pos, Quaternion.identity);
        BossProjectile bossProjectile = projectileObj.GetComponent<BossProjectile>();

    }

}
