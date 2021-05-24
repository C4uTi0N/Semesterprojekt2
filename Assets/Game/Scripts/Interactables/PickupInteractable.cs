using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickupInteractable : MonoBehaviour, Interactable
{
    public UnityEngine.Events.UnityEvent onInteract;
    private AudioSource audioSrc;
    private Inventory inventory;

    public void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        inventory = GameObject.Find("Player Character").GetComponent<Inventory>();
    }

    public string getInteractableText()
    {
        return "press [E] to Pickup item!";
    }

    public void onInteraction()
    {
        onInteract.Invoke();
        audioSrc.PlayOneShot(audioSrc.clip);
        inventory.addToInventory(name);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}