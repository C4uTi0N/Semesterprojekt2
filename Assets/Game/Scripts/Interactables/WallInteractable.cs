using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteractable : MonoBehaviour, Interactable
{
    public Transform gateWall;

    public string getInteractableText()
    {
        return "press 'e' to open the gate";
    }

    public void onInteraction()
    {
        gateWall.transform.Translate(Vector3.up * 2.2f);
    }

    private void Awake()
    {
        gateWall = GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
