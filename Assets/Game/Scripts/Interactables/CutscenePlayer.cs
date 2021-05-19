using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class CutscenePlayer : MonoBehaviour
{
    private InMemoryVariableStorage yarnMemmory;
    public UnityEngine.Events.UnityEvent onCollide;
    private PlayerController playerController;
    private bool cutscenePlaying = false;

    public GameObject wall;

    private void Awake()
    {
        yarnMemmory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();
        playerController = GameObject.Find("Player Character").GetComponent<PlayerController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Movable>())
        {
            if (wall != null)
            {
                wall.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                if (!cutscenePlaying)
                    StartCoroutine(Timer());
            }
        }
    }

    private IEnumerator Timer()
    {
        cutscenePlaying = true;
        yarnMemmory.SetValue("$cutsceneRunning", true);
        yield return new WaitForSeconds(0.5f);
        onCollide.Invoke();

        yield return new WaitForSeconds(2);
        yarnMemmory.SetValue("$cutsceneRunning", false);
        playerController.cameraFollow = true;
        gameObject.SetActive(false);
    }
}