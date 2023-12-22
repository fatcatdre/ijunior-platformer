using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private int _currentHealth;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;

    public event UnityAction<int, int> Change;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void Damage(int damageAmount)
    {
        ChangeHealth(damageAmount,
            (damage) => _currentHealth -= damage);
    }

    public void Heal(int healAmount)
    {
        ChangeHealth(healAmount,
            (heal) => _currentHealth = Mathf.Min(_currentHealth + heal, _maxHealth));
    }

    private void ChangeHealth(int amount, Action<int> changeMethod)
    {
        amount = Mathf.Max(amount, 0);

        changeMethod(amount);

        Change?.Invoke(_currentHealth, _maxHealth);
    }
}
