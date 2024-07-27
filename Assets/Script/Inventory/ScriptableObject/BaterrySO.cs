using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Items/New Baterry", order = 53)]
public class BaterrySO : ItemScriptableObject
{
    [Header("Baterry Parameters")]
    [SerializeField] private int _addEnergy;
    [SerializeField] private float _timeOfUse;

    private void Awake()
    {
        Item = ItemType.Baterry;

    }
}
