using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Auto_Singleton<PlayerManager>
{
    public event Action onJump;
    public event Action onSlide;

    public BaseState currentState;

    public RunningState runningState = new RunningState();
    public SlideState slideState = new SlideState();
    public OnAirState onAir = new OnAirState();

    [HideInInspector] public InputSystemMnanger inputSystemMnanger;
    [HideInInspector] public PlayerAnimation playerAnimation;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CapsuleCollider2D col;

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

    private void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        inputSystemMnanger = GetComponent<InputSystemMnanger>();

        SwitchState(runningState);
    }

    private void Update()
    {
        onGrounded = isGrounded();
        currentState.UpdateState(transform.gameObject);
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

}
