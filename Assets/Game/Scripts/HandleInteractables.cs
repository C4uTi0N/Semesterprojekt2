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
    private PlayerController playerController;
    public UnityEngine.Events.UnityEvent continueDialogue;

    private void Awake()
    {
        player = GetComponent<Transform>();
        textObj = GameObject.Find("Press [E] to interact");
        interactable = textObj.GetComponent<Text>();
        playerController = GetComponent<PlayerController>();
    }

    void handleInteraction(Interactable hit)
    {
        interactable.text = hit.getInteractableText();

        if (Input.GetKeyDown(KeyCode.E))
        {
            hit.onInteraction();
        }
    }

    void handleInteraction(ContiniousInteractable hit)
    {
        interactable.text = hit.getInteractableText();

        if (Input.GetKeyDown(KeyCode.E))
        {
            hit.onInteractionStart();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            hit.onInteractionEnd();
        }
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfoForward;
        RaycastHit hitInfoInwards;
        Vector3 playerForward = player.TransformDirection(Vector3.right);
        Vector3 playerInwards = Vector3.forward;

        bool forwardRaycast = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerForward, out hitInfoForward, 1.1f);
        bool inwardsRaycast = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerInwards, out hitInfoInwards, 1.3f);

        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerForward, Color.red, 1.1f);
        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerInwards, Color.red, 1.3f);

        if (forwardRaycast)
        {

            var forwardInteractable = hitInfoForward.collider.GetComponent<Interactable>();
            if (forwardInteractable != null)
            {
                handleInteraction(forwardInteractable);
            }

            var forwardContinousInteractable = hitInfoForward.collider.GetComponent<ContiniousInteractable>();

            if (forwardContinousInteractable != null)
            {
                handleInteraction(forwardContinousInteractable);
                
                
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
            }
            else
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
        }
        else
        {
            colorForwards = Color.cyan;
        }

        Color colorInwards = Color.cyan;
        if (inwardsRaycast)
        {
            colorInwards = Color.green;
        }
        else
        {
            colorInwards = Color.cyan;
        }

        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerForward * 1.1f, colorForwards);
        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerInwards * 1.3f, colorInwards);
    }
}
