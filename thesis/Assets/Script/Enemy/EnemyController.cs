using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    public EnemySO enemySO;

    public event Action onDamage;
    public event Action onAttack;

    public int hp { get; set; }

    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;

    public LayerMask playerMask;

    float currentAttackDelay;
    bool canAttack;

    public float takedamageKnockback;

    [Header("===== IS Range Enemy =====")]
    public Transform bulletSpawnPoint;

    private void OnEnable()
    {
        onDamage += PlayHurtAnim;
        onAttack += PlayAttackAnim;
    }

    private void OnDisable()
    {
        onDamage -= PlayHurtAnim;
        onAttack -= PlayAttackAnim;
    }

    private void Awake()
    {
        hp = enemySO.maxHp;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        canAttack = true;
        float attackDelay = enemySO.attackSpeed;
        currentAttackDelay = attackDelay;

    }

    private void Update()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                Destroy(gameObject);
            }
        }

        if (enemySO is RangeEnemy)
        {
            RangeEnemy rangeEnemy = (RangeEnemy)enemySO;
            float range = Vector3.Distance(transform.position, GameManager.Instance.Player.position);
            if (range <= rangeEnemy.attackRange)
            {
                if (canAttack)
                {
                    onAttack?.Invoke();
                    SpawnBullet();
                    canAttack = false;
                }
                else
                {
                    currentAttackDelay -= Time.deltaTime;
                    if (currentAttackDelay < 0)
                    {
                        float attackDelay = enemySO.attackSpeed;
                        currentAttackDelay = attackDelay;
                        canAttack = true;
                    }
                }
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.transform.tag == "Player")
        {
            if (collision.transform.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
            {
                if (enemySO is CloseCombatEnemy)
                {
                    if (canAttack)
                    {
                        onAttack?.Invoke();

                        GameObject hitPar = GameManager.Instance.hitParticle;
                        GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                        StartCoroutine(playerManager.TakeDamage());
                        canAttack = false;
                    }
                    else
                    {
                        currentAttackDelay -= Time.deltaTime;
                        if (currentAttackDelay < 0)
                        {
                            float attackDelay = enemySO.attackSpeed;
                            currentAttackDelay = attackDelay;
                            canAttack = true;
                        }
                    }
                }
            }
        }

    }

    public IEnumerator TakeDamage()
    {
        hp--;
        rb.velocity = Vector2.right * takedamageKnockback;
        float attackDelay = enemySO.attackSpeed;
        currentAttackDelay = attackDelay;
        canAttack = false;

        float time = GameManager.Instance.shakeDuration;
        float mag = GameManager.Instance.shakeMagnitude;
        StartCoroutine(GameManager.Instance.SceneShake(time, mag));

        onDamage?.Invoke();
        //GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);
        yield return null;

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        anim.Play("Dead");

        int expDropCount = UnityEngine.Random.Range(enemySO.minExpDropCount, enemySO.maxExpDropCount);
        SpawnExpOrb(expDropCount);
    }

    void PlayHurtAnim()
    {
        anim.Play("Hurt");
    }

    void PlayAttackAnim()
    {
        anim.Play("Attack");
    }

    void SpawnBullet()
    {
        RangeEnemy rangeEnemy = (RangeEnemy)enemySO;
        GameObject bullet = rangeEnemy.bulletPrefab;
        float bulletSpeed = rangeEnemy.bulletSpeed;

        GameObject bulletObj = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(Vector2.left * bulletSpeed, ForceMode2D.Impulse);

    }

    void SpawnExpOrb(int count)
    {
        GameObject expPrefab = GameManager.Instance.expObj;

        for (int i = 0; i < count; i++)
        {
            float x = UnityEngine.Random.Range(2f, -2f);
            float y = UnityEngine.Random.Range(2f, -2f);
            Vector3 offset = new Vector3(x, y, 0);
            Vector3 spawnPoint = transform.position + offset;

            GameObject expObj = Instantiate(expPrefab, spawnPoint, Quaternion.identity);
        }

    }

}
