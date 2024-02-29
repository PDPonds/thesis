using System.Collections;
using System.Collections.Generic;
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
        LeanTween.alphaCanvas(fadeCanvas, 0, .25f);
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
                reviveButton.gameObject.SetActive(true);
                returnTolobbyButton.gameObject.SetActive(false);
                curReviveTime += Time.deltaTime;
                if (curReviveTime >= reviveTime)
                {
                    reviveButton.gameObject.SetActive(false);
                    returnTolobbyButton.gameObject.SetActive(true);
                }
                float revivePercent = curReviveTime / reviveTime;
                reviveFill.fillAmount = revivePercent;
            }
            else
            {
                reviveButton.gameObject.SetActive(false);
                returnTolobbyButton.gameObject.SetActive(true);
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
        if (reviveButton.gameObject.activeSelf)
        {
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.revive);
            curReviveTime = 0;
            deadScene.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);
            coinInGameText.gameObject.SetActive(true);
        }
    }

    void EnableDeadScene()
    {
        deadScene.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        coinInGameText.gameObject.SetActive(false);
    }

    void ReturnToLobby()
    {
        PlayerManager.coin += PlayerManager.Instance.inGameCoin;
        SceneManager.LoadScene(0);
    }

}
