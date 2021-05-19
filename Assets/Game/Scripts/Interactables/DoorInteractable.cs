using UnityEngine;

public class DoorInteractable : MonoBehaviour, Interactable
{
    public SmolController smolController;

    public string pressETo = " enter/go to [destination name]";
    public UnityEngine.Events.UnityEvent onInteract;

    public AudioClip soundClip;
    public AudioSource audioManager;

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
        }
        else
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
                audioManager.PlayOneShot(soundClip);
            }
        }
        else
        {
            onInteract.Invoke();
            audioManager.PlayOneShot(soundClip);
        }
    }
}