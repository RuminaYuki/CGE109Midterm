using UnityEngine;

public enum ItemType
{
    Normal,
    Holdable
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public ItemType itemType;
}
