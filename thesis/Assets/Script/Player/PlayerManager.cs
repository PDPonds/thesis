using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;

    public static int upgradeMaxHpLevel = 0;
    public static int upgradeStealHpLevel = 0;
    public static int reviveItemCount = 0;
    public static int coin = 0;
    public int inGameCoin;

    public event Action onJump;
    public event Action onSlide;
    public event Action onAttack;
    public event Action onTakeDamage;
    public event Action onDead;
    public event Action onHeal;

    public BaseState currentState;

    public RunningState running = new RunningState();
    public SlideState slide = new SlideState();
    public HookState hook = new HookState();
    public EndHookState endHook = new EndHookState();
    public HurtState hurt = new HurtState();
    public ReviveState revive = new ReviveState();

    [HideInInspector] public InputSystemMnanger inputSystemMnanger;
    [HideInInspector] public PlayerAnimation playerAnimation;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CapsuleCollider2D col;

    [Header("===== Game Play =====")]
    [Header("- Hp")]
    public Transform mesh;
    public float startMaxHP;
    public float maxHp;
    public float hpMul;
    [HideInInspector] public float currentHp;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool noDamage;
    public float noDamageTime;
    public float blinkTime;
    [SerializeField] Color normalColor;
    [SerializeField] Color blinkColor;
    float curBlinkTime;
    float curNoDamageTime;
    bool isBlink;
    [HideInInspector] public bool isDropDead;
    [HideInInspector] public Transform lastCheckPoint;
    [HideInInspector] public int curReviveCount;
    [Space(10f)]

    [Header("========== Controller ==========")]
    [Header("- Jump")]
    public float jumpForce;
    public bool onGrounded;
    [HideInInspector] public int jumpCount;
    public bool isUp;

    [Space(5f)]

    [Header("- Slide")]
    //public float slideTime;
    public float slideDrag;
    public Vector2 runningCol;
    public Vector2 runningColPos;
    public Vector2 slideCol;
    public Vector2 slideColPos;
    [Space(5f)]

    [Header("- Attack")]
    public Collider2D attackCol;
    public float attackDelay;
    //public GameObject attackParticle;

    int attackCount;
    float currentAttackDelay;
    bool canAttack;
    [Space(5f)]

    //[Header("- Hook")]
    //public GameObject hookPrefab;
    //public Transform hookSpawnPos;
    //public float hookSpeed;
    //public float onHookTime;
    //public float moveToHookSpeed;
    //[HideInInspector] public bool canHook;
    //public float delayHookTime;
    //[HideInInspector] public float curDelayHookTime;

    //[HideInInspector] public List<Collider2D> enemyInFornt = new List<Collider2D>();

    //public float hookLength;
    //public LayerMask hookMask;

    //[HideInInspector] public Transform curHook;
    //public float waitHookTime;
    //public Transform checkHookablePoint;

    //public Transform hookableImage;
    //public Transform hookBorder;
    //public Image hookFill;

    [Header("- Revive")]
    public float reviveTime;
    [Header("- Upgrade")]
    public List<float> hpPerLevels = new List<float>();
    public List<float> stealHPLevels = new List<float>();

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

        maxHp = startMaxHP + hpPerLevels[upgradeMaxHpLevel];
        currentHp = maxHp;

        SwitchState(running);
        curNoDamageTime = noDamageTime;
        //canHook = true;

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

        #region Hook

        //if (!canHook)
        //{
        //    hookableImage.gameObject.SetActive(false);
        //    hookBorder.gameObject.SetActive(true);
        //    curDelayHookTime += Time.deltaTime;
        //    if (curDelayHookTime >= delayHookTime)
        //    {
        //        canHook = true;
        //    }

        //    float percent = curDelayHookTime / delayHookTime;
        //    hookFill.fillAmount = percent;

        //}
        //else
        //{
        //    hookableImage.gameObject.SetActive(true);
        //    hookBorder.gameObject.SetActive(false);
        //    curDelayHookTime = 0;
        //}

        //Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, hookLength, hookMask);
        //if (enemys.Length > 0)
        //{
        //    enemyInFornt.Clear();
        //    foreach (Collider2D col in enemys)
        //    {
        //        EnemyController controller = col.GetComponent<EnemyController>();
        //        if (controller.hookable)
        //        {
        //            enemyInFornt.Add(col);
        //        }
        //    }
        //}
        //else
        //{
        //    if (enemyInFornt.Count > 0) enemyInFornt.Clear();
        //}

        //if (enemyInFornt.Count > 0)
        //{
        //    enemyInFornt = enemyInFornt.OrderBy
        //        (x => Vector2.Distance(transform.position, x.transform.position)).ToList();
        //}

        //if (canHook)
        //{
        //    if (enemyInFornt.Count > 0)
        //    {
        //        if (enemyInFornt[0] != null)
        //        {
        //            EnemyController controller = enemyInFornt[0].GetComponent<EnemyController>();
        //            controller.targetVisual.gameObject.SetActive(true);
        //        }

        //    }

        //    //if (enemyInFornt.Count > 1)
        //    //{
        //    //    if (enemyInFornt[1] != null)
        //    //    {
        //    //        EnemyController controller = enemyInFornt[1].GetComponent<EnemyController>();
        //    //        controller.targetVisual.gameObject.SetActive(true);
        //    //    }
        //    //}
        //}
        //else
        //{
        //    if (enemyInFornt.Count > 0)
        //    {
        //        if (enemyInFornt[0] != null)
        //        {
        //            EnemyController controller = enemyInFornt[0].GetComponent<EnemyController>();
        //            controller.targetVisual.gameObject.SetActive(false);
        //        }

        //    }

        //    //if (enemyInFornt.Count > 1)
        //    //{
        //    //    if (enemyInFornt[1] != null)
        //    //    {
        //    //        EnemyController controller = enemyInFornt[1].GetComponent<EnemyController>();
        //    //        controller.targetVisual.gameObject.SetActive(false);
        //    //    }
        //    //}

        //}

        //if (curHook != null)
        //{
        //    if (currentState == hook || currentState == endHook)
        //    {
        //        if (curHook.position.x < transform.position.x)
        //        {
        //            curHook = null;
        //            SwitchState(running);
        //        }
        //    }
        //}

        #endregion

        #region Move
        if (currentState != hurt && !isDead && currentState != hook &&
            currentState != endHook && currentState != revive)
        {
            float speed = GameManager.Instance.currentSpeed;
            //transform.Translate(Vector2.right * Time.deltaTime * speed);

            Vector3 centerPoint = GameManager.Instance.CenterPoint.position;
            Vector2 targetPos = new Vector2(centerPoint.x, transform.position.y);
            if (transform.position.x < targetPos.x)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                targetPos, speed * 1.5f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,
                targetPos, speed * Time.deltaTime);
            }
        }
        #endregion


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

        if (isDead)
        {
            rb.isKinematic = true;
            rb.simulated = false;
        }
        else
        {
            rb.isKinematic = false;
            rb.simulated = true;
            currentHp -= Time.deltaTime * hpMul;
            if (currentHp <= 0)
            {
                Die();
            }
        }


        if (noDamage)
        {
            SpriteRenderer meshImage = mesh.GetComponent<SpriteRenderer>();
            curBlinkTime += Time.deltaTime;
            if (curBlinkTime >= blinkTime)
            {
                if (isBlink)
                {
                    meshImage.color = normalColor;
                    isBlink = false;
                }
                else
                {
                    meshImage.color = blinkColor;
                    isBlink = true;
                }
                curBlinkTime = 0;
            }

            curNoDamageTime -= Time.deltaTime;
            if (curNoDamageTime < 0)
            {
                curNoDamageTime = noDamageTime;
                noDamage = false;
            }

        }
        else
        {
            SpriteRenderer meshImage = mesh.GetComponent<SpriteRenderer>();
            meshImage.color = normalColor;
            isBlink = false;
        }

    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(transform.gameObject);
    }

    public void AddCoin(int amount)
    {
        inGameCoin += amount;
    }

    public void JumpPerformed()
    {

        if (onGrounded && !isDead)
        {
            Jump(jumpForce);
        }
        else if (!onGrounded && !isDead)
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

        anim.SetBool("Slide", false);
        if (jumpCount == 1)
        {
            anim.Play("StartJump");
        }
        else
        {
            anim.Play("SecondJump");
        }

        Vector2 size = runningCol;
        Vector2 offset = runningColPos;
        SetupPlayerCol(size, offset, CapsuleDirection2D.Vertical);

        Vector3 spawnJumpParPos = transform.position + new Vector3(0, 0.5f, 0);
        GameManager.Instance.SpawnParticle(GameManager.Instance.jumpParticle, spawnJumpParPos);

        rb.velocity = Vector2.up * force;

        onJump?.Invoke();
    }

    public void JumpAfterAttack(float force)
    {
        attackCol.enabled = false;

        anim.SetBool("onAir", true);
        anim.SetBool("Slide", false);

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
        if (currentState != slide && currentState != endHook &&
            currentState != hook)
        {
            attackCol.enabled = false;
            onSlide?.Invoke();
            SwitchState(slide);
        }
    }

    public void SliderCancle()
    {
        if (currentState == slide)
        {
            SwitchState(running);
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
        if (canAttack && !isDead)
        {
            if (currentState == slide)
            {
                anim.Play("SlideAttack");
            }
            else
            {
                if (attackCount % 2 != 0) anim.Play("Attack1");
                else anim.Play("Attack2");
            }
            //GameManager.Instance.SpawnParticle(attackParticle, attackCol.transform.position);
            attackCount++;

            attackCol.enabled = true;
            if (currentState == slide) SwitchState(running);
            onAttack?.Invoke();
            canAttack = false;
        }
    }

    public IEnumerator TakeDamage(float damage)
    {
        onTakeDamage?.Invoke();
        currentHp -= damage;
        noDamage = true;
        GameManager.Instance.currentMomentumTime = 0;
        GameManager.Instance.isMomentum = false;

        SwitchState(hurt);

        float time = GameManager.Instance.shakeDuration;
        float mag = GameManager.Instance.shakeMagnitude;
        StartCoroutine(GameManager.Instance.SceneShake(time, mag));

        GameManager.Instance.StopFrame(GameManager.Instance.frameStopDuration);

        if (currentHp <= 0)
        {
            Die();
        }
        yield return null;
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            attackCol.enabled = false;
            anim.SetBool("isDead", true);
            onDead?.Invoke();

        }
    }

    public bool Heal(float amount)
    {
        if (currentHp < maxHp)
        {
            currentHp += amount;
            if (currentHp > maxHp) { currentHp = maxHp; }
            onHeal?.Invoke();
            return true;
        }
        return false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, hookLength);
    //}

    //public void FirstHookPerformed()
    //{
    //    if (enemyInFornt.Count > 0 && canHook && !isDead)
    //    {
    //        canHook = false;
    //        SpawnHook(enemyInFornt[0].transform);
    //    }
    //}

    //void SecondHookPerformed()
    //{
    //    if (enemyInFornt.Count > 1)
    //    {
    //        if (enemyInFornt[1] != null)
    //        {
    //            SpawnHook(enemyInFornt[1].transform);
    //        }
    //    }
    //    else if (enemyInFornt.Count == 1)
    //    {
    //        SpawnHook(enemyInFornt[0].transform);
    //    }
    //    canHook = false;

    //}

    //void SpawnHook(Transform enemy)
    //{
    //    GameObject hookObj = Instantiate(hookPrefab, hookSpawnPos.position, Quaternion.identity);
    //    Hook hookScr = hookObj.GetComponent<Hook>();
    //    hookScr.target = enemy;

    //    anim.Play("HookUsing");

    //    curHook = hookObj.transform;
    //    SwitchState(hook);
    //}

    //public void HoldHook()
    //{
    //    if (enemyInFornt.Count > 0 && canHook && !isDead)
    //    {
    //        inputHookPerformed = true;
    //    }
    //}

    //public void CancleHoldHook()
    //{
    //    inputHookPerformed = false;
    //    if (enemyInFornt.Count > 0 && canHook && !isDead)
    //    {
    //        if (holdTime >= holdTarget)
    //        {
    //            SecondHookPerformed();
    //            holdTime = 0;
    //        }
    //        else
    //        {
    //            FirstHookPerformed();
    //            holdTime = 0;
    //        }
    //    }
    //}

}
