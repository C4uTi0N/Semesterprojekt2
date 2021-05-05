using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmolInteractable : MonoBehaviour, Interactable
{
    public string getInteractableText()
    {
        return "Press E to make Smol follow";
    }

    public void onInteraction()
    {

        GameObject player = GameObject.Find("Player Character");

        if (player.GetComponent<Inventory>().hasItem("cereal box")) 
        {
            GameObject Smol = this.gameObject;
            Smol.GetComponent<SmolController>().ShouldFollow(true);
        }
        //Todo: dialog system. else { }


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
