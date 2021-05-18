using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractable : MonoBehaviour, Interactable
{
    public AudioSource audioManager;
    public UnityEngine.Events.UnityEvent onInteract;
    private Inventory inventory;
    public AudioClip soundClip;


    public void Awake()
    {
        inventory = GameObject.Find("Player Character").GetComponent<Inventory>();
    }

    public string getInteractableText()
    {
        return "press [E] to Pickup item!";
    }

    public void onInteraction()
    {
        onInteract.Invoke();
        audioManager.PlayOneShot(soundClip);
        inventory.addToInventory(name);
        Destroy(gameObject);
    }
}
