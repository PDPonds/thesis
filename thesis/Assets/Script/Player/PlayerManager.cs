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

    [Header("===== Game Play =====")]
    [Header("- Hp")]
    public Transform mesh;
    public int maxHp;
    [HideInInspector] public int currentHp;
    [HideInInspector] public bool isDead;

    [Space(10f)]

    [Header("========== Controller ==========")]
    [Header("- Jump")]
    public float jumpForce;
    public bool onGrounded;
    [HideInInspector] public int jumpCount;
    public bool isUp;

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

    int attackCount;
    float currentAttackDelay;
    bool canAttack;

    private void Awake()
    {
        Instance = this;

        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = mesh.GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        inputSystemMnanger = GetComponent<InputSystemMnanger>();

        currentAttackDelay = attackDelay;
        attackCol.enabled = false;

        currentHp = maxHp;

        SwitchState(running);


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

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                attackCol.enabled = false;
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
        else
        {
            anim.SetBool("onAir", true);

            Vector3 up = transform.up;
            float velocityAlongAxis = Vector3.Dot(rb.velocity, up);
            if (velocityAlongAxis > 0) isUp = true;
            else isUp = false;

            anim.SetBool("isUp", isUp);

        }
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

        attackCol.enabled = false;

        //anim.SetBool("onAir", true);
        anim.SetBool("Slide", false);
        anim.Play("StartJump");

        Vector2 size = runningCol;
        Vector2 offset = runningColPos;
        SetupPlayerCol(size, offset, CapsuleDirection2D.Vertical);

        Vector3 spawnJumpParPos = transform.position + new Vector3(0, 0.5f, 0);
        GameManager.Instance.SpawnParticle(GameManager.Instance.jumpParticle, spawnJumpParPos);

        rb.velocity = Vector2.up * force;

        onJump?.Invoke();
    }

    public void SlidePerformed()
    {
        if (currentState != slide)
        {
            attackCol.enabled = false;
            SwitchState(slide);
            onSlide?.Invoke();
        }
    }


    public void SwitchState(BaseState state)
    {
        currentState = state;
        currentState.EnterState(transform.gameObject);
    }

    public void SetupPlayerCol(Vector2 size, Vector2 offset, CapsuleDirection2D direction)
    {
        col.offset = offset;
        col.size = size;
        col.direction = direction;
    }

    public void AttackPerformed()
    {
        if (canAttack)
        {
            if (attackCount % 2 != 0) anim.Play("Attack1");
            else anim.Play("Attack2");
            attackCount++;

            attackCol.enabled = true;
            if (currentState == slide) SwitchState(running);
            onAttack?.Invoke();
            canAttack = false;
        }
    }

    public IEnumerator TakeDamage()
    {
        onTakeDamage?.Invoke();
        currentHp--;

        GameManager.Instance.currentMomentumTime = 0;
        GameManager.Instance.isMomentum = false;

        SwitchState(hurt);

        float time = GameManager.Instance.shakeDuration;
        float mag = GameManager.Instance.shakeMagnitude;
        StartCoroutine(GameManager.Instance.SceneShake(time, mag));

        //GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);

        yield return null;
        if (currentHp <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        isDead = true;
        attackCol.enabled = false;
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


}
