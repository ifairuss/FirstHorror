using UnityEngine;

public enum ItemType
{
    None,
    Key,
    AidKits,
    Batteries,
    Instruments
}

public abstract class ItemObject : ScriptableObject
{
    public GameObject _gamePrefab;
    public ItemType _itemType;
    [TextArea(15, 25)]
    public string _description;
}
