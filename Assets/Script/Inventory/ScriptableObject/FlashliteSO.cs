using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Items/New Flashlite", order = 52)]
public class FlashliteSO : ItemScriptableObject
{
    [Header("Flashlite Parameters")]
    [SerializeField] private int _maxEnergy;

    private void Awake()
    {
        Item = ItemType.Flashlite;

    }
}
