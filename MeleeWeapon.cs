using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeWeapon : MonoBehaviour {

    public float MinDamageVelocity = 1f;
    public float DamageMultiplier = 1.5f;
    public float MaxDamage = 10f;

    public float force = 100f;

    public Item itm;
    public float durabilityLoss = 20f; //How how durability does this item lose on hit?

    public Health ownerHealth; //used to prevent us from hitting ourselves

    public AudioSource source;

    public ParticleSystem impactPS;

    public Enums.DamageImpact impact = Enums.DamageImpact.none;

    public Enums.DamageModifierType damageModType;

    public string primaryAttack = "Undefined";
    public string secondaryAttack = "Undefined";

    Vector3 velocity;
    Vector3 lastVelocity;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        velocity = transform.position - lastVelocity;
        lastVelocity = transform.position;
    }

    public void DoDamage(float animDmgMultiplier = 1f, float animRadiusMultiplier = 1f)
    {
        foreach (Collider other in Physics.OverlapSphere(transform.position, itm.damageCollider.radius * animRadiusMultiplier))
        {
            //If the object has a health component and prevent zombies from damaging each other
            IDamageable damageable = GetDamageable(other.gameObject);
            if (damageable != null)// && !(gameObject.tag == "ZombieClaw" && other.gameObject.tag == "Zombie"))
            {
                //Make sure we're not damaging ourselves or our allies
                //if (other.gameObject.GetComponent<Health>().tag != ownerHealth.tag)
                if (other.gameObject.tag != ownerHealth.tag)
                {
                    float damage = Mathf.Clamp(velocity.magnitude * DamageMultiplier, 0f, MaxDamage);
                    damage *= animDmgMultiplier;

                    GameController.instance.skillTree.CheckModifiers(SkillTree.ModifierType.damage, ref damage);

                    Debug.Log(gameObject.name + " hit " + other.name + " for " + damage);

                    Vector3 otherPosHigh;
                    otherPosHigh = other.transform.position;
                    otherPosHigh.y += 1;
                    Vector3 direction = otherPosHigh - transform.position;
                    direction.y += 0.2f;
                    direction = direction.normalized;

                    float modifiedForce = force;
                    GameController.instance.skillTree.CheckModifiers(SkillTree.ModifierType.knockback, ref modifiedForce);

                    //Knock the enemy up in the air for a bit of FUN
                    Vector3 highDirection = direction;
                    highDirection.y += 1f;

                    damageable.TakeDamage(damage, gameObject, force: highDirection * modifiedForce, bleed: true, impact: impact); //direction * force);
                    
                    if (other.gameObject.GetComponent<Health>())
                    {
                        DamageModHandler.instance.AddTarget(other.gameObject.GetComponent<Health>(), damageModType);
                    }
                    if (!source.isPlaying) source.Play();
                }
            }
        }
    }

    private IDamageable GetDamageable(GameObject go)
    {
        MonoBehaviour[] list = go.GetComponents<MonoBehaviour>();
        IDamageable damageable = null;
        foreach (MonoBehaviour mb in list)
        {
            if (mb is IDamageable)
            {
                damageable = (IDamageable)mb;
            }
        }

        return damageable;
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (itm.isEquipped && gameObject.GetComponentInParent<Animator>().GetBool("isMeleeDamageActive"))
    //    {
    //        //If the object has a health component and prevent zombies from damaging each other
    //        if (other.gameObject.GetComponent<Health>() && !(gameObject.tag == "ZombieClaw" && other.gameObject.tag == "Zombie"))
    //        {
    //            //Make sure we're not damaging ourselves or our allies
    //            if (other.gameObject.GetComponent<Health>().tag != ownerHealth.tag)
    //            {
    //                Debug.Log("Axe collider entered trigger");
    //                if (velocity.magnitude > MinDamageVelocity)
    //                {

    //                    float damage = Mathf.Clamp(velocity.magnitude * DamageMultiplier, 0f, MaxDamage);

    //                    GameController.instance.skillTree.CheckModifiers(SkillTree.ModifierType.damage, ref damage);

    //                    Debug.Log(gameObject.name + " hit " + other.name + " for " + damage);

    //                    Vector3 otherPosHigh;
    //                    otherPosHigh = other.transform.position;
    //                    otherPosHigh.y += 1;
    //                    Vector3 direction = otherPosHigh - transform.position;
    //                    direction.y += 0.2f;
    //                    direction = direction.normalized;
    //                    other.gameObject.GetComponentInChildren<ParticleSystem>().Play();
    //                    //if (other.gameObject.GetComponent<Health>())
    //                    //{

    //                    float modifiedForce = force;
    //                    GameController.instance.skillTree.CheckModifiers(SkillTree.ModifierType.knockback, ref modifiedForce);

    //                    //Knock the enemy up in the air for a bit of FUN
    //                    Vector3 highDirection = direction;
    //                    highDirection.y += 1f;

    //                    other.gameObject.GetComponent<Health>().TakeDamage(damage, gameObject, highDirection * modifiedForce); //direction * force);
    //                    if (!source.isPlaying) source.Play();
    //                    //}

    //                    //itm.TakeDamage(durabilityLoss);


    //                    //if (other.gameObject.GetComponent<AICharacterControl>())
    //                    //{
    //                    //    Vector3 toTarget = (other.transform.position - transform.position).normalized;
    //                    //    other.gameObject.GetComponent<Animator>().applyRootMotion = false;
    //                    //    if (Vector3.Dot(toTarget, other.transform.forward) < 0)
    //                    //    {
    //                    //        other.gameObject.GetComponent<Animator>().SetTrigger("KnockoutFront");
    //                    //    }
    //                    //    else
    //                    //    {
    //                    //        other.gameObject.GetComponent<Animator>().SetTrigger("KnockoutBack");
    //                    //    }

    //                    //    other.gameObject.GetComponent<AICharacterControl>().enabled = false;
    //                    //    other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
    //                    //    
    //                    //}
    //                }
    //            }
    //        }
    //    }
    //}
}
