using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Slot _oldSlot;
    private Transform _playerPosition;
    private Transform _dragLayer;

    private void Start()
    {
        _playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _dragLayer = GameObject.FindGameObjectWithTag("DragLayer").GetComponent<Transform>();
        _oldSlot = transform.GetComponentInParent<Slot>();
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        if (_oldSlot.IsEmpty == true)
        {
            return;
        }
        else
        {
            var image = GetComponent<Image>();

            image.color = new Color(1, 1, 1, 0.70f);
            image.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            image.raycastTarget = false;
            
            transform.SetParent(_dragLayer);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_oldSlot.IsEmpty == true)
        {
            return;
        }
        else
        {
            transform.position = eventData.position;
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            if(_oldSlot.IsEmpty == false)
            {
                FixBugInInventory();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_oldSlot.IsEmpty == true)
        {
            return;
        }
        else
        {
            FixBugInInventory();

            if (eventData.pointerCurrentRaycast.gameObject.name == "InventoryBackground")
            {
                Instantiate(_oldSlot.ItemInSlot.DropItemPrefab, _playerPosition.position + Vector3.up + _playerPosition.forward, Quaternion.identity);
                CleaningSlots(_oldSlot);
            }

            if(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Slot>() != null)
            {
                var newSlot = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Slot>();

                if (newSlot.IsFastSlot == true)
                {
                    switch (newSlot.SlotName)
                    {
                        case "Baterry":
                            {
                                if(_oldSlot.SlotItemId > 20 && _oldSlot.SlotItemId < 30)
                                {
                                    ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Slot>());
                                }
                            }
                            break;
                        case "Medical":
                            {
                                if (_oldSlot.SlotItemId > 30 && _oldSlot.SlotItemId < 40)
                                {
                                    ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Slot>());
                                }
                            }
                            break;
                        case "Flashlite":
                            {
                                if (_oldSlot.SlotItemId > 10 && _oldSlot.SlotItemId < 20)
                                {
                                    ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Slot>());
                                }
                            }
                            break;
                        case "Instrument":
                            {
                                if (_oldSlot.SlotItemId > 0 && _oldSlot.SlotItemId < 10)
                                {
                                    ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Slot>());
                                }
                            }
                            break;
                        default:
                            print("This item cannot be placed in this slot");
                            break;
                    }
                }
                else
                {
                    if(newSlot.IsEmpty == false)
                    {
                        if (_oldSlot.IsFastSlot == true && newSlot.ItemInSlot.Item != _oldSlot.ItemInSlot.Item)
                        {
                            print("This item cannot be placed in this slot");
                        }
                        else
                        {
                            ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Slot>());

                        }
                    }
                    else
                    {
                        ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Slot>());
                    }
                }
            }
        }
    }

    private void FixBugInInventory()
    {
        var image = GetComponent<Image>();

        image.color = new Color(1, 1, 1, 1f);
        image.transform.localScale = new Vector3(1, 1, 1);
        image.raycastTarget = true;
        transform.SetParent(_oldSlot.transform);
    }

    private void CleaningSlots(Slot clearSlot)
    {
        clearSlot.ItemInSlot = null;
        clearSlot.SlotItemId = 0;
        clearSlot.IsEmpty = true;
        clearSlot._ItemInSlotImage.sprite = null;
        clearSlot._ItemInSlotImage.color = new Color(0, 0, 0, 0);
    }

    private void ExchangeSlotData(Slot newSlot)
    {
        ItemScriptableObject IntemDataNewSlot = newSlot.ItemInSlot;
        int ID_DataNewSlot = newSlot.SlotItemId;
        Sprite ImageDataNewSlot = newSlot._ItemInSlotImage.sprite;
        bool IsEmptyDataNewSlot = newSlot.IsEmpty;


        newSlot.ItemInSlot = _oldSlot.ItemInSlot;
        if(_oldSlot.IsEmpty == false)
        {
            newSlot.SlotItemId = _oldSlot.SlotItemId;
            newSlot._ItemInSlotImage.sprite = _oldSlot._ItemInSlotImage.sprite;
            newSlot._ItemInSlotImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            newSlot.SlotItemId = 0;
            newSlot._ItemInSlotImage.sprite = null;
            newSlot._ItemInSlotImage.color = new Color(0, 0, 0, 0);
        }    
        newSlot.IsEmpty = _oldSlot.IsEmpty;

        _oldSlot.ItemInSlot = IntemDataNewSlot;
        if(IsEmptyDataNewSlot == false)
        {
            _oldSlot.SlotItemId = ID_DataNewSlot;
            _oldSlot._ItemInSlotImage.sprite = ImageDataNewSlot;
            _oldSlot._ItemInSlotImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            _oldSlot.SlotItemId = 0;
            _oldSlot._ItemInSlotImage.sprite = null;
            _oldSlot._ItemInSlotImage.color = new Color(0, 0, 0, 0);
        }    
        _oldSlot.IsEmpty = IsEmptyDataNewSlot;
    }
}
