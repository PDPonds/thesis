using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;

    private void OnEnable()
    {
        startButton.onClick.AddListener(() => StartGame());
    }


    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

}
