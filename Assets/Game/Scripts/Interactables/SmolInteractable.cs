using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SmolInteractable : MonoBehaviour, Interactable
{
    private GameObject player;
    private DialogueUI dialogueUI;
    private InMemoryVariableStorage yarnMemmory;

    void Awake()
    {
        player = GameObject.Find("Player Character");
        dialogueUI = GameObject.Find("Dialogue Runner").GetComponent<DialogueUI>();
        yarnMemmory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();
    }

    public string getInteractableText()
    {
        if (player.GetComponent<Inventory>().hasItem("cereal box"))
        {
            if (!yarnMemmory.TryGetValue<bool>("$foodShown", out bool output))
            {
                return "Press E to show Smol the box";
            }
        }
        return "";
    }

    public void onInteraction()
    {
        if (player.GetComponent<Inventory>().hasItem("cereal box")) 
        {
            yarnMemmory.SetValue("$foodShown", true);
            yarnMemmory.SetValue("$highlight", false);
            dialogueUI.MarkLineComplete();
        }
    }
}
