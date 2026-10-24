
using UnityEngine;

public class WasteItem : MonoBehaviour
{
    public string wasteName;

    public WasteType correctType;
}

public enum WasteType
{
    Biodegradable,
    NonBiodegradable,
    EWaste
}
