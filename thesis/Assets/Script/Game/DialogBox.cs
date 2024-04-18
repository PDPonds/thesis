using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    public TextMeshProUGUI textBox;

    public void StartDialog(string lines)
    {
        textBox.text = string.Empty;
        StartCoroutine(TypeText(lines));
    }

    IEnumerator TypeText(string lines)
    {
        foreach (char c in lines.ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(DialogueManager.Instance.textSpeed);

        }
    }

}
