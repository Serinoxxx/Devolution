using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour {

    public Item startingItem;

	// Use this for initialization
	void Start () {
        GameController.instance.inv.forceEquip(startingItem);
    }
}
