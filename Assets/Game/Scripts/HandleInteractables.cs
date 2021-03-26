using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleInteractables : MonoBehaviour
{
    private Transform player;
    private GameObject textObj;
    private Text interactable;
    public UnityEngine.Events.UnityEvent continueDialogue;

    private void Awake()
    {
        player = GetComponent<Transform>();
        textObj = GameObject.Find("Press [E] to interact");
        interactable = textObj.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfoForward;
        RaycastHit hitInfoInwards;
        Vector3 playerForward = player.TransformDirection(Vector3.right);
        Vector3 playerInwards = Vector3.forward;

        bool forwardRaycast = Physics.Raycast(player.position, playerForward, out hitInfoForward, 1.1f);
        bool inwardsRaycast = Physics.Raycast(player.position, playerInwards, out hitInfoInwards, 1.3f);

        if (forwardRaycast)
        {
            var hitItemForward = hitInfoForward.collider.GetComponent<Interactable>();
            if (hitItemForward != null)
            {
                interactable.text = hitItemForward.getInteractableText();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hitItemForward.onInteraction();
                }
            }

        }
        else
        {
            if (inwardsRaycast)
            {
                var hitItemInwards = hitInfoInwards.collider.GetComponent<Interactable>();
                if (hitItemInwards != null)
                {
                    interactable.text = hitItemInwards.getInteractableText();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hitItemInwards.onInteraction();
                    }
                }
            } else 
            { 
                if (interactable.text != "")
                { 
                    interactable.text = ""; 
                } 
            }
        }



        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            continueDialogue.Invoke();
        }

        Color colorForwards = Color.cyan;
        if (forwardRaycast)
        {
            colorForwards = Color.green;
        } else {
            colorForwards = Color.cyan;
        }

        Color colorInwards = Color.cyan;
        if (inwardsRaycast)
        {
            colorInwards = Color.green;
        } else {
            colorInwards = Color.cyan;
        }

        Debug.DrawRay(player.position, playerForward * 1.1f, colorForwards);
        Debug.DrawRay(player.position, playerInwards * 1.3f, colorInwards);
    }
}
