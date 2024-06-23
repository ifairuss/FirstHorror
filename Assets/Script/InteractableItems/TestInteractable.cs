using UnityEngine;

public class TestInteractable : Interactable
{
    [SerializeField] private GameObject flash;
    [SerializeField] private GameObject _crossHair;
    [SerializeField] private ItemObject item;
    [SerializeField] private InventoryObject _inventoryObject;

    private void Start()
    {
        flash.SetActive(false);
        _crossHair.SetActive(false);

    }
    public override void OnFocus()
    {
        print("LOOK AT ME" + gameObject.name);
        _crossHair.SetActive(true);
    }

    public override void OnInteract()
    {
        print("CHECK INVENTORY" + gameObject.name);

        _crossHair.SetActive(false);

        _inventoryObject.AddItem(item, 1);

        Destroy(gameObject);
    }

    public override void OnLoseFocus()
    {
        _crossHair.SetActive(false);
        print("DONT LOOK AT ME" + gameObject.name);
    }

    public override void OnLoseUnFocus()
    {
        _crossHair.SetActive(false);
    }
}
