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

    public Dialog[] dialogs;
    public float textSpeed;

    [HideInInspector] public int curDialogIndex;
    DialogBox curDialogBox;

    public Transform dialogParent;

    public float dialogDist;

    public GameObject playerDialogBox;
    public GameObject velonicaDialogBox;
    public GameObject operatorDialogBox;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StrartDialog();
    }

    void StrartDialog()
    {
        curDialogIndex = 0;
        GenerateDialogBox(curDialogIndex);
    }

    public void NextDialog()
    {
        if (curDialogIndex < dialogs.Length - 1)
        {
            StopAllCoroutines();
            curDialogBox.textBox.text = dialogs[curDialogIndex].sentence;
            curDialogIndex++;
            GenerateDialogBox(curDialogIndex);
        }
        else
        {
            curDialogBox = null;
            curDialogIndex = 0;
            ClearDialogParent();
            GameManager.Instance.SwitchState(GameState.Normal);
        }
    }

    void GenerateDialogBox(int index)
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

        if (dialogParent.childCount > 0)
        {
            for (int i = 0; i < dialogParent.childCount; i++)
            {
                RectTransform rt = dialogParent.GetChild(i).GetComponent<RectTransform>();
                float yPos = (rt.rect.height * (dialogParent.childCount - i)) + (dialogDist * (dialogParent.childCount - i));
                Vector3 pos = new Vector3(0, yPos, 0);

                LeanTween.move(rt, pos, 0.5f)
                    .setEaseInOutCubic();
            }

            LeanTween.move(rectTransform, new Vector3(0, 0, 0), 0.5f)
            .setEaseInOutCubic();
        }

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
