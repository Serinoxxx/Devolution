using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class RadialMenu : MonoBehaviour {

    public GameObject RadialButtonPFB;

    public List<RadialButton> buttons;

    public float spacing; //In degrees

	// Use this for initialization
	void Start () {

        float fillAmount = 1f / buttons.Count - spacing/360f;
        float rotationIncrement = 360 / buttons.Count - spacing;
        float rotation = 0;

        foreach (RadialButton button in buttons)
        {
            rotation += rotationIncrement;

            GameObject newButton = GameObject.Instantiate(RadialButtonPFB, transform);

            newButton.SetActive(true);

            //Resize the element
            newButton.GetComponent<RectTransform>().Rotate(new Vector3(0f, 0f, rotation));
            newButton.GetComponent<Image>().fillAmount = fillAmount;

            //Prevent the transparent parts of the image to not trigger the button
            newButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f; 

            newButton.GetComponentInChildren<Text>().text = button.commandName.ToString();

            //Rotate the text in the opposite direction
            newButton.GetComponentInChildren<Text>().transform.Rotate(new Vector3(0f, 0f, -rotation));
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
	}
}

public enum commandNames { Attack, Defend, Loot, Follow }

[System.Serializable]
public class RadialButton
{
    public commandNames commandName;
}
