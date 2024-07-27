using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    private bool UseMedical => Input.GetKeyDown(_useMedicalItem);
    private bool UseBaterry => Input.GetKeyDown(_useBaterryItem);
    private bool ChooseFlashlite => Input.GetKeyDown(_flashliteSlot);
    private bool ChooseInstrument => Input.GetKeyDown(_instrumentSlot);

    [Header("Control")]
    [SerializeField] private KeyCode _useMedicalItem;
    [SerializeField] private KeyCode _useBaterryItem;
    [SerializeField] private KeyCode _flashliteSlot;
    [SerializeField] private KeyCode _instrumentSlot;

    [Header("Quick Slots")]
    [SerializeField] private Transform _quickSlotBox;
    [SerializeField] private List<Slot> _quickSlot = new List<Slot>();


    private void Awake()
    {
        for (int i = 0; i < _quickSlotBox.childCount; i++)
        {
            if (_quickSlotBox.transform.GetChild(i).GetComponent<Slot>() != null)
            {
                _quickSlot.Add(_quickSlotBox.transform.GetChild(i).GetComponent<Slot>());
            }
        }
    }

    private void Update()
    {
        ButtonQuickSlotManager();
    }

    private void ButtonQuickSlotManager()
    {
        if(UseMedical)
        {
            print("hhhhhhhhhhhh");

        }
        if (UseBaterry)
        {
            print("rrrrrrrrrrrrrr");

        }
        if (ChooseFlashlite)
        {
            print("111111111111111111");
        }
        if(ChooseInstrument)
        {
            print("2222222222222222222");

        }
    }
}
