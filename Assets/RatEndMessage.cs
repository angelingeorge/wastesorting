
using System.Collections;
using TMPro;
using UnityEngine;

public class RatEndMessage : MonoBehaviour
{
    public GameObject ratPanel;
    public TMP_Text ratMessage;
    public float letterDelay = 0.04f;
    public float messageEndDelay = 2f;

    private Coroutine typingCoroutine;

    void Start()
    {
        ShowMessage(
            "Welcome! Collect all the waste and put each item in the correct bin."
        );
    }

    public void ShowMessage(string message)
    {
        if (ratPanel == null || ratMessage == null)
        {
            Debug.LogError("RatPanel or RatMessage is not assigned!");
            return;
        }

        ratPanel.SetActive(true);
        ratMessage.gameObject.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeMessage(message));
    }

    IEnumerator TypeMessage(string message)
    {
        ratMessage.text = "";

        foreach (char letter in message)
        {
            ratMessage.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        // Wait until the player can read the full message
        yield return new WaitForSeconds(messageEndDelay);

        // Hide the panel after the message finishes
        ratPanel.SetActive(false);

        typingCoroutine = null;
    }
}
