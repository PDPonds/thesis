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
        if (isPause) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

}
