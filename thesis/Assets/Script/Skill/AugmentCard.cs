using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentCard : MonoBehaviour
{
    public SkillSO skill;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDiscription;
    public Image cardIcon;

    public void SetupCardInfo(string name, string discription, Sprite icon)
    {
        cardName.text = name;
        cardDiscription.text = discription;
        cardIcon.sprite = icon;
    }

}
