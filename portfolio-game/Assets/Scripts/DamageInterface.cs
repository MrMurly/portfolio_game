using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public void TakeDamage(float damage, float knockback);
    public void Heal(float healAmount);
}
