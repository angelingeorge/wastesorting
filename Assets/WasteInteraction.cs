using UnityEngine;
using TMPro;

public class WasteInteraction : MonoBehaviour
{
    [Header("Player Camera")]
    public Camera playerCamera;

    [Header("Interaction Settings")]
    public float interactionDistance = 10f;

    // How large the selection area is around the crosshair
    public float selectionRadius = 2.5f;

    // How far from the center of the screen an object can be
    public float screenSelectionDistance = 150f;

    [Header("Waste UI")]
    public GameObject wastePanel;
    public TMP_Text wasteNameText;
    public TMP_Text resultText;

    [Header("Game Manager")]
    public GameManager gameManager;

    private WasteItem selectedWaste;

    // Prevents interaction after the game has ended
    private bool gameEnded = false;

    void Start()
    {
        // Hide the waste panel when the game starts
        if (wastePanel != null)
        {
            wastePanel.SetActive(false);
        }

        // Keep the mouse locked during normal gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Do absolutely nothing after the game has ended
        if (gameEnded)
        {
            return;
        }

        // Left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            FindWaste();
        }
    }

    void FindWaste()
    {
        // Do nothing if the game has ended
        if (gameEnded)
        {
            return;
        }

        if (playerCamera == null)
        {
            Debug.LogError("WasteInteraction: Player Camera is not assigned.");
            return;
        }

        // Center of the screen
        Vector3 screenCenter = new Vector3(
            Screen.width / 2f,
            Screen.height / 2f,
            0f
        );

        // Get every WasteItem currently active in the scene
        WasteItem[] allWaste = FindObjectsOfType<WasteItem>();

        WasteItem closestWaste = null;

        float closestScreenDistance = screenSelectionDistance;

        foreach (WasteItem waste in allWaste)
        {
            if (waste == null)
            {
                continue;
            }

            if (!waste.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Find the position of the waste
            Vector3 wastePosition = waste.transform.position;

            // Distance from player/camera to waste
            float distanceFromCamera = Vector3.Distance(
                playerCamera.transform.position,
                wastePosition
            );

            // Ignore waste that is too far away
            if (distanceFromCamera > interactionDistance)
            {
                continue;
            }

            // Convert the waste position to screen position
            Vector3 screenPosition = playerCamera.WorldToScreenPoint(
                wastePosition
            );

            // Ignore objects behind the camera
            if (screenPosition.z <= 0)
            {
                continue;
            }

            // Calculate distance from the screen center
            float screenDistance = Vector2.Distance(
                new Vector2(screenPosition.x, screenPosition.y),
                new Vector2(screenCenter.x, screenCenter.y)
            );

            // Only consider objects close enough to the crosshair
            if (screenDistance <= closestScreenDistance)
            {
                // Check line of sight
                Ray ray = playerCamera.ViewportPointToRay(
                    new Vector3(0.5f, 0.5f, 0f)
                );

                float distanceToWaste = Vector3.Distance(
                    playerCamera.transform.position,
                    wastePosition
                );

                if (Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    distanceToWaste + selectionRadius
                ))
                {
                    // If the ray hits this waste or one of its children
                    WasteItem hitWaste =
                        hit.collider.GetComponentInParent<WasteItem>();

                    // If something else blocks the ray, don't select this one
                    if (hitWaste != null && hitWaste == waste)
                    {
                        closestWaste = waste;
                        closestScreenDistance = screenDistance;
                    }
                }
                else
                {
                    // No collider blocking the view
                    closestWaste = waste;
                    closestScreenDistance = screenDistance;
                }
            }
        }

        // Did we find a waste object?
        if (closestWaste != null)
        {
            selectedWaste = closestWaste;

            // Show the waste name
            if (wasteNameText != null)
            {
                wasteNameText.text = "Waste: " + selectedWaste.wasteName;
            }

            // Clear old result message
            if (resultText != null)
            {
                resultText.text = "";
            }

            // Show the waste panel
            if (wastePanel != null)
            {
                wastePanel.SetActive(true);
            }

            // Unlock mouse so player can click UI buttons
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("Waste selected: " + selectedWaste.wasteName);
        }
        else
        {
            Debug.Log("No waste found near the crosshair.");
        }
    }

    public void SelectContainer(WasteItem.WasteType selectedType)
    {
        // IMPORTANT:
        // Do nothing if the timer has already reached 00:00
        if (gameEnded)
        {
            return;
        }

        // Make sure a waste item has been selected
        if (selectedWaste == null)
        {
            return;
        }

        // Check whether the selected category is correct
        if (selectedWaste.correctType == selectedType)
        {
            // Show correct message
            if (resultText != null)
            {
                resultText.text = "CORRECT!";
            }

            // Make the waste disappear
            selectedWaste.gameObject.SetActive(false);

            // Hide the waste panel
            if (wastePanel != null)
            {
                wastePanel.SetActive(false);
            }

            // Lock mouse again for gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Clear selected waste
            selectedWaste = null;
        }
        else
{
    if (resultText != null)
    {
        resultText.text = "WRONG!";
        CancelInvoke(nameof(ClearResultMessage));
        Invoke(nameof(ClearResultMessage), 1f);
    }

    if (gameManager != null)
    {
        gameManager.WrongAnswer();
    }
}
    }

    // Called by GameManager when the timer reaches 00:00
    public void StopInteraction()
    {
        // Mark interaction as permanently stopped
        gameEnded = true;

        // Clear selected waste
        selectedWaste = null;

        // Hide the waste panel
        if (wastePanel != null)
        {
            wastePanel.SetActive(false);
        }

        // Lock the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Waste interaction stopped because the game has ended.");
    }

    // BIODEGRADABLE button
    public void SelectBiodegradable()
    {
        if (gameEnded)
        {
            return;
        }

        SelectContainer(WasteItem.WasteType.Biodegradable);
    }

    // NON-BIODEGRADABLE button
    public void SelectNonBiodegradable()
    {
        if (gameEnded)
        {
            return;
        }

        SelectContainer(WasteItem.WasteType.NonBiodegradable);
    }

    // E-WASTE button
    public void SelectEWaste()
    {
        if (gameEnded)
        {
            return;
        }

        SelectContainer(WasteItem.WasteType.EWaste);
    }
    void ClearResultMessage()
{
    if (resultText != null)
    {
        resultText.text = "";
    }
}
}