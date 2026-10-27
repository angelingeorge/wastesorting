
using UnityEngine;
using TMPro;

public class WasteInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 10f;

    public GameObject wastePanel;
    public TMP_Text wasteNameText;
    public TMP_Text resultText;

    private WasteItem selectedWaste;

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
        if (wastePanel != null && wastePanel.activeSelf)
            return;

        if (Input.GetMouseButtonDown(0))
            FindWaste();
    }

    void FindWaste()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Assign Player Camera in Inspector!");
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            Debug.Log("Nothing hit. Aim at the waste.");
            return;
        }

        WasteItem waste = hit.collider.GetComponentInParent<WasteItem>();

        if (waste == null)
        {
            Debug.Log("Add WasteItem to this waste object's root.");
            return;
        }

        selectedWaste = waste;

        Debug.Log("WASTE SELECTED: " + selectedWaste.wasteName);

        if (wasteNameText != null)
            wasteNameText.text = "Waste: " + selectedWaste.wasteName;

        if (resultText != null)
            resultText.text = "";

        if (wastePanel != null)
            wastePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SelectContainer(WasteType selectedType)
    {
        if (selectedWaste == null)
        {
            Debug.LogError("No waste selected. Check Button OnClick connection.");
            return;
        }

        if (selectedWaste.correctType == selectedType)
        {
            Debug.Log("CORRECT! Hiding " + selectedWaste.wasteName);

            // Hide the complete waste object and its child model
            GameObject wasteObject = selectedWaste.gameObject;
            wasteObject.SetActive(false);

            selectedWaste = null;

            if (wastePanel != null)
                wastePanel.SetActive(false);

            if (resultText != null)
                resultText.text = "";

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Debug.Log("WRONG CATEGORY!");

            if (resultText != null)
            {
                resultText.text = "WRONG!";
                resultText.color = Color.red;
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
}
