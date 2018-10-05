using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class RelfexPotion : Potion {

    public float timeScale = 0.5f;

    private TimeScaler scaler;

    private PostProcessingBehaviour _postProcessing;

    [SerializeField] private PostProcessingProfile coldPostProcessing;
    [SerializeField] private PostProcessingProfile standardPostProcessing;

    public override void StartOvrd()
    {
        _postProcessing = Camera.main.GetComponent<PostProcessingBehaviour>();
    }

    public override void DrinkEffectsOvrd()
    {
        scaler = new TimeScaler(timeScale);
        TimeScaleManager.instance.AddTimeScale(scaler);
        TimeScaleManager.instance.AddUnscaledAnimator(item.owner.GetComponent<Animator>());

        _postProcessing.profile = coldPostProcessing;


    }

    public override void ExpireEffectsOvrd()
    {
        TimeScaleManager.instance.RemoveTimeScale(scaler);
        TimeScaleManager.instance.RemoveUnscaledAnimator(item.owner.GetComponent<Animator>());
        _postProcessing.profile = standardPostProcessing;
    }
}
