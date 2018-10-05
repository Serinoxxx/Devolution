using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SquadManager : MonoBehaviour
{

    public List<AIAllyCharacterControl> Allies;

    public List<UIAllyRowControls> AllyRows;

    public List<Portrait> Portraits;

    public GameObject UIAlliesPanel;
    public GameObject UIAlliesBodyPanel;
    public GameObject UIAllyRowPfb; //Prefab for the UI panel that will contain all the buttons/sliders for each ally

    public GameObject UIPortraitsPanel;
    public GameObject UIPortraitPfb;

    public GameObject UICommandWheel;

    public Color selectedColor;
    public Color unselectedColor;

    private bool _isWayPointActive;
    public bool isWayPointActive
    {
        get { return _isWayPointActive; }
        set { _isWayPointActive = value; }
    }

    private bool allAlliesSelected;



    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4
     };

    void Update()
    {

        ////When we press tab, show or hide the allies panel in the UI
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    //determine whether to show or hide based on the current active state
        //    if (UIAlliesPanel.activeInHierarchy)
        //    {
        //        HideAlliesPanel();
        //    }
        //    else
        //    {
        //        ShowAlliesPanel();
        //    }
        //}

        //Show the command wheel when we're holding Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowCommandWheel();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            HideCommandWheel();
        }

        //Removing for now, all selected for demo
        //AllySelection();




    }

    private void AllySelection()
    {
        //Number keys for selecting allies
        for (int i = 0; i < Allies.Count; i++)
        {
            //Tilda means select/deselect all allies
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                if (allAlliesSelected)
                {
                    DeselectAlly(i);
                }
                else
                {
                    SelectAlly(i);
                }

            }
            else if (Input.GetKeyDown(keyCodes[i]))
            {
                if (Allies[i].isSelected)
                {
                    DeselectAlly(i);
                }
                else
                {
                    SelectAlly(i);
                }
            }
        }

        //Swtich the select/deselect all boolean
        if (Input.GetKeyDown(KeyCode.BackQuote))
            allAlliesSelected = !allAlliesSelected;
    }

    public void AddAlly(GameObject ally)
    {
        
        AIAllyCharacterControl AI = ally.GetComponent<AIAllyCharacterControl>();
        //Don't add the ally if we already recruited them
        if (!Allies.Contains(AI))
        {
            //Create a UI row for the allies 
            GameObject UIPortrait = Instantiate(UIPortraitPfb, UIPortraitsPanel.transform);

            Allies.Add(AI);


            Portrait portrait = UIPortrait.GetComponent<Portrait>();
            Portraits.Add(portrait);

            SelectAlly(Allies.IndexOf(AI));

            Health health = ally.GetComponent<Health>();
            health.healthSlider = portrait.sldHealth;
            portrait.txtName.text = AI.attributes.fullname;
            portrait.imgPortrait.sprite = AI.attributes.portrait;
        }

    }

    private void SelectAlly(int index)
    {

        Allies[index].isSelected = true;
        Portraits[index].outline.effectColor = selectedColor;

    }
        
    private void DeselectAlly(int index)
    {
        Allies[index].isSelected = false;
        Portraits[index].outline.effectColor = unselectedColor;
    }

    //TODO
    public void RemoveAlly()
    {

    }

    public void ShowAlliesPanel()
    {
        PopulateAlliesUI();
        UIAlliesPanel.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(UIAlliesPanel.GetComponent<RectTransform>());
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = true;
        // UIAlliesPanel.GetComponent<LayoutRebuilder>().Rebuild(CanvasUpdate.Layout);
    }
    public void HideAlliesPanel()
    {
        SaveAttributeChanges();
        ClearAlliesUI();
        UIAlliesPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = false;
    }

    public void ShowCommandWheel()
    {
        UICommandWheel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = true;
        GameController.instance.player.GetComponent<ThirdPersonUserControl>().isCommanding = true;
    }

    public void HideCommandWheel()
    {
        UICommandWheel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = false;
        GameController.instance.player.GetComponent<ThirdPersonUserControl>().isCommanding = false;
    }

    public void PopulateAlliesUI()
    {
        //Loop through all the players current allies
        foreach (AIAllyCharacterControl ally in Allies)
        {
            //Create a UI row for the allies and 
            GameObject UIAllyRow = Instantiate(UIAllyRowPfb, UIAlliesBodyPanel.transform);
            UIAllyRowControls rowControls = UIAllyRow.GetComponent<UIAllyRowControls>();
            AllyRows.Add(rowControls);
            rowControls.txtName.text = ally.attributes.fullname;
            rowControls.txtNickname.text = ally.attributes.nickname;
            rowControls.txtOccupation.text = ally.attributes.occupation;
            rowControls.txtToughness.text = "" + ally.attributes.toughness;
            rowControls.txtStrength.text = "" + ally.attributes.strength;
            rowControls.txtSpeed.text = "" + ally.attributes.speed;
            rowControls.sldBravery.value = ally.attributes.bravery;
            rowControls.ally = ally;
        }
    }

    public void SaveAttributeChanges()
    {
        foreach (UIAllyRowControls rowControls in AllyRows)
        {
            rowControls.ally.attributes.nickname = rowControls.txtNickname.text;
            rowControls.ally.attributes.bravery = rowControls.sldBravery.value;
        }
    }

    public void ClearAlliesUI()
    {
        foreach (UIAllyRowControls rowControls in AllyRows)
        {
            Destroy(rowControls.gameObject);
        }

        AllyRows.Clear();
    }

}
