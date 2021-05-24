using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorInteractable : MonoBehaviour, Interactable
{
    public SmolController smolController;

    public string pressETo = " enter/go to [destination name]";
    public UnityEngine.Events.UnityEvent onInteract;

    private AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

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
                audioSrc.PlayOneShot(audioSrc.clip);
            }
        }
        else
        {
            onInteract.Invoke();
            audioSrc.PlayOneShot(audioSrc.clip);
        }
    }
}