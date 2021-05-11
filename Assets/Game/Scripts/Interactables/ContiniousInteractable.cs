using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ContiniousInteractable
{
    public string getInteractableText();

    void onInteractionStart();

    void onInteractionEnd();


}
