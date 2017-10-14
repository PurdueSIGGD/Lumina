using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioSource : MonoBehaviour {
    public AudioClip[] clips;
    private AudioSource source;
    public bool looping;
    public bool oneOnly; // One only if a looping, single constant sound
    public bool playOnAwake;
    public float delay;
	// Use this for initialization
	void Start () {
        source = this.GetComponent<AudioSource>();
        source.loop = false;
        if (playOnAwake) {
            if (oneOnly) {
                int singleSound = Random.Range(0, clips.Length - 1);
                source.clip = clips[singleSound];
                source.loop = true;
                source.Play();
            } else {
                StartCoroutine(RandomClip());
            }
        }
	}
	
	/**
     * Plays a sound, loops, and then picks another sound
     */
    IEnumerator RandomClip() {
        if (clips.Length > 0) {
            do {
                yield return new WaitForSeconds(delay);
                int index = Random.Range(0, clips.Length - 1);
                source.clip = clips[index];
                source.Play();
            } while (looping);

        }
      
    }
   
    public void PlayOnce() {
        Stop();
        int index = Random.Range(0, clips.Length - 1);
        source.clip = clips[index];
        source.Play();
        //if (looping) {
        //StartCoroutine(RandomClip());
        //} else {
        //    int index = Random.Range(0, clips.Length - 1);
        //    source.clip = clips[index];
        //    source.Play();
        //}

    }
    public void Stop() {
        source.Stop();
        if (looping) {
            StopCoroutine(RandomClip());
        }
    }



}
