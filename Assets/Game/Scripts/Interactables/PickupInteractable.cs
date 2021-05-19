using UnityEngine;

public class PickupInteractable : MonoBehaviour, Interactable
{
    public UnityEngine.Events.UnityEvent onInteract;
    private Inventory inventory;

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
        inventory.addToInventory(name);
        Destroy(gameObject);
    }
}