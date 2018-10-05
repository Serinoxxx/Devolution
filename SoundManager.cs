using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public void PlaySound(AudioClip clip, float pitch = 1f, float volume = 0.7f)
    {
        AudioSource source = null;
        //Find an available audioSource
        foreach (AudioSource src in GetComponents<AudioSource>())
        {
            //If the source is a loop then it's the running source so dont steal this one!
            if (!src.isPlaying && !src.loop)
            {
                source = src;
            }
        }

        //Create a new audiosource if we couldn't find one
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        source.pitch = Random.Range(pitch - .2f, pitch + .2f);
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }
}
