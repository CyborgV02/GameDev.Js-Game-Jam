using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject {

    public string recordName;
    public string[] dialogueLines;
    public float typingSpeed=0.03f;
    public AudioClip voiceSound;
    public float voicePitch=1.0f;
    public bool[] autoProgressLines;
    public float autoprogressDelay=1.5f;
}

   

