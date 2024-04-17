using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("===== HP =====")]
    [SerializeField] GameObject hpBorder;
    [SerializeField] Image hpFill;
    [Header("===== Time =====")]
    [SerializeField] TextMeshProUGUI timeText;
    [Header("===== Fade =====")]
    [SerializeField] CanvasGroup fadeCanvas;
    [Header("===== Dash =====")]
    [SerializeField] GameObject dashBorder;
    [SerializeField] Image dashFill;
    [Header("===== Score Coin =====")]
    public GameObject coinBorder;
    public TextMeshProUGUI coinInGameText;

    [Header("===== Dead Scene =====")]
    public Transform deadScene;
    public TextMeshProUGUI coinText;
    public GameObject reviveButton;
    public Image reviveFill;
    public float reviveTime;
    float curReviveTime;
    public Button returnTolobbyButton;
    [Header("- Animation")]
    public GameObject coinParent;
    public GameObject reviveParent;
    public GameObject lobbyButtonParent;
    public GameObject reviveInfoText;

    [Header("===== Gadget =====")]
    public GameObject gadgetSlotParent;
    public Image gadgetIcon;
    public TextMeshProUGUI gadgetAmount;
    [Header("===== Pause =====")]
    public GameObject pausePanel;
    public Button resumeBut;
    public Button settingBut;
    public GameObject settingBorder;
    public Button goBackToMenuBut;

    [Header("===== Boss =====")]
    public GameObject bossHPBar;
    public Image bossHPFill;
    [Header("===== CutScene =====")]
    public GameObject cutscenePanel;
    public GameObject top;
    public GameObject down;
    [Header("===== Progress Bar =====")]
    public GameObject[] progressPoint;

    private void OnDisable()
    {
        PlayerManager.Instance.onDead -= EnableDeadScene;

    }

    private void Awake()
    {
        Instance = this;

        FadeIn();

        deadScene.gameObject.SetActive(false);
    }

    private void Start()
    {
        PlayerManager.Instance.onDead += EnableDeadScene;

        returnTolobbyButton.onClick.AddListener(() => ReturnToLobby());
        goBackToMenuBut.onClick.AddListener(() => ReturnToLobby());
        resumeBut.onClick.AddListener(() => PauseManager.Instance.TogglePause());
        settingBut.onClick.AddListener(ToggleSettingBorder);
    }

    private void Update()
    {
        coinInGameText.text = PlayerManager.Instance.inGameCoin.ToString();

        float hpPercent = PlayerManager.Instance.currentHp / PlayerManager.Instance.maxHp;
        hpFill.fillAmount = hpPercent;

        float minutes = Mathf.FloorToInt(GameManager.Instance.curGameTime / 60);
        float seconds = Mathf.FloorToInt(GameManager.Instance.curGameTime % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

        if (deadScene.gameObject.activeSelf)
        {
            coinText.text = PlayerManager.Instance.inGameCoin.ToString();
            if (PlayerManager.reviveItemCount > 0 &&
                PlayerManager.Instance.curReviveCount < GameManager.Instance.maxRevivePerGame)
            {

                if (curReviveTime >= reviveTime)
                {
                    LeanTween.scale(reviveParent, new Vector3(0, 0, 0), 0.3f).setEaseInOutCubic()
                        .setOnComplete(() => GameManager.Instance.EnterBadCutScene());
                    deadScene.gameObject.SetActive(false);
                }
                else
                {
                    LeanTween.scale(reviveParent, new Vector3(1, 1, 1), 0.3f);
                    curReviveTime += Time.deltaTime;
                }

                float revivePercent = curReviveTime / reviveTime;
                reviveFill.fillAmount = revivePercent;
            }
            else
            {
                GameManager.Instance.EnterBadCutScene();
                deadScene.gameObject.SetActive(false);

            }
        }

        if (PlayerManager.Instance.gadgetSlot.gadget != null)
        {
            gadgetSlotParent.SetActive(true);
            gadgetIcon.sprite = PlayerManager.Instance.gadgetSlot.gadget.gadgetSprint;
            gadgetAmount.text = PlayerManager.Instance.gadgetSlot.amount.ToString();
        }
        else
        {
            gadgetSlotParent.SetActive(false);
        }

        if (GameManager.Instance.state != GameState.BossFight)
        {
            bossHPBar.gameObject.SetActive(false);
        }
        else
        {
            if (GameManager.Instance.curBoss != null)
            {
                BossController boss = GameManager.Instance.curBoss.GetComponent<BossController>();
                if (boss.curBehavior != BossBehavior.AfterSpawn && !boss.isDead &&
                    boss.curBehavior != BossBehavior.Escape)
                {
                    bossHPBar.gameObject.SetActive(true);
                }

                float max = boss.bossSO.maxHp;
                float cur = boss.hp;
                float per = cur / max;
                bossHPFill.fillAmount = per;
            }
            else
            {
                bossHPBar.gameObject.SetActive(false);
            }
        }

        UpdateDashTime();

    }

    public void FadeIn()
    {
        fadeCanvas.gameObject.SetActive(true);
        LeanTween.alphaCanvas(fadeCanvas, 0f, .25f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() => fadeCanvas.gameObject.SetActive(false));
    }

    public delegate void AfterFadeEvent();

    public void FadeOut(AfterFadeEvent afterFade)
    {
        fadeCanvas.gameObject.SetActive(true);
        LeanTween.alphaCanvas(fadeCanvas, 1f, 1f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() => afterFade?.Invoke());
    }

    void UpdateDashTime()
    {
        float cur = PlayerManager.Instance.curDashDelay;
        float max = PlayerManager.Instance.dashDelay;
        float per = Mathf.Abs((cur / max) - 1);
        if (per == 0) per = 1;
        dashFill.fillAmount = per;
    }

    public void ReviveBut()
    {
        if (PlayerManager.reviveItemCount > 0 &&
                PlayerManager.Instance.curReviveCount < GameManager.Instance.maxRevivePerGame
                && curReviveTime < reviveTime)
        {
            SoundManager.Instance.PlayOnShot("Button");
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.revive);
            curReviveTime = 0;
            deadScene.gameObject.SetActive(false);
            coinInGameText.gameObject.SetActive(true);
        }

    }

    void EnableDeadScene()
    {
        deadScene.gameObject.SetActive(true);


        LeanTween.scale(coinParent, new Vector3(1, 1, 1), 0.3f);
        LeanTween.scale(reviveInfoText, new Vector3(1, 1, 1), 1.25f).setLoopClamp();
        coinInGameText.gameObject.SetActive(false);
    }

    void ReturnToLobby()
    {
        if (PauseManager.Instance.IsPauseState())
            Time.timeScale = 1f;

        SoundManager.Instance.PlayOnShot("Button");
        PlayerManager.coin += PlayerManager.Instance.inGameCoin;
        SaveSystem.Save();
        LeanTween.alphaCanvas(fadeCanvas, 1, .3f)
             .setEase(LeanTweenType.easeInOutCubic)
             .setOnComplete(() => SceneManager.LoadScene(0));
    }

    public void EnterCutScene()
    {
        Vector3 topYPos = new Vector3(0, (Screen.height / 2), 0);
        Vector3 downYPos = new Vector3(0, -(Screen.height / 2), 0);

        cutscenePanel.gameObject.SetActive(true);
        LeanTween.moveLocal(top, topYPos, 0.5f).setEaseInOutCubic();
        LeanTween.moveLocal(down, downYPos, 0.5f).setEaseInOutCubic();

        hpBorder.gameObject.SetActive(false);
        dashBorder.gameObject.SetActive(false);
        coinBorder.gameObject.SetActive(false);
        gadgetSlotParent.gameObject.SetActive(false);
        bossHPBar.gameObject.SetActive(false);

        InputSystemMnanger input = PlayerManager.Instance.transform.GetComponent<InputSystemMnanger>();
        input.enabled = false;

    }

    public void ExitCutScene()
    {
        Vector3 topYPos = new Vector3(0, (Screen.height / 2) + 100, 0);
        Vector3 downYPos = new Vector3(0, -(Screen.height / 2) - 100, 0);

        LeanTween.moveLocal(top, topYPos, 0.5f).setEaseInOutCubic();
        LeanTween.moveLocal(down, downYPos, 0.5f)
            .setEaseInOutCubic()
            .setOnComplete(() => EneableUIInfo());
    }

    void EneableUIInfo()
    {
        cutscenePanel.gameObject.SetActive(false);
        hpBorder.gameObject.SetActive(true);
        dashBorder.gameObject.SetActive(true);
        coinBorder.gameObject.SetActive(true);
        gadgetSlotParent.gameObject.SetActive(true);
        bossHPBar.gameObject.SetActive(true);

        InputSystemMnanger input = PlayerManager.Instance.transform.GetComponent<InputSystemMnanger>();
        input.enabled = true;
    }

    void DisableAllProgressPoint()
    {
        foreach (GameObject g in progressPoint)
        {
            g.SetActive(false);
        }
    }

    public void EnableProgressPoint(int Phase)
    {
        DisableAllProgressPoint();
        progressPoint[Phase].gameObject.SetActive(true);
    }

    void ToggleSettingBorder()
    {
        if (settingBorder.transform.localScale.y == 0)
        {
            settingBorder.SetActive(true);
            LeanTween.scaleY(settingBorder.gameObject, 1, 0.5f).setEaseInOutCubic();
            LeanTween.move(goBackToMenuBut.GetComponent<RectTransform>(),
                new Vector3(0, -400, 0), 0.5f).setEaseInOutCubic();
        }
        else if (settingBorder.transform.localScale.y > 0)
        {
            LeanTween.scaleY(settingBorder.gameObject, 0, 0.5f).setEaseInOutCubic();
            LeanTween.move(goBackToMenuBut.GetComponent<RectTransform>(),
                new Vector3(0, -100, 0), 0.5f).setEaseInOutCubic()
                .setOnComplete(() => settingBorder.SetActive(false));
        }
    }

}
