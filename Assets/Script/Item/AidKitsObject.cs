using UnityEngine;

[CreateAssetMenu(fileName = "New AidKitsObject", menuName = "Inventory System/Item/AidKitsObject")]
public class AidKitsObject : ItemObject
{
    public int RestoreHealthValue;

    private void Awake()
    {
        _itemType = ItemType.AidKits;
    }
}
