using UnityEngine;


 public enum ItemType
 {
    None,
    Instrument,
    Baterry,
    AidKit,
    Flashlite,
    Key
 }
public class ItemScriptableObject : ScriptableObject
{
    [Header("Item Properties")]
    public int ItemID;
    [TextArea (15,20)] public string Description;
    [Header("Item GameObject")]
    public Transform DropItemPrefab;
    public Sprite ItemImageInventory;
    public ItemType Item;
}
