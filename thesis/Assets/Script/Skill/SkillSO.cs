using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillSO")]
public class SkillSO : ScriptableObject
{
    public int skillID;
    public Sprite skillIcon;
    public string skillName;
    public string skillDiscription;

    [Header("Skill Detail")]
    public List<SkillLevelAndDelay> skillLevelAndDelays = new List<SkillLevelAndDelay>();
}

[Serializable]
public class SkillLevelAndDelay
{
    public float delay;
    public float level;
}