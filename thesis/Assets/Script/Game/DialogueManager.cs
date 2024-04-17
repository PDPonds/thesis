using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogOwner
{
    Player, Velonica, Operator
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [Header("===== Tutorial =====")]
    public Dialog[] beforeBossTutorialDialogs;
    public Dialog[] afterBossTutorialDialogs;
    [Header("===== Before Game Start =====")]
    public Dialog[] dialogs;
    [Header("===== First State Boss =====")]
    public Dialog[] beforeFirstBossDialogs;
    public Dialog[] afterFirstBossDialogs;
    [Header("===== Second State Boss =====")]
    public Dialog[] beforeSecondBossDialogs;
    public Dialog[] afterSecondBossDialogs;

    [Header("===== Setting =====")]
    public float textSpeed;

    [HideInInspector] public int curDialogIndex;
    DialogBox curDialogBox;

    public Transform dialogParent;

    public float dialogDist;
    [Header("===== Prefabs =====")]
    public GameObject playerDialogBox;
    public GameObject velonicaDialogBox;
    public GameObject operatorDialogBox;

    [HideInInspector] public bool bossIsSpawn;

    private void Awake()
    {
        Instance = this;
    }


    public void StrartDialog(Dialog[] dialogs)
    {
        curDialogIndex = 0;
        GenerateDialogBox(curDialogIndex, dialogs);
    }

    public void NextDialog(Dialog[] dialogs)
    {
        if (curDialogIndex < dialogs.Length - 1)
        {
            if (curDialogBox == null) return;

            StopAllCoroutines();
            curDialogBox.textBox.text = dialogs[curDialogIndex].sentence;
            curDialogIndex++;
            GenerateDialogBox(curDialogIndex, dialogs);
        }
        else
        {
            curDialogBox = null;
            curDialogIndex = 0;
            ClearDialogParent();
            switch (GameManager.Instance.state)
            {
                case GameState.BeforeGameStart:
                    if (PlayerManager.passTutorial)
                    {
                        GameManager.Instance.SwitchState(GameState.Normal);
                    }
                    else
                    {
                        if (!bossIsSpawn)
                        {
                            GameManager.Instance.SpawnBoss();
                            bossIsSpawn = true;
                            StrartDialog(afterBossTutorialDialogs);
                        }
                        else
                        {
                            BossController bossCon = GameManager.Instance.curBoss.GetComponent<BossController>();
                            bossCon.SwitchBehavior(BossBehavior.TutorialEscape);
                        }
                    }
                    break;
                case GameState.AfterSecondBoss:
                    GameManager.Instance.EnterGoodCutScene();
                    break;
                case GameState.AfterFirstBoss:
                    GameManager.Instance.SwitchState(GameState.Normal);
                    break;
                case GameState.BeforeSecondBoss:
                case GameState.BeforeFirstBoss:
                    GameManager.Instance.SwitchState(GameState.BossFight);
                    break;
            }

        }
    }

    void GenerateDialogBox(int index, Dialog[] dialogs)
    {
        Dialog dialog = dialogs[index];
        GameObject dialogPrefab = null;
        switch (dialog.owner)
        {
            case DialogOwner.Player:
                dialogPrefab = playerDialogBox;
                break;
            case DialogOwner.Velonica:
                dialogPrefab = velonicaDialogBox;
                break;
            case DialogOwner.Operator:
                dialogPrefab = operatorDialogBox;
                break;
            default:
                dialogPrefab = playerDialogBox;
                break;
        }

        GameObject dialogObj = Instantiate(dialogPrefab, dialogParent);

        //Setup Position
        RectTransform rectTransform = dialogObj.GetComponent<RectTransform>();
        float dialogHeight = rectTransform.rect.height;
        rectTransform.anchoredPosition = new Vector3(0, -dialogHeight, 0);

        if (curDialogBox != null)
        {
            RectTransform rt = curDialogBox.GetComponent<RectTransform>();
            Vector3 pos = new Vector3(0, -rt.rect.height, 0);

            LeanTween.move(rt, pos, 0.5f)
                .setEaseInOutCubic();

        }

        LeanTween.move(rectTransform, new Vector3(0, dialogDist, 0), 0.5f)
            .setEaseInOutCubic();

        DialogBox dialogBox = dialogObj.GetComponent<DialogBox>();
        curDialogBox = dialogBox;
        dialogBox.StartDialog(dialog.sentence);

    }

    void ClearDialogParent()
    {
        if (dialogParent.childCount > 0)
        {
            for (int i = 0; i < dialogParent.childCount; i++)
            {
                Destroy(dialogParent.GetChild(i).gameObject);
            }
        }
    }

}

[System.Serializable]
public class Dialog
{
    public string sentence;
    public DialogOwner owner;
}
