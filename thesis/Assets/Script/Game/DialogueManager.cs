using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Dialog[] dialogs;
    public float textSpeed;

    int curDialogIndex;
    DialogBox curDialogBox;

    public Transform dialogParent;

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
        GameObject dialogObj = Instantiate(dialog.dialogPrefab, dialogParent);
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
    public GameObject dialogPrefab;
}
