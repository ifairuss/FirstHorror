using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private InventoryObject _inventory;

    [Header("Slot Properties")]
    [SerializeField] private int X_START;
    [SerializeField] private int Y_START;
    [SerializeField] private int X_SPACE_BETWEN_ITEM;
    [SerializeField] private int NUMBER_OF_COLUMN;
    [SerializeField] private int Y_SPACE_BETWEN_ITEM;

    private Dictionary<InventorySlot, GameObject> _itemDisplay = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        CreateDisplay();
    }

    private void Update()
    {
        UdateDisplay();
    }

    private void OnApplicationQuit()
    {
        _inventory.Container.Clear();
    }

    private void CreateDisplay()
    {
        for (int i = 0; i < _inventory.Container.Count; i++)
        {
            var itemObject = Instantiate(_inventory.Container[i].Item._gamePrefab, Vector3.zero, Quaternion.identity, transform);
            itemObject.GetComponent<RectTransform>().localPosition = GetPosition(i);
            itemObject.GetComponentInChildren<TextMeshProUGUI>().text = _inventory.Container[i].Amount.ToString("x");
            _itemDisplay.Add(_inventory.Container[i], itemObject);
        }
    }

    private void UdateDisplay()
    {
        for (int i = 0; i < _inventory.Container.Count; i++)
        {
            if (_itemDisplay.ContainsKey(_inventory.Container[i]))
            {
                _itemDisplay[_inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = _inventory.Container[i].Amount.ToString("x0");
            }
            else
            {
                var itemObject = Instantiate(_inventory.Container[i].Item._gamePrefab, Vector3.zero, Quaternion.identity, transform);
                itemObject.GetComponent<RectTransform>().localPosition = GetPosition(i);
                itemObject.GetComponentInChildren<TextMeshProUGUI>().text = _inventory.Container[i].Amount.ToString("x");
                _itemDisplay.Add(_inventory.Container[i], itemObject);
            }
        }
    }

    private Vector3 GetPosition(int posotion)
    {
        return new Vector3(X_START + (X_SPACE_BETWEN_ITEM * (posotion %  NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEN_ITEM * (posotion/NUMBER_OF_COLUMN)), 0);
    }
}
