using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public Transform player;
    public Transform smol;

    public void MoveThroughDoor(Transform spawnLocation)
    {
        Vector3 smolSpawnLocation = spawnLocation.position + new Vector3(-0.5f, -1, 1);

        player.position = spawnLocation.position;
        smol.position = smolSpawnLocation;
    }
}