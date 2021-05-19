using UnityEngine;

[RequireComponent(typeof(FixedJoint))]
public class Movable : MonoBehaviour, ContiniousInteractable
{
    private GameObject box;
    private GameObject player;
    private SpriteRenderer sprite;
    private FixedJoint joint;

    private void Awake()
    {
        box = gameObject;
        player = GameObject.Find("Player Character");
        joint = GetComponent<FixedJoint>();
    }

    public string getInteractableText()
    {
        return "Hold E to push or pull";
    }

    public void onInteractionStart()
    {
        box.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<PlayerController>().isInteracting = true;
        joint.connectedBody = player.GetComponent<Rigidbody>();
    }

    public void onInteractionEnd()
    {
        joint.connectedBody = null;
        player.GetComponent<PlayerController>().isInteracting = false;
        box.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<PlayerController>().isInteracting = false;
    }
}