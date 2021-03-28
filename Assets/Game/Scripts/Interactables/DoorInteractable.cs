using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, Interactable
{
    public string pressETo = " enter/go to [destination name]";
    public UnityEngine.Events.UnityEvent onInteract;

    public string getInteractableText()
    {
        return ("press [E] to " + pressETo);
    }

    public void onInteraction()
    {
        onInteract.Invoke();
    }
}
