using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Properties")]
    [SerializeField] private Transform _inventory;

    [Header("Inventory Slot")]
    [SerializeField] private List<Slot> _inventorySlot = new List<Slot>();

    private void Awake()
    {
        for (int i = 0; i < _inventory.childCount; i++)
        {
            if(_inventory.transform.GetChild(i).GetComponent<Slot>() != null)
            {
                _inventorySlot.Add(_inventory.transform.GetChild(i).GetComponent<Slot>());
            }
        }
    }

    public void AddItemInInventory(ItemScriptableObject ItemInGame)
    {
        foreach (Slot slot in _inventorySlot)
        {
            if (slot.IsEmpty == true)
            {
                slot.ItemInSlot = ItemInGame;
                slot.SwitchImage(ItemInGame.ItemImageInventory);
                slot.IsEmpty = false;
                break;
            }
        }
    }
}
