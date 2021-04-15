using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public Transform player;
    public Transform smol;

    public void MoveThroughDoor(Transform setLocation)
    {
        Vector3 setLocationOffest = new Vector3(setLocation.position.x - 1.5f, setLocation.position.y - 0.4f, setLocation.position.z - 1);

        player.position = setLocation.position;
        smol.position = setLocationOffest;
    }
}
