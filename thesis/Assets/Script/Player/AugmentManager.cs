using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviour
{
    public List<AugmentSlot> skillInventory = new List<AugmentSlot>();

    private void Update()
    {
        if (skillInventory.Count > 0)
        {
            for (int i = 0; i < skillInventory.Count; i++)
            {
                AugmentSlot slot = skillInventory[i];
                if (!slot.ready)
                {
                    slot.delay -= Time.deltaTime;
                    if (slot.delay < 0)
                    {
                        SkillSO skill = slot.skill;
                        int level = slot.level;
                        float delayTime = skill.skillLevelAndDelays[level - 1].delay;

                        slot.delay = delayTime;
                        slot.ready = true;
                    }
                }
            }
        }

        if (HasSkill(1, out int counterSkillIndex))
        {
            AugmentSlot slot = skillInventory[counterSkillIndex];
            if (slot.ready)
            {
                PlayerManager.Instance.counterEffect.SetActive(true);
            }
            else
            {
                PlayerManager.Instance.counterEffect.SetActive(false);
            }
        }

        if (HasSkill(2, out int fireballSkillIndex))
        {
            AugmentSlot slot = skillInventory[fireballSkillIndex];
            if (slot.ready)
            {
                PlayerManager.Instance.projectileEffect.SetActive(true);
            }
            else
            {
                PlayerManager.Instance.projectileEffect.SetActive(false);
            }
        }

        if (HasSkill(3,out int shieldSkillIndex))
        {
            AugmentSlot slot = skillInventory[shieldSkillIndex];
            if (slot.ready)
            {
                PlayerManager.Instance.shieldEffect.SetActive(true);
            }
            else
            {
                PlayerManager.Instance.shieldEffect.SetActive(false);
            }
        }

        if (HasSkill(4,out int VampireskillIndex))
        {
            AugmentSlot slot = skillInventory[VampireskillIndex];
            if(slot.ready)
            {
                SkillSO skill = slot.skill;
                int level = slot.level;
                float delayTime = skill.skillLevelAndDelays[level - 1].delay;

                if(PlayerManager.Instance.Heal())
                {
                    slot.delay = delayTime;
                    slot.ready = false;
                }

            }
        }

    }

    public void AddSkill(SkillSO skill)
    {
        if (TryGetSkill(skill, out int index))
        {
            skillInventory[index].level++;
        }
        else
        {
            AugmentSlot newSlot = new AugmentSlot();
            newSlot.skill = skill;
            newSlot.level = 1;
            newSlot.delay = 0;
            newSlot.ready = true;

            skillInventory.Add(newSlot);
        }
    }

    bool TryGetSkill(SkillSO skill, out int index)
    {
        if (skillInventory.Count > 0)
        {
            for (int i = 0; i < skillInventory.Count; i++)
            {
                AugmentSlot slot = skillInventory[i];
                if (slot.skill == skill)
                {
                    index = i;
                    return true;
                }
            }
        }
        index = -1;
        return false;
    }

    public bool CanAddOrLevelUpCard()
    {
        int maxCount = 0;
        if (skillInventory.Count == PlayerManager.Instance.maxSkillCount)
        {
            foreach (AugmentSlot slot in skillInventory)
            {
                if (slot.level == slot.skill.skillLevelAndDelays.Count)
                    maxCount++;
            }
            return maxCount != skillInventory.Count;
        }

        return true;
    }

    public bool HasSkill(int skillId, out int skillIndex)
    {
        if (skillInventory.Count > 0)
        {
            for (int i = 0; i < skillInventory.Count; i++)
            {
                if (skillInventory[i].skill.skillID == skillId)
                {
                    skillIndex = i;
                    return true;
                }
            }
        }
        skillIndex = -1;
        return false;
    }

}

[Serializable]
public class AugmentSlot
{
    public SkillSO skill;
    public int level;
    public float delay;
    public bool ready;
}
