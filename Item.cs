using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public string name = "Default Item";
    public string animationTriggerName = "Attack";
    public Sprite icon;

    public enum itemType
    {
        meleeWeapon, rifle, pistol, consumable
    }
    public itemType type;

    public enum animationType
    {
        attack, drink
    }
    public animationType animType;

    public float damage = 1f;

    public bool isEquipped = false;
    public bool isInInventory = false;

    public Vector3 holsteredPos;
    public Vector3 holsteredRot;

    public Vector3 equippedPos;
    public Vector3 equippedRot;

    public float durability = 100f;

    public SphereCollider damageCollider;
    public Collider interactCollider;

    private float startingDurability;

    public Quaternion startRotation;

    public bool isReloading;

    public GameObject owner;


    public void Start()
    {
        startingDurability = durability;
        if (damageCollider != null) damageCollider.enabled = false;
        interactCollider.enabled = true;
        startRotation = transform.rotation;
    }

    public virtual void Equip()
    {

    }

    public virtual void Holster()
    {

    }

    public void Consume()
    {
        owner.GetComponent<Inventory>().dropSelectedItem();
        interactCollider.enabled = false;
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }
    }

    public void TakeDamage(float damage)
    {
        durability -= damage;
        if (durability <= 0)
        {
            Destroyed();
        }
    }

    private void Destroyed()
    {
        GameController.instance.inv.removeItem(this);
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isInInventory)
        {

            if (other.gameObject.GetComponent<ThirdPersonUserControl>())
            {
                GameController.instance.interactText.text = "Press (E) to pickup " + name;
                GameController.instance.inv.pickupItem = this;
            }
            else if (other.gameObject.GetComponent<Inventory>())
            {
                other.gameObject.GetComponent<Inventory>().pickupItem = this;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!isInInventory)
        {
            if (other.gameObject.GetComponent<ThirdPersonUserControl>())
            {
                GameController.instance.interactText.text = "";
                GameController.instance.inv.pickupItem = null;
            }
        }
    }

    public Vector3 GunRaycastHit()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, 50f))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }







}
