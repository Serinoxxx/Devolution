using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SporeCondenser : MonoBehaviour {

    [System.Serializable]
    public class PotionRecipe
    {
        public Enums.SporeColors color1;
        public Enums.SporeColors color2;
        public GameObject potionPrefab;
    }

    public Transform potionSpawnPoint;

    public PotionRecipe[] potionRecipes;

    public Slider potionSlider;
    public GameObject UISporeConsender;
    public Image potionLiquid;
    public float fillDuration;
    public int sporeCost;

    public Color startColor;
    public Color redColor;
    public Color greenColor;
    public Color blueColor;
    public float saturation = 1.5f;

    public AudioClip fillingSound;
    public AudioClip corkSound;
    public Animation corkAnimation;

    public GameObject bubbles;

    private bool isFilling;

    private int colorIndex; //the index of the color we're adding to the potion

    private Enums.SporeColors[] potionColors = new Enums.SporeColors[2];

    private bool isPotionComplete;

    //private void Update()
    //{
    //    //When we press tab, show or hide the mutations panel in the UI
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        //determine whether to show or hide based on the current active state
    //        if (UISporeConsender.activeInHierarchy)
    //        {
    //            HideCondenserPanel();
    //        }
    //        else
    //        {
    //            ShowCondenserPanel();
    //        }
    //    }
    //}

    public void ShowCondenserPanel()
    {
        UISporeConsender.SetActive(true);
        ResetPotion();

        GameController.instance.player.GetComponent<Movement>().addLock();

        Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResetPotion()
    {
        potionSlider.value = 0;
        potionLiquid.color = startColor;
        StopAllCoroutines();
        bubbles.SetActive(false);
        isFilling = false;
        potionColors = new Enums.SporeColors[2];
        colorIndex = 0;
        isPotionComplete = false;
        GameController.instance.sporeSystem.ResetPending();
    }

    public void HideCondenserPanel()
    {
        UISporeConsender.SetActive(false);

        Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameController.instance.sporeSystem.ResetPending();

        GameController.instance.player.GetComponent<Movement>().removeLock();
    }

    public void ToggleCondenserPanel()
    {
        if (UISporeConsender.activeInHierarchy)
        {
            HideCondenserPanel();
        }
        else
        {
            ShowCondenserPanel();
        }

    }

    public void CreateClick()
    {

        if (!isPotionComplete) return;

        //Loop through our recipes to find one that aligned with the selected colors
        foreach(PotionRecipe recipe in potionRecipes)
        {
            if (recipe.color1 == potionColors[0] && recipe.color2 == potionColors[1]
                || recipe.color1 == potionColors[1] && recipe.color2 == potionColors[0])
            {
                //Create the potion
                GameObject.Instantiate(recipe.potionPrefab, potionSpawnPoint.position, Quaternion.identity);

                //Consume the selected colored spores
                GameController.instance.sporeSystem.ConsumeSpores(recipe.color1, sporeCost);
                GameController.instance.sporeSystem.ConsumeSpores(recipe.color2, sporeCost);

                HideCondenserPanel();
                GameController.instance.soundManager.PlaySound(corkSound, 1f, 0.6f);
                return;
            }
        }

        Debug.LogWarning("Unable to find recipe with colors: " + potionColors[0] + " and " + potionColors[1]);
    }


    public void SporeClick(string colorString)
    {
        //Don't start filling the potion if it's already filling or it's full!
        if (isFilling || potionSlider.value > 0.55f) return;

        //Determine the color of the spore to be added
        Color color = Color.white;
        Enums.SporeColors sporeColor = Enums.SporeColors.red;
        switch (colorString)
        {
            case "red":
                color = redColor;
                sporeColor = Enums.SporeColors.red;
                break;
            case "green":
                color = greenColor;
                sporeColor = Enums.SporeColors.green;
                break;
            case "blue":
                color = blueColor;
                sporeColor = Enums.SporeColors.blue;
                break;
        }

        //Make sure we have enough spores to add to the potion
        if (GameController.instance.sporeSystem.SporesAvailable(sporeColor, sporeCost))
        {
            potionColors[colorIndex++] = sporeColor;
            StartCoroutine(FillingPotion(color));
        }

    }

    private IEnumerator FillingPotion(Color color)
    {

        GameController.instance.soundManager.PlaySound(fillingSound, 1f + potionSlider.value, 1f);

        
        Color mixedColor;
        //If this is the first fill, then use the base color
        if (potionSlider.value == 0f)
        {
            mixedColor = color;
        }
        //This is the second fill, so mix with the base color
        else
        {
            mixedColor = (potionLiquid.color + color) / 2;

            //add saturation
            mixedColor *= saturation;
        }

        float smoothness = 0.02f;
        float progress = 0f;
        float initialSliderValue = potionSlider.value;
        float increment = smoothness / fillDuration;
        float newSliderValue = potionSlider.value + 0.5f;

        isFilling = true;
        while (progress < newSliderValue)
        {
            Color newColor = Color.Lerp(potionLiquid.color, mixedColor, progress);
            progress += increment;
            potionLiquid.color = newColor;
            potionSlider.value = progress + initialSliderValue;
            yield return new WaitForSeconds(smoothness); // Maybe try time.deltaTime?
        }
        if (newSliderValue >= 0.6f)
        {
            corkAnimation.Play();
            bubbles.SetActive(true);
            isPotionComplete = true;
        }
        isFilling = false;
    }

}
