using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        startButton.onClick.AddListener(() => StartGame());
    }

    private void Start()
    {
        SoundManager.Instance.Pause("BossBGM");
        SoundManager.Instance.Play("NormalBGM");
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

    }

    #region Function
    void UpgradeMaxHpBut()
    {
        if (PlayerManager.coin >= upgardeMaxHpCostPerLevel[PlayerManager.upgradeMaxHpLevel] &&
            PlayerManager.upgradeMaxHpLevel < upgardeMaxHpCostPerLevel.Count)
        {
            SoundManager.Instance.PlayOnShot("Button");
            PlayerManager.coin -= upgardeMaxHpCostPerLevel[PlayerManager.upgradeMaxHpLevel];
            PlayerManager.upgradeMaxHpLevel++;
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
            .setEase(LeanTweenType.easeInOutCubic);
    }

    void CloseShop()
    {
        SoundManager.Instance.PlayOnShot("Button");
        LeanTween.moveLocal(shopBorder, new Vector3(0, -1000, 0), 0.5f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() => ToggleUI(upgradeUI, false));
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
            .setEase(LeanTweenType.easeInOutCubic);
    }

    void CancleExitBut()
    {
        SoundManager.Instance.PlayOnShot("Button");
        LeanTween.moveLocal(confirmBorder, new Vector3(0, -700, 0), 0.5f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() => ToggleUI(confirmPanel, false));
    }

    void ExitGame()
    {
        SoundManager.Instance.PlayOnShot("Button");
        Application.Quit();
    }

    #endregion


}
