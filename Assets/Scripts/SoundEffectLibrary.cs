using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    private Dictionary<string , List<AudioClip>> soundDictionary;
    [SerializeField] private sfxGroup[] sfxGroups;

    void Awake()
    {
        InitilizeDictionary();
    }

    private void InitilizeDictionary()
    {
       soundDictionary=new Dictionary<string, List<AudioClip>>();
       foreach (sfxGroup sfxGroup in sfxGroups)
        {
            soundDictionary[sfxGroup.name]=sfxGroup.audioClips;
        }
    }

    public AudioClip Getrandomclip(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundDictionary[name];

            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }

        return null;
    }
}
[System.Serializable]
public struct sfxGroup
{
    public string name;
    public List<AudioClip> audioClips;


}