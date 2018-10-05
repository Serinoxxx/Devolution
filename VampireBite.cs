using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBite : MonoBehaviour {

    [SerializeField] float damage = 100f;
    [SerializeField] float lifeStealPercentage = .5f;
    [SerializeField] float radius = 1f;
    [SerializeField] AudioSource source;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Enums.DamageImpact impact = Enums.DamageImpact.none;
    [SerializeField] Enums.DamageModifierType damageModType;

    Health ownerHealth;

    private void Start()
    {
        ownerHealth = GetComponentInParent<Health>();
    }

    public void Bite()
    {
            foreach (Collider other in Physics.OverlapSphere(transform.position, radius))
            {
                //If the object has a health component and prevent zombies from damaging each other
                if (other.gameObject.GetComponent<Health>())
                {
                    //Make sure we're not damaging ourselves or our allies
                    if (other.gameObject.GetComponent<Health>().tag != ownerHealth.tag)
                    {
                        other.gameObject.GetComponent<Health>().TakeDamage(damage, gameObject, bleed: true, impact: impact);

                        DamageModHandler.instance.AddTarget(other.gameObject.GetComponent<Health>(), damageModType);

                        ownerHealth.Heal(damage * lifeStealPercentage);

                        if (!source.isPlaying) source.Play();
                        if (particles != null) particles.Play(false);
                    }
                }
        }
    }
}
