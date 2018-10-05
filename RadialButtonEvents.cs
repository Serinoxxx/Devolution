using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RadialButtonEvents : MonoBehaviour {

    public Text commandText;
    public GameObject waypointPfb;

    public void OnClick()
    {
        //Convert the command value to an Enum for use in the switch statement
        commandNames command = (commandNames)Enum.Parse(typeof(commandNames), commandText.text, true);
        //Determine which command we have clicked based on the UI Text component on the button
        switch (command)
        {
            //case commandNames.Attack:
                
            //    break;

            //case commandNames.Defend:
            //    break;
            //case commandNames.Loot:

            //    break;

            //Follow is the only command that doesn't need a waypoint
            case commandNames.Follow:
                SquadManager sqdMgr = GameController.instance.squadMgr;
                foreach (AIAllyCharacterControl ally in sqdMgr.Allies)
                {
                    if (ally.isSelected)
                    {
                        ally.stateMachine.ChangeState(FollowState.Instance);
                    }
                }
                break;
            default:
                Waypoint waypoint = GameObject.Instantiate(waypointPfb).GetComponent<Waypoint>();
                waypoint.command = command;
                waypoint.Activate();
                break;
        }
    }
}
