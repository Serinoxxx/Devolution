using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardPotion : Potion {

    public GameObject reptileClawPfb;

    Item reptileClaw;
    Inventory inv;

    public override void DrinkEffectsOvrd()
    {
        item.owner.GetComponent<Animator>().SetInteger("mutationState", (int)mutationState);

        //reptileClaw = GameObject.Instantiate(reptileClawPfb);
        inv = item.owner.GetComponent<Inventory>();

        reptileClaw = item.owner.GetComponentInChildren<Equippables>().ActivateItem("reptileClaw");
        item.owner.GetComponentInChildren<Equippables>().ActivateItem("LizardHead");
        inv.deselectCurrentItem();
        inv.equippedItem = reptileClaw;
        // inv.forceEquip(reptileClaw);
        // inv.addItem(reptileClaw);
        // inv.selectItem(inv.items.IndexOf(reptileClaw));

        item.owner.GetComponentInChildren<Equippables>().DeactivateItem("mainHead");
    }

    public override void ExpireEffectsOvrd()
    {

        item.owner.GetComponent<Animator>().SetInteger("mutationState", (int)Enums.MutationStateType.none);
        // inv.removeItem(reptileClaw);
        inv.equippedItem = null;

        item.owner.GetComponentInChildren<Equippables>().DeactivateItem("reptileClaw");
        item.owner.GetComponentInChildren<Equippables>().DeactivateItem("LizardHead");

        item.owner.GetComponentInChildren<Equippables>().ActivateItem("mainHead");
    }

    public override void StartOvrd()
    {
       // throw new NotImplementedException();
    }
}
