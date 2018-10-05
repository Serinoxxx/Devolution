using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCore : MonoBehaviour, IDamageable {

    [SerializeField] float _maxHP = 100f;
    [SerializeField] float _currentHP;

    [SerializeField] Mesh damagedMesh;
    [SerializeField] Mesh destroyedMesh;

    [SerializeField] ParticleSystem leak;

    [SerializeField] float _damagedPercent = 0.5f;

    bool isDestroyed = false;

    public void Start()
    {
        _currentHP = _maxHP;
    }

    void IDamageable.TakeDamage(float damage, GameObject attacker, Vector3 force, bool bleed, Enums.DamageImpact impact)
    {
        //don't continue if we've been destroyed
        if (isDestroyed) return;

        _currentHP -= damage;

        if (_currentHP < _maxHP * _damagedPercent)
        {
            GetComponent<MeshFilter>().mesh = damagedMesh;
            if (!leak.isPlaying)
            {
                leak.Play();
            }
        }

        if (_currentHP <= 0 )
        {
            GetComponent<MeshFilter>().mesh = destroyedMesh;
            leak.Stop();
            GameController.instance.OnPowerCoreDestroyed(this.gameObject);
            isDestroyed = true;
        }


    }
}
