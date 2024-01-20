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
    public float delay;

}
