using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleInteractables : MonoBehaviour
{
    private Transform player;
    private Text interactable;
    private GameObject interactTextObj;
    private PlayerController playerController;
    public UnityEngine.Events.UnityEvent continueDialogue;

    private void Awake()
    {
        player = GetComponent<Transform>();
        interactTextObj = GameObject.Find("Interact");
        interactable = GameObject.Find("Press [E] to interact").GetComponent<Text>();
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

        if (Input.GetKey(KeyCode.E))
        {
            hit.onInteractionStart();
        } else
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

        bool forwardRaycast = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 3, 0), playerForward, out hitInfoForward, 1.1f);
        bool inwardsRaycast = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerInwards, out hitInfoInwards, 1.3f);

        if (forwardRaycast)
        {
            interactTextObj.SetActive(true);
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
                interactTextObj.SetActive(true);
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
                interactTextObj.SetActive(false);
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

        Color colorForwards = Color.yellow;
        if (forwardRaycast)
        {
            colorForwards = Color.green;
        }
        else
        {
            colorForwards = Color.yellow;
        }

        Color colorInwards = Color.yellow;
        if (inwardsRaycast)
        {
            colorInwards = Color.green;
        }
        else
        {
            colorInwards = Color.yellow;
        }

        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 3, 0), playerForward * 1.1f, colorForwards);
        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerInwards * 1.3f, colorInwards);
    }
}
