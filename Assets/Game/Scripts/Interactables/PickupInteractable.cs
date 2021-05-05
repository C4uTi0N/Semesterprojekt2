using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractable : MonoBehaviour, Interactable
{
    public UnityEngine.Events.UnityEvent onInteract;
    private Inventory inventory;
    public string getInteractableText()
    {
        return "press [E] to Pickup item!";
    }
    public void onInteraction()
    {
        onInteract.Invoke();
        inventory.addToInventory(name);
        Destroy(gameObject);
    }

    public void Awake()
    {
        inventory = GameObject.Find("Player Character").GetComponent<Inventory>();

    }
}
