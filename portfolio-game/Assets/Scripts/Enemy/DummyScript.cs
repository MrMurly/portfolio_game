using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DummyScript : MonoBehaviour, IDamage
{
    private float _currentHealth;
    
    public EnemyData dummyData;
    
    private void Start()
    {
        _currentHealth = dummyData.maxHealth;
    }

    public float GetDamage()
    {
        return Math.Max(0, _currentHealth);
    }
    public void TakeDamage(float damage, float knockback)
    {
        _currentHealth -= damage;
        Debug.Log($"{name} took {damage} damage! I now have {_currentHealth}");
    }

    public void Heal(float healAmount)
    {
        throw new System.NotImplementedException();
    }
}
