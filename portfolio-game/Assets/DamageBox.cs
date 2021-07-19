using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        print($"{other.name} took damage");
        var component = other.GetComponent<IDamage>();
        component?.TakeDamage(1, 4);
    }
}
