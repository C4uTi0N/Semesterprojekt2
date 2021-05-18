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
    private GameObject hitObject;
    private PlayerController playerController;
    private Interactable inwardInteractable;
    private Interactable forwardInteractable;
    private Interactable forwardInnerInteractable;
    private ContiniousInteractable forwardContinousInteractable;

    RaycastHit hitInfoForward;
    RaycastHit hitInfoForwardInner;
    RaycastHit hitInfoInward;

    public UnityEngine.Events.UnityEvent continueDialogue;


    Vector3 playerForward;
    bool raycastForward;

    Vector3 playerForwardInner;
    bool raycastForwardInner;

    Vector3 playerInward;
    bool raycastInward;

    private void Awake()
    {
        player = GetComponent<Transform>();
        interactTextObj = GameObject.Find("Interact");
        interactable = GameObject.Find("Press [E] to interact").GetComponent<Text>();
        playerController = GetComponent<PlayerController>();
    }


    // Update is called once per frame
    void Update()
    {
        playerForward = player.TransformDirection(Vector3.right);
        raycastForward = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 3, 0), playerForward, out hitInfoForward, 1.1f);

        playerForwardInner = player.TransformDirection(Vector3.right);
        raycastForwardInner = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 3, 1), playerForwardInner, out hitInfoForwardInner, 1.1f);

        playerInward = Vector3.forward;
        raycastInward = Physics.Raycast(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerInward, out hitInfoInward, 1.3f);


        RaycastColor();
        if (raycastForward)
        {
            RaycastForward();
        }

        if (raycastForwardInner)
        {
            RaycastForwardInner();
        }

        if (raycastInward)
        {
            RaycastInward();
        }

        if (!raycastForward && !raycastForwardInner && !raycastInward)
        {
            interactTextObj.SetActive(false);

            if (interactable.text != "")
            {
                interactable.text = "";
            }

            RayHitExit();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            continueDialogue.Invoke();
        }
    }

    private void RaycastForward()
    {
        interactTextObj.SetActive(true);

        forwardInteractable = hitInfoForward.collider.GetComponent<Interactable>();
        forwardContinousInteractable = hitInfoForward.collider.GetComponent<ContiniousInteractable>();

        

        if (forwardInteractable != null)
        {
            RayHit(hitInfoForward.collider.gameObject);

            interactable.text = forwardInteractable.getInteractableText();
            if (Input.GetKeyDown(KeyCode.E))
            {
                forwardInteractable.onInteraction();
            }
            return;
        } 

        if (forwardContinousInteractable != null)
        {
            RayHit(hitInfoForward.collider.gameObject);

            interactable.text = forwardContinousInteractable.getInteractableText();
            if (Input.GetKey(KeyCode.E))
            {
                forwardContinousInteractable.onInteractionStart();
            }
            else
            {
                forwardContinousInteractable.onInteractionEnd();
            }
            return;
        }
    }

    private void RaycastForwardInner()
    {
        interactTextObj.SetActive(true);

        forwardInnerInteractable = hitInfoForwardInner.collider.GetComponent<Interactable>();
        forwardContinousInteractable = hitInfoForwardInner.collider.GetComponent<ContiniousInteractable>();

        if (forwardInnerInteractable != null)
        {
            RayHit(hitInfoForwardInner.collider.gameObject);

            interactable.text = forwardInnerInteractable.getInteractableText();
            if (Input.GetKeyDown(KeyCode.E))
            {
                forwardInnerInteractable.onInteraction();
            }
        }
        
        if (forwardContinousInteractable != null)
        {
            RayHit(hitInfoForwardInner.collider.gameObject);

            interactable.text = forwardContinousInteractable.getInteractableText();
            if (Input.GetKey(KeyCode.E))
            {
                forwardContinousInteractable.onInteractionStart();
            }
            else
            {
                forwardContinousInteractable.onInteractionEnd();
            }
        }
    }

    private void RaycastInward()
    {
        interactTextObj.SetActive(true);

        inwardInteractable = hitInfoInward.collider.GetComponent<Interactable>();

        if (inwardInteractable != null)
        {
            RayHit(hitInfoInward.collider.gameObject);

            interactable.text = inwardInteractable.getInteractableText();
            if (Input.GetKeyDown(KeyCode.E))
            {
                inwardInteractable.onInteraction();
            }
        }
    }

    private void RaycastColor()
    {
        Color colorInward = Color.yellow;
        if (raycastInward)
        {
            colorInward = Color.green;
        }
        else
        {
            colorInward = Color.yellow;
        }
        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 2, 0), playerInward * 1.3f, colorInward);


        Color colorForwards = Color.yellow;
        if (raycastForward || raycastForwardInner)
        {
            colorForwards = Color.green;
        }
        else
        {
            colorForwards = Color.yellow;
        }
        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 3, 0), playerForward * 1.1f, colorForwards);
        Debug.DrawRay(player.position + new Vector3(0, playerController.capsCollider.height / 3, 1), playerForwardInner * 1.1f, colorForwards);
    }

    private void RayHit(GameObject interactable)
    {

        if (hitObject == null)
        {
            if (interactable != null)
            {
                hitObject = interactable;
                if (!hitObject.GetComponent<DoorInteractable>())
                    hitObject.SendMessage("RayHitEnter");
            }
        }

        if (hitObject == interactable)
        {
            if (!hitObject.GetComponent<DoorInteractable>())
                hitObject.SendMessage("RayHitEnter");
        }
    }

    private void RayHitExit()
    {
        if (hitObject != null)
        {
            if (!hitObject.GetComponent<DoorInteractable>())
                hitObject.SendMessage("RayHitExit");
            hitObject = null;
        }
    }
}