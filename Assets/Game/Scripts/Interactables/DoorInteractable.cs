using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, Interactable
{
    public SmolController smolController;

    public string pressETo = " enter/go to [destination name]";
    public UnityEngine.Events.UnityEvent onInteract;

    public string getInteractableText()
    {
        if (smolController != null)
        {
            if (smolController.shouldFollow)
            {
                return ("Press [E] to " + pressETo);
            }
            else
            {
                return ("You shouldn't leave yet");
            }
        } else
        {
            return ("Press [E] to " + pressETo);
        }
    }

    public void onInteraction()
    {
        if (smolController != null)
        {
            if (smolController.shouldFollow)
            {
                onInteract.Invoke();
            }
        }
        else
        {
            onInteract.Invoke();
        }
    }
}
