using UnityEngine;

public class WasteItem : MonoBehaviour
{
    public enum WasteType
    {
        Biodegradable,
        NonBiodegradable,
        EWaste
    }

    [Header("Waste Information")]
    public string wasteName;

    // Correct answer stored internally.
    // The player will NOT see this directly.
    public WasteType correctType;
}