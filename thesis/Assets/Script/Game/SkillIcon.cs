using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI levelText;

    public void UpdateLevelAndIcon(Sprite icon, int level)
    {
        image.sprite = icon;
        levelText.text = level.ToString();
    }

}
