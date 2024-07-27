using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Items/New Instrument", order = 51)]
public class InstrumentsSO : ItemScriptableObject
{
    [SerializeField] private int _damage;

    private void Awake()
    {
        Item = ItemType.Instrument;
    }
}
