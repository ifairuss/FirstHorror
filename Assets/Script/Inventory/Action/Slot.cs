using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("Functional Slot")]
    public bool IsEmpty = true;
    public bool IsFastSlot = false;

    [Header("Setting Slot")]
    public string SlotName = "DefaulthSlot";

    [Header("Slot Properties")]
    public ItemScriptableObject ItemInSlot;
    public int SlotItemId;
    public Image _ItemInSlotImage;

    private void Awake()
    {
        _ItemInSlotImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void SwitchImage(Sprite image)
    {
        _ItemInSlotImage.sprite = image;
        _ItemInSlotImage.color = new Color(1, 1, 1, 1);

        SlotItemId = ItemInSlot.ItemID;
    }
}
