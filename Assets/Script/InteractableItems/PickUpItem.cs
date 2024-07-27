using UnityEngine;

public class PickUpItem : Interactable
{
    [Header("Interactable Parameters")]
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private ItemScriptableObject _itemInGame;

    private void Start()
    {
        _inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    public override void OnFocus()
    {
    }

    public override void OnInteract()
    {
        if(_itemInGame != null)
        {
            _inventoryManager.AddItemInInventory(_itemInGame);

            Destroy(gameObject);
        }
    }

    public override void OnLoseFocus()
    {
    }
}
