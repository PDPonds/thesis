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
    [SerializeField] Button exitButton;
    [SerializeField] GameObject upgradeUI;
    [SerializeField] Button closeButton;
    [SerializeField] Button runButton;

    [Header("========== UpgardUI")]
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
    [Header("- GameObject")]
    [SerializeField] GameObject[] maxHpLevelObj;
    [SerializeField] GameObject[] stealHpLevelObj;

    [SerializeField] List<int> upgardeMaxHpCostPerLevel = new List<int>();
    [SerializeField] List<int> upgardeStealHpCostPerLevel = new List<int>();
    [SerializeField] int reviveCost;

    private void OnEnable()
    {
        startButton.onClick.AddListener(() => ClickStart());
        exitButton.onClick.AddListener(() => ExitGame());

        closeButton.onClick.AddListener(() => ToggleUI(upgradeUI, false));

        upgradeMaxHpBut.onClick.AddListener(() => UpgradeMaxHpBut());
        upgradeStealHpBut.onClick.AddListener(() => UpgradeStealHPBut());
        buyReviveBut.onClick.AddListener(() => BuyReviveItem());

        runButton.onClick.AddListener(() => StartGame());
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

    void UpgradeMaxHpBut()
    {
        if (PlayerManager.coin >= upgardeMaxHpCostPerLevel[PlayerManager.upgradeMaxHpLevel] &&
            PlayerManager.upgradeMaxHpLevel < upgardeMaxHpCostPerLevel.Count)
        {
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
            PlayerManager.coin -= upgardeStealHpCostPerLevel[PlayerManager.upgradeStealHpLevel];
            PlayerManager.upgradeStealHpLevel++;
            UpdateStealHPInfo();

        }
    }

    void BuyReviveItem()
    {
        if (PlayerManager.coin >= reviveCost)
        {
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

        upgardeMaxHpCostText.text = upgardeMaxHpCostPerLevel[PlayerManager.upgradeMaxHpLevel].ToString();
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

        upgardeStealHpCostText.text = upgardeStealHpCostPerLevel[PlayerManager.upgradeStealHpLevel].ToString();
        UpdateCoin();

    }

    void UpdateReviveItem()
    {
        reviveCostText.text = reviveCost.ToString();
        reviveCount.text = PlayerManager.reviveItemCount.ToString();
        UpdateCoin();
    }

    void ClickStart()
    {
        ToggleUI(upgradeUI, true);
        UpdateUpgradeInfo();
    }

    void UpdateCoin()
    {
        coinText.text = PlayerManager.coin.ToString();
    }

    void ToggleUI(GameObject ui, bool show)
    {
        ui.SetActive(show);
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void ExitGame()
    {
        Application.Quit();
    }

}
