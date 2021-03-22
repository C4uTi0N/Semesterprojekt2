using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteractable : MonoBehaviour, Interactable
{
    public Transform gateWall;
    public UnityEngine.Events.UnityEvent onInteract;

    public string getInteractableText()
    {
        return "press [E] to open the gate";
    }

    public void onInteraction()
    {
        gateWall.transform.Translate(Vector3.up * 2.2f);
        onInteract.Invoke();
    }

    private void Awake()
    {
        gateWall = GetComponent<Transform>();
    }
}
