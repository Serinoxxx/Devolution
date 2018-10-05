using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler
{
    public float timeScale;

    public TimeScaler(float timeScale)
    {
        this.timeScale = timeScale;
    }
}

public class TimeScaleManager : MonoBehaviour {

    public static TimeScaleManager instance;
    List<TimeScaler> timeScalers = new List<TimeScaler>();
    List<Animator> unscaledAnimators = new List<Animator>();

    private void Start()
    {
        instance = this;

        //Add a default scaler so we always have one in the list. NEVER remove this
        TimeScaler defaultScaler = new TimeScaler(1f);
        timeScalers.Add(defaultScaler);
    }

    public void AddTimeScale(TimeScaler scaler)
    {
        //Set the timeScale and add the new scaler to the List
        Debug.Log("Adding timescale: " + scaler.timeScale);
        Time.timeScale = scaler.timeScale;
        timeScalers.Add(scaler);
    }

    public void RemoveTimeScale(TimeScaler scaler)
    {
        //Remove the scaler and set the scale to the last scaler in the list
        Debug.Log("Removing timescale: " + scaler.timeScale);
        timeScalers.Remove(scaler);
        Time.timeScale = timeScalers[timeScalers.Count - 1].timeScale;
    }

    public void AddUnscaledAnimator(Animator anim)
    {
        //Add the animator to the list and set the update mode to unscaled
        Debug.Log("Adding unscaled animator: " + anim.gameObject.name);
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        unscaledAnimators.Add(anim);
    }

    public void RemoveUnscaledAnimator(Animator anim)
    {
        //Remove the animator from the list and set the update mode to normal
        Debug.Log("Removing unscaled animator: " + anim.gameObject.name);
        unscaledAnimators.Remove(anim);
        anim.updateMode = AnimatorUpdateMode.Normal;
    }

    //Call this to override the unscaled animators to normal (i.e. when pausing)
    public void NormaliseAnimators()
    {
        foreach (Animator anim in unscaledAnimators)
        {
            anim.updateMode = AnimatorUpdateMode.Normal;
        }
    }

    //Call this to set the unscaled animators to unscaled when resuming (after pausing)
    public void UnscaleAnimators()
    {
        foreach (Animator anim in unscaledAnimators)
        {
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }
}
