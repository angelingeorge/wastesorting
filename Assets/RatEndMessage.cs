
using System.Collections;
using TMPro;
using UnityEngine;

public class RatEndMessage : MonoBehaviour
{
    public GameObject ratPanel;
    public TMP_Text ratMessage;
    public float letterDelay = 0.05f;

    public void ShowMessage(string message)
    {
        StopAllCoroutines();

        ratPanel.SetActive(true);
        StartCoroutine(TypeMessage(message));
    }

    IEnumerator TypeMessage(string message)
    {
        ratMessage.text = "";

        foreach (char letter in message)
        {
            ratMessage.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }
}

