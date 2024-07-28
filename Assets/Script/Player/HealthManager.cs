using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Health Parameters")]
    [SerializeField] private int _maxHelth;

    [Header("Sliders component")]
    [SerializeField] private Slider _helthSlider;
    [SerializeField] private Slider _timeToAddHealthSlider;

    [Header("Decrement timer properties")]
    [SerializeField] private float _timerValueDecrement;

    [Header("Game Component")]
    [SerializeField] private FirstPersonController _playerMove;

    private int _currentHealth;
    public int GetHealth { get { return _currentHealth; } private set { } }

    private void Awake()
    {
        _currentHealth = _maxHelth;

        _helthSlider.maxValue = _maxHelth;
        _helthSlider.value = _currentHealth;

        _timeToAddHealthSlider.gameObject.SetActive(false);

    }

    private void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        _helthSlider.value = _currentHealth;

        if (_currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        _currentHealth = 0;

        print("DEAD");
    }

    private void ApplyHealth(int health)
    {
        _currentHealth += health;
        _helthSlider.value = _currentHealth;

        if (_currentHealth > _maxHelth)
        {
            _currentHealth = _maxHelth;
        }
    }

    public void OnTakeDamage(int damage)
    {
        ApplyDamage(damage);
    }

    public void AddHealthAfterTimers(float timer, int health, Slot slot)
    {
        _timeToAddHealthSlider.maxValue = timer;
        _timeToAddHealthSlider.gameObject.SetActive(true);

        StartCoroutine(DecrementTimer(timer, health, slot));
    }

    private IEnumerator DecrementTimer(float timer, int health, Slot slot)
    {
        _playerMove.CanMovUseItem = false;

        while (timer > 0)
        {
            timer -= _timerValueDecrement;
            _timeToAddHealthSlider.value = timer;

            if (timer < 0)
            {
                ApplyHealth(health);
                CleaningSlots(slot);
                _playerMove.CanMovUseItem = true;
                _timeToAddHealthSlider.gameObject.SetActive(false);
                timer = 0;
            }

            yield return timer;
        }
    }

    private void CleaningSlots(Slot clearSlot)
    {
        clearSlot.ItemInSlot = null;
        clearSlot.SlotItemId = 0;
        clearSlot.IsEmpty = true;
        clearSlot._ItemInSlotImage.sprite = null;
        clearSlot._ItemInSlotImage.color = new Color(0, 0, 0, 0);
    }
}
