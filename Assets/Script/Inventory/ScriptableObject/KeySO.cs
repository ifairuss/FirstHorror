using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Items/New Key", order = 55)]
public class KeySO : ItemScriptableObject
{
    private void Awake()
    {
        Item = ItemType.Key;

    }
}
