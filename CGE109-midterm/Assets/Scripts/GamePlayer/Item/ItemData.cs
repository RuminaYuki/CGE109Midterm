using UnityEngine;

public enum ItemType
{
    Normal,
    Holdable,
    GameObj
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public ItemType itemType;
    public GameObject gameObj;
}
