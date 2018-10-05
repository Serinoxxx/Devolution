using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Skill))]
public class SkillNode : MonoBehaviour
{
    public bool isAvailable;
    public bool isActive;
    public List<SkillNode> childNodes; //Assigned in inspector


    public Skill skill;

    public SkillTree skillTree;

    public void Awake()
    {
        skill = gameObject.GetComponent<Skill>();
        skillTree = gameObject.GetComponentInParent<SkillTree>();
    }

    public void OnClick()
    {
        if (isAvailable && !isActive)
        {
            Activate();
        }
        else
        {
            //Play error sound
        }
    }

    public void Activate()
    {
        isActive = true;

        gameObject.GetComponent<Image>().color = skillTree.activeColour;

        //Activate passive abilities
        if (skill.isPassive)
            skill.Activate();

        foreach (SkillNode node in childNodes)
        {
            node.SetAvailable();
        }
    }

    public void SetAvailable()
    {
        isAvailable = true;
        gameObject.GetComponent<Image>().color = skillTree.availableColour;
        //set image color to white transparent
    }

}
