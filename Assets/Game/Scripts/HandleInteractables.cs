using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleInteractables : MonoBehaviour
{
    private Transform player;
    public Text gate;

    private void Awake()
    {
        player = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        Vector3 playerForward = player.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(player.transform.position, playerForward, out hitInfo, 1.5f))
        {
            var hitItem = hitInfo.collider.GetComponent<Interactable>();
            if(hitItem != null)
            {
                gate.text = hitItem.getInteractableText();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hitItem.onInteraction();
                }
            }
        }
        else { if (gate.text != "") { gate.text = ""; } }



        //Ray ray = Physics.Raycast(player.position, player.TransformDirection(Vector3.forward), 1.5f);
        Debug.DrawRay(player.position, player.TransformDirection(Vector3.forward) * 1.5f, Color.red);                                          // Middle
    }
}
