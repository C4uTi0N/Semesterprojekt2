using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public Transform player;
    public Transform smol;

    public void MoveThroughDoor(Transform spawnLocation)
    {
        Vector3 spawnLocationOffest = new Vector3(spawnLocation.position.x - 0.5f, spawnLocation.position.y, 1.5f);

        player.position = spawnLocation.position;
        smol.position = spawnLocationOffest;
    }
}
