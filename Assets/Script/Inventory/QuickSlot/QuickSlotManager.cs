using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Slot selected")]
    [SerializeField] private bool _flashliteSlotSelected;
    [SerializeField] private bool _instrumentSlotSelected;

    [Header("Games component")]
    [SerializeField] private HealthManager _healthManager;

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
        foreach(Slot slot in _quickSlot)
        {
            MedicalQuickSlot(slot);
            BaterryQuickSlot(slot);
            FlashliteQuickSlot(slot);
            InstrumentQuickSlot(slot);
        }
    }

    private void MedicalQuickSlot(Slot slot)
    {
        if (slot.SlotName == "Medical" && slot.IsEmpty != true)
        {
            if (UseMedical)
            {
                MedicamentsSO medicalParameters = slot.ItemInSlot as MedicamentsSO;

                if (medicalParameters != null && _healthManager.GetHealth < 100)
                {
                    _healthManager.AddHealthAfterTimers(medicalParameters._timeOfUse, medicalParameters._addHealth, slot);
                }
            }
        }
    }

    private void BaterryQuickSlot(Slot slot)
    {
        if (slot.SlotName == "Baterry" && slot.IsEmpty != true)
        {
            if (UseBaterry)
            {
                print("rrrrrrrrrrrrrr");
            }
        }
    }

    private void FlashliteQuickSlot(Slot slot)
    {
        if (slot.SlotName == "Flashlite" && slot.IsEmpty != true)
        {
            if (ChooseFlashlite && !_flashliteSlotSelected)
            {
                print("Flashlite selected");
                _flashliteSlotSelected = true;

            }
            else if (ChooseFlashlite && _flashliteSlotSelected)
            {
                print("Flashlite UnSelected");
                _flashliteSlotSelected = false;
            }
        }
        else if (slot.SlotName == "Flashlite" && slot.IsEmpty == true && _flashliteSlotSelected != false)
        {
            print("Flashlite UnSelected");
            _flashliteSlotSelected = false;
        }
    }

    private void InstrumentQuickSlot(Slot slot)
    {
        if (slot.SlotName == "Instrument" && slot.IsEmpty != true)
        {
            if (ChooseInstrument && !_instrumentSlotSelected)
            {
                print("Instruent selected");
                _instrumentSlotSelected = true;
            }
            else if (ChooseInstrument && _instrumentSlotSelected)
            {
                print("Instrument UnSelected");
                _instrumentSlotSelected = false;
            }
        }
        else if (slot.SlotName == "Instrument" && slot.IsEmpty == true && _instrumentSlotSelected != false)
        {
            print("Instrument UnSelected");
            _instrumentSlotSelected = false;
        }
    }
}
