using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public bool isAI;
    public List<Item> items;
    public int maxItems = 2;
    public List<GameObject> itemUIElements;
    public List<Transform> holsterSlots;
    public Transform equippedSlot;
    public Item equippedItem;

    public Color highlight;

    public int heavyAmmo;
    public int mediumAmmo;
    public int lightAmmo;
    public Text ammoText;

    public Image reloadSymbol;

    private GameObject selectedItem;

    public Item pickupItem;

    public Text UIItemInfo;

    public GameObject UILockedPanel;

	// Use this for initialization
	void Start () {

    }

    private bool isLocked = false;

    public void Lock()
    {
        isLocked = true;
        UILockedPanel.SetActive(true);
    }

    public void Unlock()
    {
        isLocked = false;
        UILockedPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

       
        if (!isAI && !isLocked)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) selectItem(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) selectItem(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) selectItem(2);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (pickupItem != null)
                {
                    addItem(pickupItem);
                    pickupItem = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                dropSelectedItem();
            }
        }
    }

    public void addItem(Item item)
    {
        if (items.Count < maxItems)
        {
            items.Add(item);

            if (!isAI)
            {
                //itemUIElements[items.IndexOf(item)].GetComponentInChildren<Text>().text = item.name;
                itemUIElements[items.IndexOf(item)].GetComponentsInChildren<Image>()[1].sprite = item.icon;
                itemUIElements[items.IndexOf(item)].GetComponentsInChildren<Image>()[1].color = Color.white;
                GameController.instance.interactText.text = "";
            }
                
            item.interactCollider.enabled = false;
            if (item.damageCollider != null) item.damageCollider.enabled = true;
            item.isInInventory = true;
            //item.transform.SetParent(holsterSlots[items.IndexOf(item)]);
            item.transform.localEulerAngles = item.holsteredRot;
            item.transform.localPosition = item.holsteredPos;

            if (item.GetComponent<MeleeWeapon>())
            {
                item.GetComponent<MeleeWeapon>().ownerHealth = gameObject.GetComponent<Health>();
            }

            item.owner = gameObject;

            pickupItem = null;
        }
    }

    public void removeItem(Item item)
    {
        GameObject UIItem = itemUIElements[items.IndexOf(item)];
        //UIItem.GetComponentInChildren<Text>().text = "";
        UIItem.GetComponentsInChildren<Image>()[1].sprite = null;
        UIItem.GetComponentsInChildren<Image>()[1].color = Color.clear;

        //Add and remove the UI element so that the index is pushed to the end
        itemUIElements.Remove(UIItem);
        itemUIElements.Add(UIItem);
        UIItem.transform.SetAsLastSibling();
        renumberItemSlots();

        items.Remove(item);
        item.isInInventory = false;
        item.interactCollider.enabled = true;
        if (item.damageCollider != null) item.damageCollider.enabled = false;
        item.transform.parent = null;
        equippedItem = null;
        UIItemInfo.text = "";

        //Automatically select the next available item
        foreach (Item itm in items)
        {
            selectItem(items.IndexOf(itm));
            break;
        }
    }

    public void deselectCurrentItem()
    {
        Item item;
        //First deselect the previously selected Item
        if (selectedItem != null)
        {
            selectedItem.GetComponentInChildren<Outline>().effectColor = Color.black;
            int index = itemUIElements.IndexOf(selectedItem);
            if (items.Count > index)
            {
                item = items[index];
                item.isEquipped = false;
                //item.transform.SetParent(holsterSlots[index]);
                item.transform.localPosition = item.holsteredPos;
                item.transform.localEulerAngles = item.holsteredRot;

                equippedItem = null;

                item.Holster();
            }
        }
    }

    public void selectItem(int slot)
    {
        if (isAI)
        {
            selectItemAI(slot);
        }
        else
        {
            Item item;
            //First deselect the previously selected Item
            deselectCurrentItem();

            //Select the new item
            selectedItem = itemUIElements[slot];

            //"EMPTY" item means it's the placeholder so dont do any of this
            if (items.Count > slot)
            {
                item = items[slot];
                //item.transform.SetParent(equippedSlot);
                item.transform.localPosition = item.equippedPos;
                item.transform.localEulerAngles = item.equippedRot;
                item.isEquipped = true;
                equippedItem = item;
                item.Equip();

                if (item.GetComponent<MeleeWeapon>())
                {
                    MeleeWeapon wep = item.GetComponent<MeleeWeapon>();
                    UIItemInfo.text = string.Format("Left Click: {0} \n Right Click: {1} \n V: Vomit (Cancel Potion)", wep.primaryAttack, wep.secondaryAttack);
                }
                else
                {
                    UIItemInfo.text = "";
                }
            }

            if (selectedItem != null)
            {
                selectedItem.GetComponentInChildren<Outline>().effectColor = highlight;
            }

        }
    }

    //Resets the numbers when we remove items
    private void renumberItemSlots()
    {
        foreach (GameObject UIItem in itemUIElements)
        {
            UIItem.GetComponentInChildren<Text>().text = "" + (itemUIElements.IndexOf(UIItem) + 1);
        }
    }

    //currently only supports 1 item
    private void selectItemAI(int slot)
    {
        Item item;
        item = items[slot];
        item.transform.SetParent(equippedSlot);
        item.transform.localPosition = item.equippedPos;
        item.transform.localEulerAngles = item.equippedRot;
        item.isEquipped = true;
        equippedItem = item;
    }

    public void dropSelectedItem()
    {
        Item itm = items[itemUIElements.IndexOf(selectedItem)];
        if (selectedItem != null)
        {

            RaycastHit hit;
            Physics.Raycast(itm.transform.position, Vector3.down, out hit);
            itm.transform.position = hit.point;
            itm.transform.rotation = itm.startRotation;
            itm.isEquipped = false;
            equippedItem = null;

            removeItem(itm);

        }
    }

    public void forceEquip(Item item)
    {
        addItem(item);
        selectItem(items.IndexOf(item));
    }
}
