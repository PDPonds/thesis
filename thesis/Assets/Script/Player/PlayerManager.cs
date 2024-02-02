using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;

    public event Action onJump;
    public event Action onSlide;
    public event Action onAttack;
    public event Action onTakeDamage;
    public event Action onDead;
    public event Action onHeal;
    public event Action onGetExp;
    public event Action onLevelup;

    public BaseState currentState;

    public RunningState running = new RunningState();
    public SlideState slide = new SlideState();
    //public OnAirState onAir = new OnAirState();
    public HurtState hurt = new HurtState();

    [HideInInspector] public InputSystemMnanger inputSystemMnanger;
    [HideInInspector] public PlayerAnimation playerAnimation;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CapsuleCollider2D col;
    [HideInInspector] public AugmentManager augmentManager;

    [Header("===== Game Play =====")]
    [Header("- Hp")]
    public int maxHp;
    [HideInInspector] public int currentHp;
    [HideInInspector] public bool isDead;

    [Header("- Level")]
    public float expMul;
    public float expTarget;
    public int startLevel;
    /*[HideInInspector]*/
    public float currentExp;
    /*[HideInInspector]*/
    public int currentLevel;

    [Header("- Augment")]
    public int maxSkillCount;
    [Space(5f)]
    public GameObject projectileEffect;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    [Space(5f)]
    public GameObject shieldEffect;
    [Space(5f)]
    public GameObject counterEffect;

    [Space(10f)]

    [Header("========== Controller ==========")]
    [Header("- Jump")]
    public float jumpForce;
    //public Collider2D groundCheck;
    //public float checkGroundDistance;
    //public LayerMask groundMask;
    [HideInInspector] public bool onGrounded;
    [HideInInspector] public int jumpCount;
    [Space(5f)]

    [Header("- Slide")]
    public float slideTime;
    public float slideDrag;
    public Vector2 runningCol;
    public Vector2 runningColPos;
    public Vector2 slideCol;
    public Vector2 slideColPos;
    [Space(5f)]

    [Header("- Attack")]
    public Collider2D attackCol;
    public float attackDelay;

    float currentAttackDelay;
    bool canAttack;

    private void Awake()
    {
        Instance = this;

        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        inputSystemMnanger = GetComponent<InputSystemMnanger>();
        augmentManager = GetComponent<AugmentManager>();

        currentAttackDelay = attackDelay;
        attackCol.enabled = false;

        currentHp = maxHp;

        SwitchState(running);

        currentLevel = startLevel;

    }

    private void Update()
    {
        currentState.UpdateState(transform.gameObject);

        #region Attack
        if (!canAttack)
        {
            currentAttackDelay -= Time.deltaTime;
            if (currentAttackDelay < 0)
            {
                canAttack = true;
                currentAttackDelay = attackDelay;
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                PlayerManager.Instance.attackCol.enabled = false;
            }
        }

        #endregion

        if (currentState != hurt && !isDead)
        {
            float speed = GameManager.Instance.currentSpeed;
            Vector3 centerPoint = GameManager.Instance.CenterPoint.position;
            Vector2 targetPos = new Vector2(centerPoint.x, transform.position.y);
            if (transform.position.x - targetPos.x < -0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                targetPos, speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,
                targetPos, speed * Time.deltaTime);
            }
        }

        if (isDead)
        {
            rb.isKinematic = true;
            rb.simulated = false;
        }

        if (onGrounded) anim.SetBool("onAir", false);

    }

    public void JumpPerformed()
    {
        if (onGrounded)
        {
            Jump(jumpForce);
        }
        else
        {
            if (jumpCount > 0)
            {
                Jump(jumpForce * 0.75f);
            }
        }
    }

    public void Jump(float force)
    {
        jumpCount--;

        anim.SetBool("onAir", true);
        anim.SetBool("Slide", false);

        Vector2 size = runningCol;
        Vector2 offset = runningColPos;
        SetupPlayerCol(size, offset);

        Vector3 spawnJumpParPos = transform.position + new Vector3(0, 0.5f, 0);
        GameManager.Instance.SpawnParticle(GameManager.Instance.jumpParticle, spawnJumpParPos);

        rb.velocity = Vector2.up * force;

        onJump?.Invoke();
    }

    public void SlidePerformed()
    {
        if (currentState != slide)
        {
            SwitchState(slide);
            onSlide?.Invoke();
        }
    }

    //public bool isGrounded()
    //{
    //    //Vector3 legPosV3 = transform.position;
    //    //Vector2 legPos = new Vector2(legPosV3.x, legPosV3.y);

    //    //RaycastHit2D groundHit = Physics2D.Raycast(legPos, Vector2.down, checkGroundDistance, groundMask);
    //    //Debug.DrawRay(legPos, Vector2.down * checkGroundDistance, Color.red);

    //    //return groundHit.collider != null;
    //    return false;
    //}

    public void SwitchState(BaseState state)
    {
        currentState = state;
        currentState.EnterState(transform.gameObject);
    }

    public void SetupPlayerCol(Vector2 size, Vector2 offset)
    {
        col.offset = offset;
        col.size = size;
    }

    public void AttackPerformed()
    {
        if (canAttack)
        {
            if (augmentManager.HasSkill(2, out int projectileSkillIndex))
            {
                AugmentSlot projectileSkillSlot = augmentManager.skillInventory[projectileSkillIndex];
                if (projectileSkillSlot.ready)
                {
                    SkillSO skill = projectileSkillSlot.skill;
                    int level = projectileSkillSlot.level;
                    float delayTime = skill.skillLevelAndDelays[level - 1].delay;

                    SpawnBullet();

                    projectileSkillSlot.delay = delayTime;
                    projectileSkillSlot.ready = false;

                }
            }
            anim.Play("Attack");
            attackCol.enabled = true;
            if (currentState == slide) SwitchState(running);
            onAttack?.Invoke();
            canAttack = false;
        }
    }

    public IEnumerator TakeDamage()
    {
        if (augmentManager.HasSkill(3, out int skillIndex))
        {
            AugmentSlot slot = augmentManager.skillInventory[skillIndex];
            if (slot.ready)
            {
                SkillSO skill = slot.skill;
                int level = slot.level;
                float delayTime = skill.skillLevelAndDelays[level - 1].delay;

                slot.delay = delayTime;
                slot.ready = false;
            }
            else
            {
                onTakeDamage?.Invoke();
                currentHp--;

                SwitchState(hurt);

                float time = GameManager.Instance.shakeDuration;
                float mag = GameManager.Instance.shakeMagnitude;
                StartCoroutine(GameManager.Instance.SceneShake(time, mag));

                GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);

                yield return new WaitForSecondsRealtime(0.2f);
                if (currentHp <= 0)
                {
                    Die();
                }
            }
        }
        else
        {
            onTakeDamage?.Invoke();
            currentHp--;

            SwitchState(hurt);

            float time = GameManager.Instance.shakeDuration;
            float mag = GameManager.Instance.shakeMagnitude;
            StartCoroutine(GameManager.Instance.SceneShake(time, mag));

            GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);

            yield return new WaitForSecondsRealtime(0.2f);
            if (currentHp <= 0)
            {
                Die();
            }
        }

    }

    public void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        onDead?.Invoke();
    }

    public bool Heal()
    {
        if (currentHp < maxHp)
        {
            currentHp++;
            onHeal?.Invoke();
            return true;
        }
        return false;
    }

    public void AddExp(float amount)
    {
        currentExp += amount;
        onGetExp?.Invoke();
        if (currentExp >= expTarget)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        onLevelup?.Invoke();
        expTarget += expMul;
        currentExp = 0;
        currentLevel++;
    }

    public void SpawnBullet()
    {
        GameObject bullet = bulletPrefab;

        GameObject bulletObj = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(Vector2.right * GameManager.Instance.currentSpeed * 2f, ForceMode2D.Impulse);
    }

}
