using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionController : MonoBehaviour {

    public static PotionController instance;

    public GameObject potionSlider;

    public Potion activePotion;

    private void Awake()
    {
        instance = this;
    }

    public void ExpireEarly()
    {
        if (activePotion != null)
        {
            StopAllCoroutines();
            activePotion.ExpireEffects();
            activePotion = null;
            potionSlider.SetActive(false);
        }
    }

    public void PotionDraining(Potion potion)
    {
        StopAllCoroutines();
        StartCoroutine(IEPotionDraining(potion));
    }

    public IEnumerator IEPotionDraining(Potion potion)
    {
        activePotion = potion;
        potionSlider.SetActive(true);
        Slider sldPotion = potionSlider.GetComponent<Slider>();
        sldPotion.value = 1f;
        while (sldPotion.value > 0)
        {
            sldPotion.value -= Time.deltaTime / potion.duration;
            yield return null;
        }
        potionSlider.SetActive(false);
        activePotion = null;
    }
}
