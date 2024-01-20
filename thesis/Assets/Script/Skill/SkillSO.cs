using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillSO")]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public string skillDiscription;
    public int skillID;

    [Header("Skill Detail")]
    public float delay;

}
