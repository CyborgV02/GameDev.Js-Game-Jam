using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour,IInteractable
{
  public Dialogue dialogueData;
  public GameObject dialoguePanel;
  public TMP_Text dialogueText,nameText;
  private int dialogueIndex;
  private bool isTyping,isDialogueActive;

    public bool CanInteract()
    {
       return !isDialogueActive;
    }

    public void interact()
    {
        if (dialogueData == null)
        {
            return;
        }

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
           
        }
    }

    void StartDialogue()
    {
        isDialogueActive=true;
        dialogueIndex=0;
        nameText.SetText(dialogueData.recordName);
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
        PauseManager.SetPause(true);
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping=false;
            return;
        }
        

        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }

        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping=true;
        dialogueText.SetText("");
        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text+=letter;
            SFXManager.Playvoice(dialogueData.voiceSound,dialogueData.voicePitch);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping=false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoprogressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive=false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseManager.SetPause(false);
        
    }
}
