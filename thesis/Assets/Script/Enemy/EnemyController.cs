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
        float attackDelay = enemySO.attackSpeed;
        currentAttackDelay = attackDelay;
        canAttack = false;

        float time = GameManager.Instance.shakeDuration;
        float mag = GameManager.Instance.shakeMagnitude;
        StartCoroutine(GameManager.Instance.SceneShake(time, mag));

        GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);
        onDamage?.Invoke();
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.right * takedamageKnockback;
        canAttack = false;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        anim.Play("Dead");
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

}
