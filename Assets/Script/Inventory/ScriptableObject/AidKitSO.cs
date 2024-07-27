using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Items/New AidKit", order = 54)]
public class AidKitSO : ItemScriptableObject
{
    [Header("Medical Parameters")]
    [SerializeField] private int _addHealth;
    [SerializeField] private float _timeOfUse;

    private void Awake()
    {
        Item = ItemType.AidKit;

    }
}
