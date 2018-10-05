using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public List<SkillNode> skillNodes; //Assigned in inspector
    public enum ModifierType { damage, healing, stamina, movespeed, accuracy, knockback }

    public Color activeColour;
    public Color availableColour;
    public Color unavailableColour;

    public GameObject UIMutations;

    private void Start()
    {
        foreach(SkillNode node in skillNodes)
        {
            node.gameObject.GetComponent<Image>().color = unavailableColour;
        }
    }

    private void Update()
    {
        //When we press tab, show or hide the mutations panel in the UI
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //determine whether to show or hide based on the current active state
            if (UIMutations.activeInHierarchy)
            {
                HideMutationsPanel();
            }
            else
            {
                ShowMutationsPanel();
            }
        }
    }

    public void ShowMutationsPanel()
    {
        UIMutations.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = true;

        //Activate the root node
        skillNodes[0].Activate();
    }

    public void HideMutationsPanel()
    {
        UIMutations.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = false;
    }

    public void CheckModifiers(ModifierType type, ref float amount)
    {
        foreach (SkillNode node in skillNodes)
        {
            if (node.isActive)
            {
                Skill skill = node.skill;
                switch (type)
                {
                    case ModifierType.damage:
                        skill.ModifyMeleeDamage(ref amount);
                        break;
                    case ModifierType.healing:
                        skill.ModifyHealingEffect(ref amount);
                        break;
                    case ModifierType.stamina:
                        skill.ModifyStaminaRegen(ref amount);
                        break;
                    case ModifierType.movespeed:
                        skill.ModifyMoveSpeed(ref amount);
                        break;
                    case ModifierType.accuracy:
                        skill.ModifyGunAccuracy(ref amount);
                        break;
                    case ModifierType.knockback:
                        skill.ModifyKnockback(ref amount);
                        break;
                }
            }
        }
    }
}

public abstract class Skill : MonoBehaviour
{
    //All modifiers are percentage based
    public float meleeDamageModifier = 1f;
    public float gunAccuracyModifier = 1f;
    public float healingModifier = 1f;
    public float moveSpeedModifier = 1f;
    public float staminaRegenModifier = 1f;
    public float knockbackModifier = 1f;
    public bool isPassive; //Has no time limit
    public float duration; //duration of ability, only applicable for active abilities
    public bool isActive;

    public void Activate()
    {
        //e.g. play particle system
        //e.g. increase maximum hp
        isActive = true;

        if (!isPassive)
            Invoke("Deactivate", duration);

        OnActivate();

    }

    private void Deactivate()
    {
        isActive = false;
        OnDeactivate();
    }

    public abstract void OnActivate();

    public abstract void OnDeactivate();

    public abstract void ModifyMeleeDamage(ref float damage);
    
    public abstract void ModifyGunAccuracy(ref float accuracy);

    public abstract void ModifyHealingEffect(ref float healing);

    public abstract void ModifyStaminaRegen(ref float stamina);

    public abstract void ModifyMoveSpeed(ref float speed);

    public abstract void ModifyKnockback(ref float knockback);
}