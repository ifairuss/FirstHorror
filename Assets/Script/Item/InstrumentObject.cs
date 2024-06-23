using UnityEngine;

[CreateAssetMenu(fileName = "New Instrument", menuName = "Inventory System/Item/Instrument")]
public class InstrumentObject : ItemObject
{
    private void Awake()
    {
        _itemType = ItemType.Instruments;
    }
}
