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
        RaycastHit hitInfoForwardInner;
        RaycastHit hitInfoForward;
        RaycastHit hitInfoInwards;
        Vector3 playerForwardInner = player.TransformDirection(Vector3.right);
        Vector3 playerForward = player.TransformDirection(Vector3.right);
        Vector3 playerInwards = Vector3.forward;

        bool forwardInnerRaycast = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 3, 1), playerForwardInner, out hitInfoForwardInner, 1.1f);
        bool forwardRaycast = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 3, 0), playerForward, out hitInfoForward, 1.1f);
        bool inwardsRaycast = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerInwards, out hitInfoInwards, 1.3f);


        if (forwardRaycast)
        {
            
            var forwardInteractable = hitInfoForward.collider.GetComponent<Interactable>();
            var forwardContinousInteractable = hitInfoForward.collider.GetComponent<ContiniousInteractable>();

            if (forwardInteractable != null)
            {
                handleInteraction(forwardInteractable);
            }

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

        if (forwardInnerRaycast)
        {
            var forwardInteractable = hitInfoForwardInner.collider.GetComponent<Interactable>();
            var forwardContinousInteractable = hitInfoForwardInner.collider.GetComponent<ContiniousInteractable>();
            if (forwardInteractable != null)
            {
                handleInteraction(forwardInteractable);
            }

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

        if (forwardRaycast || forwardInnerRaycast || inwardsRaycast)
        {
            interactTextObj.SetActive(true);
        }
        else
        {
            interactTextObj.SetActive(false);
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
        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 3, 1), playerForward * 1.1f, colorForwards);
        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerInwards * 1.3f, colorInwards);
    }
}
