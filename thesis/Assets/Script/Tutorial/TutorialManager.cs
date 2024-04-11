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
    [SerializeField] Image tutorialImage;
    [SerializeField] TextMeshProUGUI tutorialText;

    [HideInInspector]
    public bool isTutorialPause;

    [Header("===== Info =====")]
    [Header("- Jump")]
    [SerializeField] Sprite JumpSprite;
    [SerializeField] string JumpText;
    [Header("- Slide")]
    [SerializeField] Sprite SlideSprite;
    [SerializeField] string SlideText;
    [Header("- Dash")]
    [SerializeField] Sprite DashSprite;
    [SerializeField] string DashText;
    [Header("- Attack")]
    [SerializeField] Sprite AttackSprite;
    [SerializeField] string AttackText;
    [Header("- Counter")]
    [SerializeField] Sprite CounterSprite;
    [SerializeField] string CounterText;
    [Header("- Shuriken")]
    [SerializeField] Sprite ShurikenSprite;
    [SerializeField] string ShurikenText;
    [Header("- Smoke")]
    [SerializeField] Sprite SmokeSprite;
    [SerializeField] string SmokeText;
    [Header("- Mine")]
    [SerializeField] Sprite MineSprite;
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
                tutorialImage.sprite = JumpSprite;
                tutorialText.text = JumpText;
                break;
            case TutorialState.Slide:
                tutorialImage.sprite = SlideSprite;
                tutorialText.text = SlideText;
                break;
            case TutorialState.Dash:
                tutorialImage.sprite = DashSprite;
                tutorialText.text = DashText;
                break;
            case TutorialState.Attack:
                tutorialImage.sprite = AttackSprite;
                tutorialText.text = AttackText;
                break;
            case TutorialState.Counter:
                tutorialImage.sprite = CounterSprite;
                tutorialText.text = CounterText;
                break;
            case TutorialState.Shuriken:
                tutorialImage.sprite = ShurikenSprite;
                tutorialText.text = ShurikenText;
                break;
            case TutorialState.Smoke:
                tutorialImage.sprite = SmokeSprite;
                tutorialText.text = SmokeText;
                break;
            case TutorialState.Mine:
                tutorialImage.sprite = MineSprite;
                tutorialText.text = MineText;
                break;
        }

        LeanTween.scale(tutorialImage.gameObject, Vector3.one, 0.5f)
            .setEaseInOutCubic();

        PauseManager.Instance.ToggleEnableScript(false);
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
                break;
        }

        LeanTween.scale(tutorialImage.gameObject, Vector3.zero, 0.5f)
            .setEaseInOutCubic().setOnComplete(() => tutorailInfo.SetActive(false));

        PauseManager.Instance.ToggleEnableScript(true);
        isTutorialPause = false;

    }

    public bool IsTutorial(TutorialState state)
    {
        return tutorialState == state;
    }

}
