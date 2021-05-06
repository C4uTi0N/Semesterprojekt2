using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SmolInteractable : MonoBehaviour, Interactable
{
    private InMemoryVariableStorage yarnMemmory;
    private GameObject player;

    void Awake()
    {
        player = GameObject.Find("Player Character");
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
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
