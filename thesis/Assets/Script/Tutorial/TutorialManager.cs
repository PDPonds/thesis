using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialState
{
    Jump, Slide, Dash, Attack, Counter, Shuriken, Smoke, Mine
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public TutorialState tutorialState;

    [SerializeField] GameObject tutorailInfo;
    [SerializeField] TextMeshProUGUI tutorialText;

    [HideInInspector]
    public bool isTutorialPause;

    [Header("===== Info =====")]
    [Header("- Jump")]
    [SerializeField] string JumpText;
    [Header("- Slide")]
    [SerializeField] string SlideText;
    [Header("- Dash")]
    [SerializeField] string DashText;
    [Header("- Attack")]
    [SerializeField] string AttackText;
    [Header("- Counter")]
    [SerializeField] string CounterText;
    [Header("- Shuriken")]
    [SerializeField] string ShurikenText;
    [Header("- Smoke")]
    [SerializeField] string SmokeText;
    [Header("- Mine")]
    [SerializeField] string MineText;

    private void Awake()
    {
        instance = this;
    }

    public void ShowTutorial()
    {
        tutorailInfo.SetActive(true);

        switch (tutorialState)
        {
            case TutorialState.Jump:
                tutorialText.text = JumpText;
                break;
            case TutorialState.Slide:
                tutorialText.text = SlideText;
                break;
            case TutorialState.Dash:
                tutorialText.text = DashText;
                break;
            case TutorialState.Attack:
                tutorialText.text = AttackText;
                break;
            case TutorialState.Counter:
                tutorialText.text = CounterText;
                break;
            case TutorialState.Shuriken:
                tutorialText.text = ShurikenText;
                break;
            case TutorialState.Smoke:
                tutorialText.text = SmokeText;
                break;
            case TutorialState.Mine:
                tutorialText.text = MineText;
                break;
        }

        Time.timeScale = 0.5f;
        isTutorialPause = true;

    }

    public void NextTutorial()
    {
        switch (tutorialState)
        {
            case TutorialState.Jump:
                tutorialState = TutorialState.Slide;
                break;
            case TutorialState.Slide:
                tutorialState = TutorialState.Dash;
                break;
            case TutorialState.Dash:
                tutorialState = TutorialState.Attack;
                break;
            case TutorialState.Attack:
                tutorialState = TutorialState.Counter;
                break;
            case TutorialState.Counter:
                tutorialState = TutorialState.Shuriken;
                break;
            case TutorialState.Shuriken:
                tutorialState = TutorialState.Smoke;
                break;
            case TutorialState.Smoke:
                tutorialState = TutorialState.Mine;
                break;
            case TutorialState.Mine:
                PlayerManager.passTutorial = true;
                SaveSystem.Save();
                GameManager.Instance.curFloorIndex = 0;
                GameManager.Instance.curMap = GameManager.Instance.normalMap[Random.Range(0, GameManager.Instance.normalMap.Length)];
                break;
        }

        tutorailInfo.SetActive(false);

        //PauseManager.Instance.ToggleEnableScript(true);
        Time.timeScale = 1f;
        isTutorialPause = false;

    }

    public bool IsTutorial(TutorialState state)
    {
        return tutorialState == state;
    }

}
