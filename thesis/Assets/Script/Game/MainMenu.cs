using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("========== Start")]
    [SerializeField] Button startButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject upgradeUI;
    [SerializeField] Button closeButton;
    [SerializeField] CanvasGroup fadeCanvas;
    [Header("========== Setting")]
    [SerializeField] Button settingBut;
    [SerializeField] Button closeSettingBut;
    [SerializeField] GameObject setting;
    [SerializeField] GameObject settingBorder;
    [Header("========== ShopUI")]
    [Header("- GameObject")]
    [SerializeField] GameObject shopBorder;
    [SerializeField] GameObject[] maxHpLevelObj;
    [SerializeField] GameObject[] stealHpLevelObj;
    [Header("- Text")]
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI upgardeMaxHpCostText;
    [SerializeField] TextMeshProUGUI upgardeStealHpCostText;
    [SerializeField] TextMeshProUGUI reviveCostText;
    [SerializeField] TextMeshProUGUI reviveCount;
    [Header("- Button")]
    [SerializeField] Button upgradeMaxHpBut;
    [SerializeField] Button upgradeStealHpBut;
    [SerializeField] Button buyReviveBut;
    [SerializeField] List<int> upgardeMaxHpCostPerLevel = new List<int>();
    [SerializeField] List<int> upgardeStealHpCostPerLevel = new List<int>();
    [SerializeField] int reviveCost;

    [Header("========== ExitUI")]
    [SerializeField] GameObject confirmPanel;
    [SerializeField] GameObject confirmBorder;
    [SerializeField] Button confirmBut;
    [SerializeField] Button cancleBut;
    [Header("========== Test Boss")]
    public Button testBossBut;

    private void OnEnable()
    {
        shopButton.onClick.AddListener(() => ShowShop());
        exitButton.onClick.AddListener(() => ShowConfirmToExitBut());
        confirmBut.onClick.AddListener(() => ExitGame());
        cancleBut.onClick.AddListener(() => CancleExitBut());

        closeButton.onClick.AddListener(() => CloseShop());

        upgradeMaxHpBut.onClick.AddListener(() => UpgradeMaxHpBut());
        upgradeStealHpBut.onClick.AddListener(() => UpgradeStealHPBut());
        buyReviveBut.onClick.AddListener(() => BuyReviveItem());

        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        startButton.onClick.AddListener(() => StartGame());

        settingBut.onClick.AddListener(ShowSetting);
        closeSettingBut.onClick.AddListener(() => CloseSetting());

        testBossBut.onClick.AddListener(() => TestBoss());
    }

    private void Awake()
    {
        SaveSystem.Load();
        GameManager.isTestBoss = false;
    }

    private void Start()
    {
        SoundManager.Instance.Pause("BossBGM");
        SoundManager.instance.Pause("City");
        SoundManager.Instance.Play("NormalBGM");
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (PlayerManager.coin >= upgardeMaxHpCostPerLevel[PlayerManager.upgradeMaxHpLevel] &&
            PlayerManager.upgradeMaxHpLevel < upgardeMaxHpCostPerLevel.Count)
        {
            upgradeMaxHpBut.interactable = true;
        }
       else upgradeMaxHpBut.interactable = false;

        if (PlayerManager.coin >= upgardeStealHpCostPerLevel[PlayerManager.upgradeStealHpLevel] &&
            PlayerManager.upgradeStealHpLevel < upgardeStealHpCostPerLevel.Count)
        {
            upgradeStealHpBut.interactable = true;
        }
        else upgradeStealHpBut.interactable = false;

        if (PlayerManager.coin >= reviveCost)
        {
            buyReviveBut.interactable = true;
        }
        else buyReviveBut.interactable = false;

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);

        if (shopBorder.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseShop();

            }
        }

        if (confirmBorder.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancleExitBut();
            }
        }
    }

    #region Function

    void ShowSetting()
    {
        SoundManager.Instance.PlayOnShot("Button");

        setting.SetActive(true);
        LeanTween.scale(settingBorder.GetComponent<RectTransform>(), Vector2.one, .3f)
            .setEaseInOutCubic();
    }

    void CloseSetting()
    {
        SoundManager.Instance.PlayOnShot("Button");

        LeanTween.scale(settingBorder.GetComponent<RectTransform>(), Vector2.zero, .3f)
            .setEaseInOutCubic()
            .setOnComplete(() => setting.SetActive(false));
    }

    void UpgradeMaxHpBut()
    {
        if (PlayerManager.coin >= upgardeMaxHpCostPerLevel[PlayerManager.upgradeMaxHpLevel] &&
            PlayerManager.upgradeMaxHpLevel < upgardeMaxHpCostPerLevel.Count)
        {
            SoundManager.Instance.PlayOnShot("Button");
            PlayerManager.coin -= upgardeMaxHpCostPerLevel[PlayerManager.upgradeMaxHpLevel];
            PlayerManager.upgradeMaxHpLevel++;
            SaveSystem.Save();
            UpdateMaxHPInfo();
        }
    }

    void UpgradeStealHPBut()
    {
        if (PlayerManager.coin >= upgardeStealHpCostPerLevel[PlayerManager.upgradeStealHpLevel] &&
            PlayerManager.upgradeStealHpLevel < upgardeStealHpCostPerLevel.Count)
        {
            SoundManager.Instance.PlayOnShot("Button");
            PlayerManager.coin -= upgardeStealHpCostPerLevel[PlayerManager.upgradeStealHpLevel];
            PlayerManager.upgradeStealHpLevel++;
            SaveSystem.Save();
            UpdateStealHPInfo();

        }
    }

    void BuyReviveItem()
    {
        if (PlayerManager.coin >= reviveCost)
        {
            SoundManager.Instance.PlayOnShot("Button");
            PlayerManager.coin -= reviveCost;
            PlayerManager.reviveItemCount++;
            SaveSystem.Save();
            UpdateReviveItem();
        }
    }

    void UpdateUpgradeInfo()
    {
        UpdateCoin();
        UpdateReviveItem();
        UpdateMaxHPInfo();
        UpdateStealHPInfo();
    }

    void UpdateMaxHPInfo()
    {
        foreach (GameObject go in maxHpLevelObj)
        {
            Image image = go.GetComponent<Image>();
            image.color = Color.white;
        }

        for (int i = 0; i < PlayerManager.upgradeMaxHpLevel; i++)
        {
            Image image = maxHpLevelObj[i].GetComponent<Image>();
            image.color = Color.green;
        }

        upgardeMaxHpCostText.text = $"{upgardeMaxHpCostPerLevel[PlayerManager.upgradeMaxHpLevel].ToString()} <sprite=0>";
        UpdateCoin();

    }

    void UpdateStealHPInfo()
    {
        foreach (GameObject go in stealHpLevelObj)
        {
            Image image = go.GetComponent<Image>();
            image.color = Color.white;
        }

        for (int i = 0; i < PlayerManager.upgradeStealHpLevel; i++)
        {
            Image image = stealHpLevelObj[i].GetComponent<Image>();
            image.color = Color.green;
        }

        upgardeStealHpCostText.text = $"{upgardeStealHpCostPerLevel[PlayerManager.upgradeStealHpLevel].ToString()} <sprite=0>";
        UpdateCoin();

    }

    void UpdateReviveItem()
    {
        reviveCostText.text = $"{reviveCost.ToString()} <sprite=0>";
        reviveCount.text = PlayerManager.reviveItemCount.ToString();
        UpdateCoin();
    }

    void ShowShop()
    {
        SoundManager.Instance.PlayOnShot("Button");
        ToggleUI(upgradeUI, true);
        UpdateUpgradeInfo();
        LeanTween.moveLocal(shopBorder, new Vector3(0, 0, 0), .5f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(SelectUpgradeBut);
    }

    void SelectUpgradeBut()
    {
        EventSystem.current.SetSelectedGameObject(upgradeMaxHpBut.gameObject);
    }

    void CloseShop()
    {
        SoundManager.Instance.PlayOnShot("Button");
        LeanTween.moveLocal(shopBorder, new Vector3(0, -1000, 0), 0.5f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() => ToggleUI(upgradeUI, false));
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    void UpdateCoin()
    {
        coinText.text = $"{PlayerManager.coin.ToString()}";
    }

    void ToggleUI(GameObject ui, bool show)
    {
        ui.SetActive(show);
    }

    void StartGame()
    {
        SoundManager.Instance.PlayOnShot("Button");
        LeanTween.alphaCanvas(fadeCanvas, 1, 0.25f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() => SceneManager.LoadScene(1));

    }

    void ShowConfirmToExitBut()
    {
        SoundManager.Instance.PlayOnShot("Button");
        ToggleUI(confirmPanel, true);
        LeanTween.moveLocal(confirmBorder, new Vector3(0, 0, 0), .5f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(SelectConfirmBut);
    }

    void SelectConfirmBut()
    {
        EventSystem.current.SetSelectedGameObject(confirmBut.gameObject);
    }

    void CancleExitBut()
    {
        SoundManager.Instance.PlayOnShot("Button");
        LeanTween.moveLocal(confirmBorder, new Vector3(0, -700, 0), 0.5f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() => ToggleUI(confirmPanel, false));
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    void ExitGame()
    {
        SoundManager.Instance.PlayOnShot("Button");
        SaveSystem.Save();
        Application.Quit();
    }

    #endregion

    void TestBoss()
    {
        GameManager.isTestBoss = true;
        StartGame();
    }

}
