using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPotion : Potion
{ 
    public GameObject spiderClawPfb;

    Item spiderClaw;
    Inventory inv;

    public override void DrinkEffectsOvrd()
    {
        item.owner.GetComponent<Animator>().SetInteger("mutationState", (int)mutationState);

        inv = item.owner.GetComponent<Inventory>();

        spiderClaw = item.owner.GetComponentInChildren<Equippables>().ActivateItem("spiderClaw");
        item.owner.GetComponentInChildren<Equippables>().ActivateItem("spiderHead");
        inv.deselectCurrentItem();
        inv.equippedItem = spiderClaw;
        // inv.forceEquip(spiderClaw);
        // inv.addItem(spiderClaw);
        // inv.selectItem(inv.items.IndexOf(spiderClaw));


        item.owner.GetComponentInChildren<Equippables>().DeactivateItem("mainHead");

    }

    public override void ExpireEffectsOvrd()
    {
        item.owner.GetComponent<Animator>().SetInteger("mutationState", (int)Enums.MutationStateType.none);
        //inv.removeItem(spiderClaw);
        inv.equippedItem = null;

        item.owner.GetComponentInChildren<Equippables>().DeactivateItem("spiderClaw");
        item.owner.GetComponentInChildren<Equippables>().DeactivateItem("spiderHead");


        item.owner.GetComponentInChildren<Equippables>().ActivateItem("mainHead");
    }

    public override void StartOvrd()
    {
        // throw new NotImplementedException();
    }
}
