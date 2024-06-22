using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static Action<int> OnTakeDamage;

    [Header("Health Parameters")]
    [SerializeField] private int _maxHelth;

    private int _currentHealth;

    [Header("Health item")]
    [SerializeField] private int _bandages;
    [SerializeField] private int _bigBandages;

    [SerializeField] private Slider _helthSlider;

    private void Awake()
    {
        _currentHealth = _maxHelth;

        _helthSlider.maxValue = _maxHelth;
        _helthSlider.value = _currentHealth;
    }

    private void Update()
    {
        UseBandages();
    }

    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
    }

    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }

    private void ApplyDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void ApplyHealth(int health)
    {
        _currentHealth += health;

        if (_currentHealth > _maxHelth)
        {
            _currentHealth = _maxHelth;
        }
    }

    private void KillPlayer()
    {
        _currentHealth = 0;

        print("DEAD");
    }

    private void UseBandages()
    {
        if (_currentHealth < _maxHelth && _currentHealth > 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_bandages > 0)
                {
                    ApplyHealth(15);
                    _bandages -= 1;
                }
                else if (_bigBandages > 0)
                {
                    ApplyHealth(35);
                    _bigBandages -= 1;
                }
            }
        }

        _helthSlider.value = _currentHealth;
    }
}
