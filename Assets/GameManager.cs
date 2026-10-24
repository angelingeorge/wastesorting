
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WasteInteraction wasteInteraction;
    public RatEndMessage ratEndMessage;

    private WasteItem[] allWaste;
    private bool gameCompleted = false;

    void Start()
    {
        allWaste = FindObjectsByType<WasteItem>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        if (ratEndMessage != null && ratEndMessage.ratPanel != null)
            ratEndMessage.ratPanel.SetActive(false);

        if (allWaste.Length == 0)
            Debug.LogWarning("No WasteItem objects found!");
    }

    void Update()
    {
        if (gameCompleted || allWaste == null)
            return;

        foreach (WasteItem waste in allWaste)
        {
            if (waste != null && waste.gameObject.activeInHierarchy)
                return;
        }

        CompleteGame();
    }

    void CompleteGame()
    {
        gameCompleted = true;

        Debug.Log("ALL WASTE SORTED!");

        if (wasteInteraction != null)
            wasteInteraction.StopInteraction();

        if (ratEndMessage != null)
        {
            ratEndMessage.ShowMessage(
                "Amazing! You sorted all the waste! Great job!"
            );
        }
        else
        {
            Debug.LogError(
                "RatEndMessage is not assigned in GameManager!"
            );
        }
    }
}
