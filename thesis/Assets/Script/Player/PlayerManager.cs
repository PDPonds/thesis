using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public static bool passTutorial;
    public static int upgradeMaxHpLevel = 0;
    public static int upgradeStealHpLevel = 0;
    public static int reviveItemCount = 50;
    public static int coin = 0;
    public int inGameCoin;

    public event Action onJump;
    public event Action onSlide;
    public event Action onAttack;
    public event Action onTakeDamage;
    public Action onDead;
    public event Action onHeal;

    public BaseState currentState;

    public RunningState running = new RunningState();
    public SlideState slide = new SlideState();
    public HurtState hurt = new HurtState();
    public ReviveState revive = new ReviveState();
    public DashState dash = new DashState();

    public BaseState lastState;

    [HideInInspector] public InputSystemMnanger inputSystemMnanger;
    [HideInInspector] public PlayerAnimation playerAnimation;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CapsuleCollider2D col;

    [Header("===== Game Play =====")]
    [Header("- Move")]
    float speed;
    [HideInInspector] public float leftInput;
    [HideInInspector] public float rightInput;
    [Header("- Hp")]
    public Transform mesh;
    public float startMaxHP;
    public float maxHp;
    [HideInInspector] public float currentHp;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool noDamage;
    public float noDamageTime;
    public float blinkTime;
    [SerializeField] Color normalColor;
    [SerializeField] Color blinkColor;
    float curBlinkTime;
    [HideInInspector] public float curNoDamageTime;
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
    //bool jumpAfterAttack;

    [Space(5f)]

    [Header("- Slide")]
    //public float slideTime;
    public float slideDrag;
    public Vector2 runningCol;
    public Vector2 runningColPos;
    public Vector2 slideCol;
    public Vector2 slideColPos;
    [HideInInspector] public bool isSlideButtonDown;
    [HideInInspector] public bool hasGroundOnHead;
    public float checkHeadRange;
    [Space(5f)]

    [Header("- Attack")]
    public Collider2D attackCol;
    public float attackDelay;
    //public GameObject attackParticle;
    public float counterBulletSpeed;

    int attackCount;
    float currentAttackDelay;
    bool canAttack;

    [Header("- Counter")]
    public float counterToTargetTime;
    [HideInInspector] public float curCounterToTargetTime;
    public bool isCounterToTarget;

    [Header("- Dash")]
    public float dashDelay;
    public float dashPower;
    public float dashTime;
    [SerializeField] DashIcon dashIcon;
    [HideInInspector] public float curDashDelay;
    [HideInInspector] public float curDashTime;
    [HideInInspector] public bool canDash;

    [Space(5f)]
    [Header("========== Shop ==========")]
    [Header("- Revive")]
    public float reviveTime;
    [Header("- Upgrade")]
    public List<float> hpPerLevels = new List<float>();
    public List<float> stealHPLevels = new List<float>();

    [Header("========== Special Gadget ==========")]
    public GadgetSlot gadgetSlot;
    [Header("- Projectile")]
    public Transform shurikenSpawnPoint;

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

        #region Dash

        if (!canDash)
        {
            curDashDelay -= Time.deltaTime;
            if (curDashDelay < 0)
            {
                curDashDelay = dashDelay;
                dashIcon.PlayDashReady();
                canDash = true;
            }
        }

        #endregion

        #region Move
        if (currentState != revive && !isDead)
        {
            Vector3 centerPoint = GameManager.Instance.CenterPoint.position;

            if (GameManager.Instance.state == GameState.BeforeGameStart)
            {
                speed = GameManager.Instance.currentSpeed;
                Vector2 targetPos = new Vector2(centerPoint.x, transform.position.y);
                transform.position = Vector3.MoveTowards(transform.position,
                    targetPos, speed * Time.deltaTime);
            }
            else
            {

                speed = GameManager.Instance.currentSpeed;
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
        }
        #endregion

        #region Counter To Target
        if (!isCounterToTarget)
        {
            curCounterToTargetTime += Time.deltaTime;
            if (curCounterToTargetTime >= counterToTargetTime)
            {
                isCounterToTarget = true;
            }
        }
        #endregion

        if (onGrounded)
        {
            anim.SetBool("onAir", false);
        }
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
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = false;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
        }

        if (noDamage)
        {
            SpriteRenderer meshImage = mesh.GetComponent<SpriteRenderer>();

            Physics2D.IgnoreLayerCollision(3, 7, true);
            Physics2D.IgnoreLayerCollision(3, 12, true);
            Physics2D.IgnoreLayerCollision(3, 16, true);

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

            if (currentState != revive)
            {
                curNoDamageTime -= Time.deltaTime;
                if (curNoDamageTime < 0)
                {
                    curNoDamageTime = noDamageTime;
                    noDamage = false;
                }
            }
        }
        else
        {
            SpriteRenderer meshImage = mesh.GetComponent<SpriteRenderer>();
            meshImage.color = normalColor;
            isBlink = false;
            Physics2D.IgnoreLayerCollision(3, 7, false);
            Physics2D.IgnoreLayerCollision(3, 12, false);
            Physics2D.IgnoreLayerCollision(3, 16, false);
        }

        #region Momentum

        float curSpeed = GameManager.Instance.currentSpeed;
        float minSpeed = GameManager.Instance.minSpeed;

        if (curSpeed > minSpeed)
        {
            if (onGrounded)
            {
                GameManager.Instance.currentSpeed -= GameManager.Instance.decreaseSpeedMul * Time.deltaTime;
            }
        }

        #endregion

        if (currentState != slide) hasGroundOnHead = false;



    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(transform.gameObject);

    }

    public void DashPerformed()
    {
        if (canDash && !isDead && currentState != revive &&
            currentState != dash &&
            GameManager.Instance.state != GameState.BeforeGameStart &&
            GameManager.Instance.state != GameState.BeforeFirstBoss &&
            GameManager.Instance.state != GameState.BeforeSecondBoss &&
            GameManager.Instance.state != GameState.AfterFirstBoss &&
            GameManager.Instance.state != GameState.AfterSecondBoss &&
            !TutorialManager.instance.isTutorialPause)
        {
            SwitchState(dash);
            GameManager.Instance.AddMomentum(MomentumAction.Dash, GameManager.Instance.dashMulSpeed);
        }

        bool Mine = !passTutorial && TutorialManager.instance.isTutorialPause &&
          TutorialManager.instance.IsTutorial(TutorialState.Mine) &&
          TutorialManager.instance.CheckCanSkip();

        bool Dash = !passTutorial && TutorialManager.instance.isTutorialPause &&
            TutorialManager.instance.IsTutorial(TutorialState.Dash) &&
          TutorialManager.instance.CheckCanSkip();

        if (Mine || Dash)
        {
            TutorialManager.instance.NextTutorial();
        }
    }

    public void AddCoin(int amount)
    {
        inGameCoin += amount;
    }

    public void JumpPerformed()
    {
        if (!isDead && GameManager.Instance.state != GameState.BeforeGameStart &&
            GameManager.Instance.state != GameState.BeforeFirstBoss &&
            GameManager.Instance.state != GameState.BeforeSecondBoss &&
            GameManager.Instance.state != GameState.AfterFirstBoss &&
            GameManager.Instance.state != GameState.AfterSecondBoss &&
            !TutorialManager.instance.isTutorialPause)
        {
            if (currentState != slide)
            {
                if (onGrounded)
                {
                    Jump(jumpForce);
                }
                else if (!onGrounded)
                {
                    if (jumpCount > 0)
                    {
                        Jump(jumpForce);
                    }
                }
            }
            else
            {
                if (!hasGroundOnHead)
                {
                    if (onGrounded)
                    {
                        Jump(jumpForce);
                    }
                    else if (!onGrounded)
                    {
                        if (jumpCount > 0)
                        {
                            Jump(jumpForce);
                        }
                    }
                }
            }
        }
        else if (isDead)
        {
            UIManager.Instance.ReviveBut();
        }
        else if (GameManager.Instance.state == GameState.BeforeGameStart)
        {
            DialogueManager.Instance.NextDialog(DialogueManager.Instance.dialogs);
        }
        else if (GameManager.Instance.state == GameState.BeforeFirstBoss)
        {
            DialogueManager.Instance.NextDialog(DialogueManager.Instance.beforeFirstBossDialogs);
        }
        else if (GameManager.Instance.state == GameState.BeforeSecondBoss)
        {
            DialogueManager.Instance.NextDialog(DialogueManager.Instance.beforeSecondBossDialogs);
        }
        else if (GameManager.Instance.state == GameState.AfterFirstBoss)
        {
            DialogueManager.Instance.NextDialog(DialogueManager.Instance.afterFirstBossDialogs);
        }
        else if (GameManager.Instance.state == GameState.AfterSecondBoss)
        {
            DialogueManager.Instance.NextDialog(DialogueManager.Instance.afterSecondBossDialogs);
        }

        bool inTu = !passTutorial && TutorialManager.instance.isTutorialPause &&
          TutorialManager.instance.CheckCanSkip();

        if (inTu)
        {
            TutorialManager.instance.NextTutorial();
        }

    }

    public void Jump(float force)
    {
        jumpCount--;

        attackCol.enabled = false;
        SoundManager.Instance.PlayOnShot("Jump");
        anim.SetBool("Slide", false);
        if (jumpCount == 1)
        {
            anim.Play("StartJump");
        }
        else
        {
            anim.Play("SecondJump");
        }

        if (currentState == dash) SwitchState(running);
        if (currentState == slide) SwitchState(running);

        GameManager.Instance.AddMomentum(MomentumAction.Jump, GameManager.Instance.jumpMulSpeed);

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
        if (currentState == slide) return;

        attackCol.enabled = false;

        SoundManager.Instance.PlayOnShot("Jump");
        anim.SetBool("onAir", true);
        anim.SetBool("Slide", false);

        //jumpAfterAttack = true;

        GameManager.Instance.AddMomentum(MomentumAction.Jump, GameManager.Instance.jumpMulSpeed);

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
        if (currentState != revive &&
            GameManager.Instance.state != GameState.BeforeGameStart &&
            GameManager.Instance.state != GameState.BeforeFirstBoss &&
            GameManager.Instance.state != GameState.BeforeSecondBoss &&
            GameManager.Instance.state != GameState.AfterFirstBoss &&
            GameManager.Instance.state != GameState.AfterSecondBoss &&
            !TutorialManager.instance.isTutorialPause)
        {
            isSlideButtonDown = true;
            attackCol.enabled = false;
            onSlide?.Invoke();
            GameManager.Instance.AddMomentum(MomentumAction.Slide, GameManager.Instance.slideMulSpeed);
            SwitchState(slide);
        }

        if (!passTutorial && TutorialManager.instance.isTutorialPause &&
            TutorialManager.instance.IsTutorial(TutorialState.Slide) &&
          TutorialManager.instance.CheckCanSkip())
        {
            TutorialManager.instance.NextTutorial();
        }
    }

    public void SliderCancle()
    {
        //if (currentState == slide)
        //{
        isSlideButtonDown = false;
        //}
    }

    public void SwitchState(BaseState state)
    {
        lastState = currentState;
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
        if (canAttack && !isDead &&
            GameManager.Instance.state != GameState.BeforeGameStart &&
            GameManager.Instance.state != GameState.BeforeFirstBoss &&
            GameManager.Instance.state != GameState.BeforeSecondBoss &&
            GameManager.Instance.state != GameState.AfterFirstBoss &&
            GameManager.Instance.state != GameState.AfterSecondBoss &&
            !TutorialManager.instance.isTutorialPause)
        {
            if (currentState == slide)
            {
                anim.Play("SlideAttack");
                SoundManager.Instance.PlayOnShot("MCAtk2");
            }
            else
            {
                if (attackCount % 2 != 0)
                {
                    anim.Play("Attack1");
                    SoundManager.Instance.PlayOnShot("MCAtk1");
                }
                else
                {
                    anim.Play("Attack2");
                    SoundManager.Instance.PlayOnShot("MCAtk2");

                }
            }

            //}
            //GameManager.Instance.SpawnParticle(attackParticle, attackCol.transform.position);
            attackCount++;

            //attackCol.enabled = true;

            onAttack?.Invoke();
            canAttack = false;
        }

        bool Attack = !passTutorial && TutorialManager.instance.isTutorialPause &&
            TutorialManager.instance.IsTutorial(TutorialState.Attack) &&
          TutorialManager.instance.CheckCanSkip();

        bool Counter = !passTutorial && TutorialManager.instance.isTutorialPause &&
         TutorialManager.instance.IsTutorial(TutorialState.Counter) &&
          TutorialManager.instance.CheckCanSkip();

        bool Shuriken = !passTutorial && TutorialManager.instance.isTutorialPause &&
           TutorialManager.instance.IsTutorial(TutorialState.Shuriken) &&
          TutorialManager.instance.CheckCanSkip();

        bool Smoke = !passTutorial && TutorialManager.instance.isTutorialPause &&
          TutorialManager.instance.IsTutorial(TutorialState.Smoke) &&
          TutorialManager.instance.CheckCanSkip();



        if (Attack || Shuriken || Smoke || Counter)
        {
            TutorialManager.instance.NextTutorial();
        }

    }

    public IEnumerator TakeDamage(float damage)
    {

        onTakeDamage?.Invoke();
        currentHp -= damage;
        noDamage = true;

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
        if (!isDead && currentState != revive)
        {
            attackCol.enabled = false;
            anim.SetBool("isDead", true);
            SoundManager.Instance.PlayOnShot("Explosive");
            onDead?.Invoke();
            isDead = true;
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

    public void AddGadget(SpecialGadget gadget, int amount)
    {
        if (gadgetSlot.gadget == null)
        {
            gadgetSlot.gadget = gadget;
            gadgetSlot.amount = amount;
        }
        else
        {
            if (gadgetSlot.gadget == gadget)
            {
                gadgetSlot.amount += amount;
                if (gadgetSlot.amount > gadgetSlot.gadget.maxStack)
                {
                    gadgetSlot.amount = gadgetSlot.gadget.maxStack;
                }
            }
            else
            {
                ClearGadget();
                gadgetSlot.gadget = gadget;
                gadgetSlot.amount = amount;
            }
        }
    }

    public void RemoveGadget(int amount)
    {
        gadgetSlot.amount -= amount;
        if (gadgetSlot.amount <= 0)
        {
            ClearGadget();
        }
    }

    public void ClearGadget()
    {
        gadgetSlot.gadget = null;
        gadgetSlot.amount = 0;
    }

    public void UseSpecialGadget()
    {
        if (gadgetSlot.gadget != null)
        {
            //if (gadgetSlot.gadget is ProjectileGadget projectileGadget)
            //{
            //    GameObject projectileObj = Instantiate(projectileGadget.projectilePrefab,
            //        shurikenSpawnPoint.position, Quaternion.identity);

            //    Rigidbody2D rb = projectileObj.GetComponent<Rigidbody2D>();
            //    rb.AddForce(Vector2.right * projectileGadget.bulletSpeed, ForceMode2D.Impulse);

            //    RemoveGadget(1);

            //    SoundManager.Instance.PlayOnShot("ShurikenThrow");

            //}

            if (noDamage)
            {
                curNoDamageTime = noDamageTime;
                Physics2D.IgnoreLayerCollision(3, 7, false);
                noDamage = false;
            }

            gadgetSlot.gadget.UseGadget();

        }
    }

}

[Serializable]
public class GadgetSlot
{
    public SpecialGadget gadget;
    public int amount;
}