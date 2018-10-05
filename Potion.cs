using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Item))]
public abstract class Potion : MonoBehaviour {
    public Enums.MutationStateType mutationState;
    public Item item;
    public float duration = 5f;
    public Color UIPotionColor;
    public bool lockInventory;
    public bool discardOnUse = true;

    private Image liquid;
    private void Start()
    {
        item = gameObject.GetComponent<Item>();
        liquid = GameController.instance.potionLiquidImage;
        StartOvrd();
    }
    public void DrinkEffects()
    {
        //Discard this potion and dont allow it to be picked up again
        if (discardOnUse)
        {
            ObjectPooler.instance.QuickSpawn("emptyTestTube", transform.position, true, 5f);
            item.Consume();
        }
        if (lockInventory)
            item.owner.GetComponent<Inventory>().Lock();

        DrinkEffectsOvrd();
        Invoke("ExpireEffects", duration);
        PotionController.instance.PotionDraining(this);
        liquid.color = UIPotionColor;

        


    }

    public void ExpireEffects()
    {
        CancelInvoke();
        ExpireEffectsOvrd();

        if (lockInventory)
            item.owner.GetComponent<Inventory>().Unlock();
    }

    private void UnlockInventory()
    {
        item.owner.GetComponent<Inventory>().Unlock();
    }

    public abstract void StartOvrd();
    public abstract void DrinkEffectsOvrd();
    public abstract void ExpireEffectsOvrd();
}
