using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour {

    public AudioClip[] sounds;
    public AudioSource source;

    public float timeBetweenSounds = 1f;
    public float minPitch = 0.2f;
    public float maxPitch = 0.4f;

    private float lastSoundTime;
    private float lastClipLength;
	
	// Update is called once per frame
	void Update () {
		
        if (!source.isPlaying && Time.time > (lastSoundTime + timeBetweenSounds + lastClipLength))
        {
            int i = Random.Range(0, sounds.Length);
            source.clip = sounds[i];
            source.pitch = Random.Range(minPitch, 1f);
            source.Play();

            lastSoundTime = Time.time;
            lastClipLength = source.clip.length;
        }
	}
}
