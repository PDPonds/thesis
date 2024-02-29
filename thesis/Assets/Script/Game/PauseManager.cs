using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    bool isPause;

    private void Awake()
    {
        Instance = this;
    }

    public void TogglePause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0f;
            UIManager.Instance.pausePanel.SetActive(true);

        }
        else
        {
            Time.timeScale = 1f;
            UIManager.Instance.pausePanel.SetActive(false);

        }
    }

}
