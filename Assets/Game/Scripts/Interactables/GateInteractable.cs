using UnityEngine;

public class GateInteractable : MonoBehaviour, Interactable
{
    private Transform gate;
    public UnityEngine.Events.UnityEvent onInteract;

    public string getInteractableText()
    {
        return "press [E] to open the gate";
    }

    public void onInteraction()
    {
        gate.transform.Translate(Vector3.up * 2.2f);
        onInteract.Invoke();
    }

    private void Awake()
    {
        gate = GetComponent<Transform>();
    }
}