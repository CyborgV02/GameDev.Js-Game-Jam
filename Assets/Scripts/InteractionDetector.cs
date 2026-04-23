using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
   private IInteractable interactableInRange=null;
   
   [SerializeField]
   private GameObject interactionIcon;
    void Start()
    {
        interactionIcon.SetActive(false);
    }

    void OnEnable()
    {
        InputController.OnActionZ+=TryInteract;
    }

    void OnDisable()
    {
        InputController.OnActionZ-=TryInteract;
    }

    private void TryInteract()
    {
         interactableInRange?.interact();
    }



    void OnTriggerEnter2D(Collider2D collision)
{

    IInteractable interactable = collision.GetComponentInParent<IInteractable>();
    
    if (interactable != null && interactable.CanInteract())
    {
        interactableInRange = interactable;
        interactionIcon.SetActive(true);
    }
}

void OnTriggerExit2D(Collider2D collision)
{
    IInteractable interactable = collision.GetComponentInParent<IInteractable>();
    
    if (interactable != null && interactable.CanInteract())
    {
        interactableInRange = null;
        interactionIcon.SetActive(false);
    }
}


}
