using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSetting : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public Slider speedSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("speed")) LoadSpeed();
        else SetSpeed();

        speedSlider.onValueChanged.AddListener(delegate { SetSpeed(); });
    }

    public void SetSpeed()
    {
        float speed = speedSlider.value;
        GameManager.Instance.currentSpeed = speed;
        speedText.text = $"Speed : {speed.ToString("N1")}";
        PlayerPrefs.SetFloat("speed", speed);
    }

    public void LoadSpeed()
    {
        speedSlider.value = PlayerPrefs.GetFloat("speed");
        SetSpeed();
    }

}
