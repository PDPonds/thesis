using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("===== HP =====")]
    [SerializeField] Image hpFill;
    [Header("===== Fade =====")]
    [SerializeField] CanvasGroup fadeCanvas;

    [Header("===== Score Coin =====")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinInGameText;

    [Header("===== Dead Scene =====")]
    public Transform deadScene;
    public TextMeshProUGUI scoreOnDeadScene;
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
    public Button goBackToMenuBut;

    private void OnDisable()
    {
        PlayerManager.Instance.onDead -= EnableDeadScene;

    }

    private void Awake()
    {
        Instance = this;
        LeanTween.alphaCanvas(fadeCanvas, 0, .25f)
            .setEase(LeanTweenType.easeInOutCubic);

        deadScene.gameObject.SetActive(false);
    }

    private void Start()
    {
        PlayerManager.Instance.onDead += EnableDeadScene;

        returnTolobbyButton.onClick.AddListener(() => ReturnToLobby());
        goBackToMenuBut.onClick.AddListener(() => ReturnToLobby());
        resumeBut.onClick.AddListener(() => PauseManager.Instance.TogglePause());
    }

    private void Update()
    {
        scoreText.text = GameManager.Instance.currentScore.ToString();
        scoreOnDeadScene.text = GameManager.Instance.currentScore.ToString();
        coinInGameText.text = PlayerManager.Instance.inGameCoin.ToString();

        float hpPercent = PlayerManager.Instance.currentHp / PlayerManager.Instance.maxHp;
        hpFill.fillAmount = hpPercent;

        if (deadScene.gameObject.activeSelf)
        {
            coinText.text = PlayerManager.Instance.inGameCoin.ToString();
            if (PlayerManager.reviveItemCount > 0 &&
                PlayerManager.Instance.curReviveCount < GameManager.Instance.maxRevivePerGame)
            {
                //reviveButton.gameObject.SetActive(true);
                //returnTolobbyButton.gameObject.SetActive(false);

                if (curReviveTime >= reviveTime)
                {
                    LeanTween.scale(reviveParent, new Vector3(0, 0, 0), 0.3f);
                    LeanTween.scale(lobbyButtonParent, new Vector3(1, 1, 1), 0.3f);

                    //reviveButton.gameObject.SetActive(false);
                    //returnTolobbyButton.gameObject.SetActive(true);
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
                //reviveButton.gameObject.SetActive(false);
                //returnTolobbyButton.gameObject.SetActive(true);
                LeanTween.scale(reviveParent, new Vector3(0, 0, 0), 0.3f);
                LeanTween.scale(lobbyButtonParent, new Vector3(1, 1, 1), 0.3f);

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

    }

    public void ReviveBut()
    {
        PlayerManager.Instance.SwitchState(PlayerManager.Instance.revive);
        curReviveTime = 0;
        deadScene.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        coinInGameText.gameObject.SetActive(true);
    }

    void EnableDeadScene()
    {
        deadScene.gameObject.SetActive(true);

        LeanTween.scale(scoreOnDeadScene.gameObject, new Vector3(1, 1, 1), 0.3f);
        LeanTween.scale(coinParent, new Vector3(1, 1, 1), 0.3f);
        LeanTween.scale(reviveInfoText, new Vector3(1, 1, 1), 1.25f).setLoopClamp();
        scoreText.gameObject.SetActive(false);
        coinInGameText.gameObject.SetActive(false);
    }

    void ReturnToLobby()
    {
        PlayerManager.coin += PlayerManager.Instance.inGameCoin;
        LeanTween.alphaCanvas(fadeCanvas, 1, .3f)
             .setEase(LeanTweenType.easeInOutCubic)
             .setOnComplete(() => SceneManager.LoadScene(0));
    }

}
