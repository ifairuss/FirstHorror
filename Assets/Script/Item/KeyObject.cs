using UnityEngine;

[CreateAssetMenu(fileName = "New Key", menuName = "Inventory System/Item/Key")]
public class KeyObject : ItemObject
{
    public GameObject Door;

    private void Awake()
    {
        _itemType = ItemType.Key;
    }
}
