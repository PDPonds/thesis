using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecreasesHPSetting : MonoBehaviour
{
    [SerializeField] Toggle decreasesToggle;

    private void Awake()
    {
        SetDecreases();
        decreasesToggle.onValueChanged.AddListener(delegate
        {
            SetDecreases();
        });
    }

    public void SetDecreases()
    {
        bool decreases = decreasesToggle.isOn;
        UIManager.Instance.isDecreases = decreases;
    }


}
