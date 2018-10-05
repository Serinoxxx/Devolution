using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
    void TakeDamage(float damage, GameObject attacker = null, Vector3 force = new Vector3(), bool bleed = false, Enums.DamageImpact impact = Enums.DamageImpact.none);
}
