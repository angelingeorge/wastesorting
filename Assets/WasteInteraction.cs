
using UnityEngine;
using TMPro;

public class WasteInteraction : MonoBehaviour
{
    [Header("Player")]
    public Camera playerCamera;
    public float interactionDistance = 10f;

    [Header("Waste UI")]
    public GameObject wastePanel;
    public TMP_Text wasteNameText;
    public TMP_Text resultText;

    private WasteItem selectedWaste;
    private bool gameEnded = false;

    void Start()
    {
        if (wastePanel != null)
            wastePanel.SetActive(false);

        if (resultText != null)
            resultText.text = "";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (gameEnded)
            return;

        if (wastePanel != null && wastePanel.activeSelf)
            return;

        if (Input.GetMouseButtonDown(0))
            FindWaste();
    }

    void FindWaste()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned!");
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            Debug.Log("Waste-il aim cheyyuka.");
            return;
        }

        WasteItem waste =
            hit.collider.GetComponentInParent<WasteItem>();

        if (waste == null)
        {
            Debug.LogWarning(
                "Hit object-il WasteItem component kandilla!"
            );
            return;
        }

        selectedWaste = waste;

        if (wasteNameText != null)
            wasteNameText.text = "Recycle " + waste.wasteName;

        if (resultText != null)
        {
            resultText.text = "";
            resultText.color = Color.white;
        }

        if (wastePanel != null)
            wastePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Waste selected: " + waste.wasteName);
    }

    public void SelectContainer(WasteType selectedType)
    {
        if (gameEnded)
            return;

        if (selectedWaste == null)
        {
            Debug.LogWarning("No waste selected!");
            return;
        }

        if (selectedWaste.correctType == selectedType)
        {
            GameObject sortedWaste = selectedWaste.gameObject;

            Debug.Log("Correct! Sorted: " + selectedWaste.wasteName);

            // Clear selection before hiding the waste
            selectedWaste = null;

            // Correct bin: waste disappear aakum
            sortedWaste.SetActive(false);

            if (wastePanel != null)
                wastePanel.SetActive(false);

            if (resultText != null)
                resultText.text = "";

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Debug.Log("Wrong waste category!");

            if (resultText != null)
            {
                resultText.gameObject.SetActive(true);
                resultText.color = Color.red;
                resultText.text = "WRONG! Try again.";
            }
        }
    }

    public void SelectBiodegradable()
    {
        SelectContainer(WasteType.Biodegradable);
    }

    public void SelectNonBiodegradable()
    {
        SelectContainer(WasteType.NonBiodegradable);
    }

    public void SelectEWaste()
    {
        SelectContainer(WasteType.EWaste);
    }

    public void StopInteraction()
    {
        gameEnded = true;
        selectedWaste = null;

        if (wastePanel != null)
            wastePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
