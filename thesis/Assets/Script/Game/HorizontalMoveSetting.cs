using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalMoveSetting : MonoBehaviour
{
    [SerializeField] Toggle horizontalMoveToggle;

    private void Awake()
    {
        SetDecreases();
        horizontalMoveToggle.onValueChanged.AddListener(delegate
        {
            SetDecreases();
        });
    }

    public void SetDecreases()
    {
        bool horizontalMove = horizontalMoveToggle.isOn;
        UIManager.Instance.isHorizontalMove = horizontalMove;
    }

}
