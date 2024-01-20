using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    public SkillSO skillSO;
    public Image image;
    public Image fill;
    public TextMeshProUGUI levelText;

    private void Update()
    {
        AugmentManager agM = PlayerManager.Instance.augmentManager;
        if (agM.HasSkill(skillSO.skillID, out int skillIndex))
        {
            AugmentSlot slot = agM.skillInventory[skillIndex];
            int level = slot.level;

            if (slot.ready)
            {
                fill.gameObject.SetActive(false);
            }
            else
            {
                fill.gameObject.SetActive(true);
                float currentTime = slot.delay;
                float maxTime = slot.skill.skillLevelAndDelays[level - 1].delay;

                float percent = currentTime / maxTime;
                float invertPercent = (percent + 1) - percent * 2;
                fill.fillAmount = invertPercent;
            }

        }
    }

    public void UpdateLevelAndIcon(SkillSO skill, Sprite icon, int level)
    {
        skillSO = skill;
        image.sprite = icon;
        levelText.text = level.ToString();
    }

}
