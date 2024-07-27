using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Health Parameters")]
    [SerializeField] private int _maxHelth;

    private int _currentHealth;

    [SerializeField] private Slider _helthSlider;

    private void Awake()
    {
        _currentHealth = _maxHelth;

        _helthSlider.maxValue = _maxHelth;
        _helthSlider.value = _currentHealth;
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
}
