
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject successText;

    private Transform wasteParent;
    private bool gameCompleted = false;

    void Start()
    {
        GameObject wasteObject = GameObject.Find("waste");

        if (wasteObject != null)
        {
            wasteParent = wasteObject.transform;
        }
        else
        {
            Debug.LogError("waste parent object not found!");
        }

        if (successText != null)
            successText.SetActive(false);
        else
            Debug.LogError("SuccessText is not assigned!");

        Debug.Log("GameManager started");
    }

    void Update()
    {
        if (gameCompleted || wasteParent == null)
            return;

        // Check all child waste objects
        foreach (Transform child in wasteParent)
        {
            if (child.gameObject.activeInHierarchy)
                return;
        }

        CompleteGame();
    }

    void CompleteGame()
    {
        gameCompleted = true;

        Debug.Log("SUCCESS! ALL WASTE COLLECTED!");

        if (successText != null)
            successText.SetActive(true);
    }
}
