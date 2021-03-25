using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, Interactable
{
    public UnityEngine.Events.UnityEvent onInteract;

    public string getInteractableText()
    {
        return "press [E] to open the door";
    }

    public void onInteraction()
    {
        onInteract.Invoke();
    }
}
