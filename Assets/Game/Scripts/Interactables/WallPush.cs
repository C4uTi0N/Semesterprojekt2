using UnityEngine;

public class WallPush : MonoBehaviour, Interactable
{
    public Transform pushWall;

    public string getInteractableText()
    {
        return "press 'e' to push the wall";
    }

    public void onInteraction()
    {
        pushWall.transform.Translate(Vector3.right * 2.2f);
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void Awake()
    {
        pushWall = GetComponent<Transform>();
    }
}