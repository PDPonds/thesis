using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Auto_Singleton<PlayerManager>
{
    public event Action onJump;
    public event Action onSlide;
    public event Action onAttack;
    public event Action onTakeDamage;
    public event Action onDead;
    public event Action onHeal;
    public event Action onLevelup;

    public BaseState currentState;

    public RunningState running = new RunningState();
    public SlideState slide = new SlideState();
    public OnAirState onAir = new OnAirState();
    public HurtState hurt = new HurtState();

    [HideInInspector] public InputSystemMnanger inputSystemMnanger;
    [HideInInspector] public PlayerAnimation playerAnimation;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CapsuleCollider2D col;

    [Header("===== Game Play =====")]
    [Header("- Hp")]
    public int maxHp;
    [HideInInspector] public int currentHp;
    [HideInInspector] public bool isDead;
    [Header("- Level")]
    public float expMul;
    public float expTarget;
    public int startLevel;
    [HideInInspector] public float currentExp;
    [HideInInspector] public int currentLevel;
    [Space(10f)]

    [Header("========== Controller ==========")]
    [Header("- Jump")]
    public float jumpForce;
    public float checkGroundDistance;
    public LayerMask groundMask;
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
        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        inputSystemMnanger = GetComponent<InputSystemMnanger>();

        currentAttackDelay = attackDelay;
        attackCol.enabled = false;

        currentHp = maxHp;

        SwitchState(running);

        currentLevel = startLevel;

    }

    private void Update()
    {
        onGrounded = isGrounded();
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

    }

    public void JumpPerformed()
    {
        if (onGrounded && currentState != onAir)
        {
            Vector3 spawnJumpParPos = transform.position + new Vector3(0, -1f, 0);
            GameManager.Instance.SpawnParticle(GameManager.Instance.jumpParticle, spawnJumpParPos);

            SwitchState(onAir);
            onJump?.Invoke();
            jumpCount--;
        }
        else
        {
            if (jumpCount > 0)
            {
                GameManager.Instance.SpawnParticle(GameManager.Instance.jumpParticle,
               transform.position + new Vector3(0, 0.5f, 0));

                SwitchState(onAir);
                onJump?.Invoke();
                jumpCount--;
            }
        }
    }

    public void SlidePerformed()
    {
        if (currentState != slide)
        {
            SwitchState(slide);
            onSlide?.Invoke();
        }
    }

    public bool isGrounded()
    {
        Vector3 legPosV3 = transform.position;
        Vector2 legPos = new Vector2(legPosV3.x, legPosV3.y);

        RaycastHit2D groundHit = Physics2D.Raycast(legPos, Vector2.down, checkGroundDistance, groundMask);
        Debug.DrawRay(legPos, Vector2.down * checkGroundDistance, Color.red);

        return groundHit.collider != null;

    }

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
            attackCol.enabled = true;
            if (currentState == slide) SwitchState(running);
            onAttack?.Invoke();
            canAttack = false;
        }
    }

    public IEnumerator TakeDamage()
    {
        currentHp--;
        onTakeDamage?.Invoke();

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

}
