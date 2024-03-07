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
        if (!UIManager.Instance.deadScene.gameObject.activeSelf &&
            !PlayerManager.Instance.isDead)
        {
            isPause = !isPause;
            if (isPause)
            {
                SoundManager.Instance.PlayOnShot("Button");
                UIManager.Instance.pausePanel.SetActive(true);
                //GameObject resumeBut = UIManager.Instance.resumeBut.gameObject;
                //GameObject quitBut = UIManager.Instance.goBackToMenuBut.gameObject;
                //resumeBut.transform.localScale = Vector3.one;
                //quitBut.transform.localPosition = Vector3.one;
                //LeanTween.scale(resumeBut, new Vector3(1, 1, 1), 0.3f)
                //    .setEase(LeanTweenType.easeInOutCubic);
                //LeanTween.scale(quitBut, new Vector3(1, 1, 1), 0.3f)
                //   .setEase(LeanTweenType.easeInOutCubic);
                //.setOnComplete(() => Time.timeScale = 0f);
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
                SoundManager.Instance.PlayOnShot("Button");
                //GameObject resumeBut = UIManager.Instance.resumeBut.gameObject;
                //GameObject quitBut = UIManager.Instance.goBackToMenuBut.gameObject;
                //resumeBut.transform.localScale = new Vector3(0, 0, 0);
                //quitBut.transform.localScale = new Vector3(0, 0, 0);
                //     LeanTween.scale(resumeBut, new Vector3(0, 0, 0), 0.3f)
                //.setEase(LeanTweenType.easeInOutCubic);
                //     LeanTween.scale(quitBut, new Vector3(0, 0, 0), 0.3f)
                //        .setEase(LeanTweenType.easeInOutCubic)
                //        .setOnComplete(() => );
                UIManager.Instance.pausePanel.SetActive(false);

            }
        }
    }

    public bool GetPauseState()
    {
        return isPause;
    }

}
