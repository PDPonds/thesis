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

    [Header("===== Score =====")]
    public int score;

    [Header("===== IS Range Enemy =====")]
    public Transform bulletSpawnPoint;

    [Header("===== Death =====")]
    public GameObject deatFrame;
    //[Header("===== Hook =====")]
    //public bool hookable;
    //public Transform targetVisual;
    //float dis;
    //public LayerMask hookMask;

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
                Instantiate(deatFrame, transform.position, Quaternion.identity);
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

        //dis = transform.position.x - PlayerManager.Instance.transform.position.x;
        //Vector3 dir = PlayerManager.Instance.checkHookablePoint.position - transform.position;

        //if (dis > 0)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, dir,
        //        PlayerManager.Instance.hookLength, hookMask);

        //    if (hit.collider != null)
        //    {
        //        if (hit.collider.CompareTag("Player")) hookable = true;
        //        else hookable = false;
        //    }
        //    else hookable = false;

        //}
        //else hookable = false;

        //if (!hookable) targetVisual.gameObject.SetActive(false);

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
                        if (!playerManager.noDamage)
                        {
                            if (GameManager.Instance.state == GameState.Normal)
                            {
                                if (!playerManager.noDamage)
                                {
                                    onAttack?.Invoke();

                                    GameObject hitPar = GameManager.Instance.hitParticle;
                                    GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                                    StartCoroutine(playerManager.TakeDamage(enemySO.damage));
                                    canAttack = false;
                                }
                            }
                            else if (GameManager.Instance.state == GameState.BossFight)
                            {
                                if (GameManager.Instance.curBoss != null &&
                                    GameManager.Instance.curBoss.activeSelf)
                                {
                                    BossController boss = GameManager.Instance.curBoss.GetComponent<BossController>();
                                    if (!playerManager.noDamage && boss.curBehavior != BossBehavior.Escape)
                                    {
                                        onAttack?.Invoke();

                                        GameObject hitPar = GameManager.Instance.hitParticle;
                                        GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                                        StartCoroutine(playerManager.TakeDamage(enemySO.damage));
                                        canAttack = false;
                                    }
                                }
                                else
                                {
                                    if (!playerManager.noDamage)
                                    {
                                        onAttack?.Invoke();

                                        GameObject hitPar = GameManager.Instance.hitParticle;
                                        GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                                        StartCoroutine(playerManager.TakeDamage(enemySO.damage));
                                        canAttack = false;
                                    }
                                }
                            }
                        }
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
        GameManager.Instance.SpawnParticle(GameManager.Instance.slashParticle, transform.position, true);

        GameObject hitPar = GameManager.Instance.hitParticle;
        GameManager.Instance.SpawnParticle(hitPar, transform.position);

        onDamage?.Invoke();
        GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);
        yield return null;

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (anim != null) anim.Play("Dead");
        SoundManager.Instance.PlayOnShot("BotDeath");
        if (GameManager.Instance.CheckInTargetMomentum())
        {
            PlayerManager.Instance.AddCoin(enemySO.dropCoin * 2);
            Debug.Log("InMomentum");
        }
        else
        {
            PlayerManager.Instance.AddCoin(enemySO.dropCoin);
        }


    }

    void PlayHurtAnim()
    {
        //anim.Play("Hurt");
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
        EnemyBullet enemyBullet = bulletObj.GetComponent<EnemyBullet>();
        enemyBullet.damage = enemySO.damage;
        SoundManager.Instance.PlayOnShot("LaserShot");
    }

}
