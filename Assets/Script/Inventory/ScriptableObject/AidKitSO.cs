using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Items/New AidKit", order = 54)]
public class AidKitSO : ItemScriptableObject
{
    public int _addHealth;
    public float _timeOfUse;

    private void Awake()
    {
        Item = ItemType.AidKit;

    }
}
