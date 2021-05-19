using UnityEngine;
using Yarn.Unity;

public class Highlighter : MonoBehaviour
{
    private SpriteRenderer sprite;
    private InMemoryVariableStorage yarnMemmory;
    private Inventory playerInv;
    private HandleInteractables handleInteractables;

    private bool shouldHighlight = false;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        yarnMemmory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();

        playerInv = GameObject.Find("Player Character").GetComponent<Inventory>();
        handleInteractables = GameObject.Find("Player Character").GetComponent<HandleInteractables>();
    }

    private void Update()
    {
        yarnMemmory.TryGetValue<bool>("$highlightPickup", out bool outputPickup);
        yarnMemmory.TryGetValue<bool>("$highlightMovable", out bool outputMovable);
        yarnMemmory.TryGetValue<bool>("$highlightSmol", out bool outputSmol);

        if (outputPickup)
        {
            if (GetComponent<PickupInteractable>())
            {
                sprite.color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 1));
            }
        }
        else if (GetComponent<PickupInteractable>())
        {
            sprite.color = Color.white;
        }

        if (outputMovable)
        {
            if (GetComponent<Movable>())
            {
                sprite.color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 1));
            }
        }
        else if (GetComponent<Movable>())
        {
            sprite.color = Color.white;
        }

        if (outputSmol)
        {
            if (GetComponent<SmolInteractable>())
            {
                sprite.color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 1));
            }
        }
        else if (GetComponent<SmolInteractable>())
        {
            sprite.color = Color.white;
        }

        if (shouldHighlight)
        {
            if (handleInteractables.hitObject.GetComponent<SmolInteractable>())
            {
                if (playerInv.hasItem("cereal box"))
                {
                    sprite.color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 1));
                    return;
                }
                else
                {
                    return;
                }
            }
            sprite.color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 1));
        }
    }

    private void RayHitEnter()
    {
        shouldHighlight = true;
        Debug.Log("RayHit: Enter");
    }

    private void RayHitStay()
    {
        Debug.Log("RayHit: Stay");
    }

    private void RayHitExit()
    {
        shouldHighlight = false;
        sprite.color = Color.white;
        Debug.Log("RayHit: Exit");
    }
}