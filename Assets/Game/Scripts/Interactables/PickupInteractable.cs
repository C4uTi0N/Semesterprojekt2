using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractable : MonoBehaviour, Interactable
{
    public UnityEngine.Events.UnityEvent onInteract;

    public string getInteractableText()
    {
        return "press [E] to Pickup item!";
    }
    public void onInteraction()
    {
        Destroy(gameObject);
    }
    
}
