using UnityEngine;
using Yarn.Unity;

public class SmolInteractable : MonoBehaviour, Interactable
{
    public Inventory playerInv;
    private DialogueUI dialogueUI;
    private InMemoryVariableStorage yarnMemmory;

    public bool shownFood;

    private void Awake()
    {
        playerInv = GameObject.Find("Player Character").GetComponent<Inventory>();
        dialogueUI = GameObject.Find("Dialogue Runner").GetComponent<DialogueUI>();
        yarnMemmory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();
    }

    public string getInteractableText()
    {
        if (playerInv.hasItem("cereal box"))
        {
            if (!yarnMemmory.TryGetValue<bool>("$foodShown", out bool foodShown))
            {
                return "Press E to show Smol the box";
            }
        }
        return "";
    }

    public void onInteraction()
    {
        if (playerInv.hasItem("cereal box"))
        {
            playerInv.removeFromInventory("cereal box");
            yarnMemmory.SetValue("$foodShown", true);
            yarnMemmory.SetValue("$highlight", false);
            GameObject.Find("Milk Box").GetComponent<BoxCollider>().enabled = false;
            dialogueUI.MarkLineComplete();
        }
    }
}