using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class HandleInteractables : MonoBehaviour
{
    private GameObject interactTextObj;
    private PlayerController playerController;
    private Inventory playerInv;
    private Transform player;

    private Text interactable;
    private Interactable inwardInteractable;
    private Interactable forwardInteractable;
    private Interactable forwardInnerInteractable;
    private ContiniousInteractable forwardContinousInteractable;

    private InMemoryVariableStorage yarnMemmory;

    public GameObject hitObject;

    private RaycastHit hitInfoForward;
    private RaycastHit hitInfoForwardInner;
    private RaycastHit hitInfoInward;

    public UnityEngine.Events.UnityEvent continueDialogue;

    private Vector3 playerForward;
    private bool raycastForward;

    private Vector3 playerForwardInner;
    private bool raycastForwardInner;

    private Vector3 playerInward;
    private bool raycastInward;

    private bool cutsceneRun;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerInv = GetComponent<Inventory>();
        player = GetComponent<Transform>();
        interactTextObj = GameObject.Find("Interact");
        interactable = GameObject.Find("Press [E] to interact").GetComponent<Text>();

        yarnMemmory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();
    }

    // Update is called once per frame
    private void Update()
    {
        yarnMemmory.TryGetValue<bool>("$cutsceneRunning", out bool cutsceneRunning);
        cutsceneRun = cutsceneRunning;

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
        forwardInteractable = hitInfoForward.collider.GetComponent<Interactable>();
        forwardContinousInteractable = hitInfoForward.collider.GetComponent<ContiniousInteractable>();

        if (forwardInteractable != null)
        {
            RayHit(hitInfoForward.collider.gameObject);

            interactable.text = forwardInteractable.getInteractableText();
            if (Input.GetKeyDown(KeyCode.E) && !cutsceneRun)
            {
                forwardInteractable.onInteraction();
            }
            return;
        }

        if (forwardContinousInteractable != null)
        {
            RayHit(hitInfoForward.collider.gameObject);

            interactable.text = forwardContinousInteractable.getInteractableText();
            if (Input.GetKey(KeyCode.E) && !cutsceneRun)
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
        forwardInnerInteractable = hitInfoForwardInner.collider.GetComponent<Interactable>();
        forwardContinousInteractable = hitInfoForwardInner.collider.GetComponent<ContiniousInteractable>();

        if (forwardInnerInteractable != null)
        {
            RayHit(hitInfoForwardInner.collider.gameObject);

            interactable.text = forwardInnerInteractable.getInteractableText();
            if (Input.GetKeyDown(KeyCode.E) && !cutsceneRun)
            {
                forwardInnerInteractable.onInteraction();
            }
        }

        if (forwardContinousInteractable != null)
        {
            RayHit(hitInfoForwardInner.collider.gameObject);

            interactable.text = forwardContinousInteractable.getInteractableText();
            if (Input.GetKey(KeyCode.E) && !cutsceneRun)
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
        inwardInteractable = hitInfoInward.collider.GetComponent<Interactable>();

        if (inwardInteractable != null)
        {
            RayHit(hitInfoInward.collider.gameObject);

            interactable.text = inwardInteractable.getInteractableText();
            if (Input.GetKeyDown(KeyCode.E) && !cutsceneRun)
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

                if (hitObject.GetComponent<SmolInteractable>())
                {
                    if (playerInv.hasItem("cereal box"))
                    {
                        interactTextObj.SetActive(true);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                interactTextObj.SetActive(true);
            }
        }

        if (hitObject == interactable)
        {
            if (!hitObject.GetComponent<DoorInteractable>())
                hitObject.SendMessage("RayHitEnter");

            if (hitObject.GetComponent<SmolInteractable>())
            {
                if (playerInv.hasItem("cereal box"))
                {
                    interactTextObj.SetActive(true);
                    return;
                }
                else
                {
                    return;
                }
            }
            interactTextObj.SetActive(true);
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