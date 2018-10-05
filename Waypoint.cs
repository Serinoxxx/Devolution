using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Waypoint : MonoBehaviour {

    public commandNames command;

    public Animation anim;

    public LayerMask layers;

    //public MeshRenderer rend;

    public Color attackColor;
    public Color defendColor;
    public Color lootColour;

    private bool active = true;
	
	void LateUpdate () {
        if (active)
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f, layers);
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward);
            transform.position = hit.point;
            transform.LookAt(Camera.main.transform);
        }
	}

    private void Update()
    {
        if (active)
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                Execute();
            }
            else if (CrossPlatformInputManager.GetButtonDown("Fire2"))
            {
                Deactivate();
            }
        }
    }

    //Execute the active command
    public void Execute()
    {
        anim.Play("IndicatorAnim");
        active = false;

        foreach (AIAllyCharacterControl ally in GameController.instance.squadMgr.Allies)
        {
            if (ally.isSelected)
            {
                switch (command)
                {
                    case commandNames.Attack:
                        ally.stateMachine.ChangeState(AttackState.Instance);
                        ally.SetTarget(transform);
                        break;
                    case commandNames.Defend:
                        ally.stateMachine.ChangeState(DefendState.Instance);
                        ally.SetTarget(transform);
                        break;
                    case commandNames.Loot:
                        ally.stateMachine.ChangeState(LootState.Instance);
                        ally.SetTarget(transform);
                        break;
                }
                
            }
        }

        StartCoroutine(fadeAlpha());
        Invoke("Deactivate", 1f);
    }

    private IEnumerator fadeAlpha()
    {
        float duration = 1f;
        float startTime = Time.time;
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        while (Time.time < startTime + duration)
        {
            foreach (MeshRenderer rend in renderers)
            {
                Color clr = rend.material.color;
                Color newColor = new Color(clr.r, clr.g, clr.b, clr.a - (Time.deltaTime / duration));
                rend.material.SetColor("_Color", newColor);
            }
            yield return null;
        }
    }

    //Appear
    public void Activate()
    {
        foreach (MeshRenderer rend in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            switch (command)
            {
                case commandNames.Attack:
                        rend.material.SetColor("_Color", attackColor);
                    break;
                case commandNames.Defend:
                    rend.material.SetColor("_Color", defendColor);
                    break;
                case commandNames.Loot:
                    rend.material.SetColor("_Color", lootColour);
                    break;
            }
        }

        GameController.instance.squadMgr.isWayPointActive = true;
    }

    //Disappear
    public void Deactivate()
    {
        GameController.instance.squadMgr.isWayPointActive = false;
        Destroy(gameObject);
    }
}
