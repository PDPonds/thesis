using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("===== HP =====")]
    public Transform hpParent;
    public GameObject heartPrefab;

    [Header("===== Score =====")]
    public TextMeshProUGUI scoreText;

    [Header("===== Dead Scene =====")]
    public Transform deadScene;
    public TextMeshProUGUI scoreOnDeadScene;
    public Button returnTolobbyButton;
    [Header("===== Level =====")]
    public Image fillLevel;
    public TextMeshProUGUI levelText;

    [Header("===== Augment =====")]
    public GameObject AugmentUI;
    public Transform skillInventory;
    public GameObject skillImagePrefab;

    [Header("===== Momentum =====")]
    public GameObject momentumEffect;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        PlayerManager.Instance.onTakeDamage -= RemoveHeartUI;
        PlayerManager.Instance.onDead -= EnableDeadScene;
        PlayerManager.Instance.onHeal -= AddHeartUI;

        PlayerManager.Instance.onLevelup -= LevelUp;

        PlayerManager.Instance.onGetExp -= UpdateExp;

    }

    private void Awake()
    {
        deadScene.gameObject.SetActive(false);
        AugmentUI.gameObject.SetActive(false);
        
    }

    private void Start()
    {
        PlayerManager.Instance.onTakeDamage += RemoveHeartUI;
        PlayerManager.Instance.onDead += EnableDeadScene;
        PlayerManager.Instance.onHeal += AddHeartUI;

        PlayerManager.Instance.onLevelup += LevelUp;

        PlayerManager.Instance.onGetExp += UpdateExp;

        returnTolobbyButton.onClick.AddListener(() => ReturnToLobby());

        UpdateExp();
    }

    private void Update()
    {
        scoreText.text = GameManager.Instance.currentScore.ToString();
        scoreOnDeadScene.text = GameManager.Instance.currentScore.ToString();

        if (GameManager.Instance.isMomentum) momentumEffect.gameObject.SetActive(true);
        else momentumEffect.gameObject.SetActive(false);

    }

    void AddHeartUI()
    {
        GameObject heart = Instantiate(heartPrefab, hpParent, false);
    }

    void RemoveHeartUI()
    {
        if (hpParent.childCount > 0)
        {
            Destroy(hpParent.GetChild(0).gameObject);
        }
    }

    void EnableDeadScene()
    {
        deadScene.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        hpParent.gameObject.SetActive(false);
    }

    void ReturnToLobby()
    {
        SceneManager.LoadScene(0);
    }

    void OpenAugmentSelectionUI()
    {
        Time.timeScale = 0f;
        AugmentUI.SetActive(true);
    }

    void UpdateExp()
    {
        float max = PlayerManager.Instance.expTarget;
        float current = PlayerManager.Instance.currentExp;

        float percent = current / max;
        fillLevel.fillAmount = percent;

        levelText.text = $"Lv. {PlayerManager.Instance.currentLevel}";

    }

    void SetupAugmentCard()
    {
        List<SkillSO> currentSkillInCard = new List<SkillSO>();
        int count = GameManager.Instance.augmentCountPerLevelUp;
        for (int i = 0; i < count; i++)
        {
            AugmentManager augmentManager = PlayerManager.Instance.augmentManager;
            if (augmentManager.skillInventory.Count < PlayerManager.Instance.maxSkillCount)
            {
                int skillIndex = Random.Range(0, GameManager.Instance.allSkill.Length);
                SkillSO skill = GameManager.Instance.allSkill[skillIndex];
                if (currentSkillInCard.Count > 0)
                {
                    if (GameManager.Instance.CheckNoRepeatSkill(currentSkillInCard, skill))
                    {
                        GameObject card = GenerateCard();

                        currentSkillInCard.Add(skill);
                        AugmentCard augmentCard = card.GetComponent<AugmentCard>();
                        augmentCard.skill = skill;

                        augmentCard.SetupCardInfo(skill.name, skill.skillDiscription, skill.skillIcon);

                        Button button = card.GetComponent<Button>();
                        button.onClick.AddListener(() => AugmentCardClick(skill));
                    }
                    else
                    {
                        i--;
                    }
                }
                else
                {
                    GameObject card = GenerateCard();

                    currentSkillInCard.Add(skill);
                    AugmentCard augmentCard = card.GetComponent<AugmentCard>();
                    augmentCard.skill = skill;

                    augmentCard.SetupCardInfo(skill.name, skill.skillDiscription, skill.skillIcon);

                    Button button = card.GetComponent<Button>();
                    button.onClick.AddListener(() => AugmentCardClick(skill));
                }
            }
            else
            {
                int skillInSlotIndex = Random.Range(0, augmentManager.skillInventory.Count);
                SkillSO skillInSlot = augmentManager.skillInventory[skillInSlotIndex].skill;
                int currentSkillLevel = augmentManager.skillInventory[skillInSlotIndex].level;

                if (currentSkillLevel < skillInSlot.skillLevelAndDelays.Count)
                {
                    if (GameManager.Instance.CheckNoRepeatSkill(currentSkillInCard, skillInSlot))
                    {
                        GameObject card = GenerateCard();

                        currentSkillInCard.Add(skillInSlot);
                        AugmentCard augmentCard = card.GetComponent<AugmentCard>();
                        augmentCard.skill = skillInSlot;
                        augmentCard.SetupCardInfo(skillInSlot.name, skillInSlot.skillDiscription, skillInSlot.skillIcon);

                        Button button = card.GetComponent<Button>();
                        button.onClick.AddListener(() => AugmentCardClick(skillInSlot));
                    }
                    else
                    {
                        i--;
                    }
                }
                else
                {
                    continue;
                }
            }

        }

        currentSkillInCard.Clear();

    }

    GameObject GenerateCard()
    {
        GameObject cardPrefab = GameManager.Instance.augmentCardPrefab;
        GameObject card = Instantiate(cardPrefab, AugmentUI.transform);
        return card;
    }

    void AugmentCardClick(SkillSO skill)
    {
        AugmentManager augmentManager = PlayerManager.Instance.augmentManager;
        augmentManager.AddSkill(skill);

        Time.timeScale = 1;

        AugmentUI.SetActive(false);
        for (int i = 0; i < AugmentUI.transform.childCount; i++)
        {
            Destroy(AugmentUI.transform.GetChild(i).gameObject);
        }

        GenerateSkillIcon();

    }

    void GenerateSkillIcon()
    {
        if (skillInventory.childCount > 0)
        {
            for (int i = 0; i < skillInventory.childCount; i++)
            {
                Destroy(skillInventory.GetChild(i).gameObject);
            }
        }

        AugmentManager augmentManager = PlayerManager.Instance.augmentManager;

        for (int i = 0; i < augmentManager.skillInventory.Count; i++)
        {
            SkillSO skill = augmentManager.skillInventory[i].skill;
            int level = augmentManager.skillInventory[i].level;
            GameObject icon = Instantiate(skillImagePrefab, skillInventory);
            SkillIcon skillIcon = icon.GetComponent<SkillIcon>();
            skillIcon.UpdateLevelAndIcon(skill, skill.skillIcon, level);
        }

    }

    void LevelUp()
    {
        AugmentManager augmentManager = PlayerManager.Instance.augmentManager;
        if (augmentManager.CanAddOrLevelUpCard())
        {
            OpenAugmentSelectionUI();
            SetupAugmentCard();
        }
        UpdateExp();

    }

}
