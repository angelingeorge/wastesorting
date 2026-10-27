
using System.Collections;
using TMPro;
using UnityEngine;

public class Rules : MonoBehaviour
{
    public GameObject rulesPanel;
    public TMP_Text rulesText;
    public AudioSource backgroundMusic;

    public float letterDelay = 0.05f;
    public float waitAfterText = 2f;

    void Start()
    {
        rulesPanel.SetActive(true);
        StartCoroutine(ShowRules());
    }

    IEnumerator ShowRules()
    {
        string fullText = rulesText.text;
        rulesText.text = "";

        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }

        foreach (char letter in fullText)
        {
            rulesText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        yield return new WaitForSeconds(waitAfterText);

        rulesPanel.SetActive(false);
    }
}