using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private static SFXManager instance;
    private static AudioSource audioSource;
    private static AudioSource dialogueaudioSource;
    private static SoundEffectLibrary soundEffectLibrary;

    void Awake()
    {
        if (instance == null)
        {
            instance=this;
            AudioSource [] audioSources =GetComponents<AudioSource>();
            audioSource=audioSources[0];
            dialogueaudioSource=audioSources[1];
            soundEffectLibrary=GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else {Destroy(gameObject);}
    }

    public static void play(string soundname)
    {
        AudioClip audioClip = soundEffectLibrary.Getrandomclip(soundname);
        if(audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public static void Playvoice(AudioClip audioClip, float pitch=1f)
    {
        dialogueaudioSource.pitch=pitch;
        dialogueaudioSource.PlayOneShot(audioClip);
    }


}
