using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAllyRowControls : MonoBehaviour {

    public Text txtName; //display only
    public InputField txtNickname; //changeable name
    public Text txtOccupation; //display only - should reflect other stats
    public Text txtToughness; //how much damage character can take
    public Text txtStrength; //how much damage character deals with melee weapons and how much they can carry
    public Text txtSpeed; //how fast character can run

    public Slider sldBravery; //Affects variables: follow distance, chase distance //Adjustable based on the players charisma stat

    public AIAllyCharacterControl ally;
}
