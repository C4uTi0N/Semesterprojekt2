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
        Vector3 spawnLocationOffest = new Vector2(spawnLocation.position.x - 1, spawnLocation.position.y);

        player.position = spawnLocation.position;
        smol.position = spawnLocationOffest;
    }
}
