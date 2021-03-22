using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, Interactable
{
    public Transform door;
    public UnityEngine.Events.UnityEvent onInteract;

    public string getInteractableText()
    {
        return "press [E] to open the door";
    }

    public void onInteraction()
    {
        onInteract.Invoke();
    }

    private void Awake()
    {
        door = GetComponent<Transform>();
    }
}
