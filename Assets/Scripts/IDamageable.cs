using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{

    void TakeDamage(float _damage) { }

    void Heal(float _health) { }

    void AddAmmo(int _ammo) { }
}
