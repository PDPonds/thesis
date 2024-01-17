using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Auto_Singleton<PlayerManager>
{
    public event Action onJump;
    public event Action onSlide;
    public event Action onAttack;

    public BaseState currentState;

    public RunningState runningState = new RunningState();
    public SlideState slideState = new SlideState();
    public OnAirState onAir = new OnAirState();

    [HideInInspector] public InputSystemMnanger inputSystemMnanger;
    [HideInInspector] public PlayerAnimation playerAnimation;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D col;

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
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        inputSystemMnanger = GetComponent<InputSystemMnanger>();

        currentAttackDelay = attackDelay;
        attackCol.enabled = false;

        SwitchState(runningState);
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
    }

    public void JumpPerformed()
    {
        if (onGrounded && currentState != onAir)
        {
            SwitchState(onAir);
            onJump?.Invoke();
            jumpCount--;
        }
        else
        {
            if (jumpCount > 0)
            {
                SwitchState(onAir);
                onJump?.Invoke();
                jumpCount--;
            }
        }
    }

    public void SlidePerformed()
    {
        if (currentState != slideState)
        {
            SwitchState(slideState);
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
            if (currentState == slideState) SwitchState(runningState);
            onAttack?.Invoke();
            canAttack = false;
        }
    }

}
